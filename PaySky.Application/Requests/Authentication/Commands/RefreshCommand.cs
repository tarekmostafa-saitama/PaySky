using MediatR;
using PaySky.Application.Common.Authentication;
using PaySky.Application.Common.Models;
using PaySky.Application.Repositories;
using PaySky.Application.Requests.Authentication.Models;

namespace PaySky.Application.Requests.Authentication.Commands;

public record RefreshCommand(RefreshRequest RefreshRequest) : IRequest<IResponse<AuthenticateResponse>>;

internal sealed class RefreshCommandHandler(IRefreshTokenValidator refreshTokenValidator,
        IAuthenticateService authenticateService,
        IUnitOfWork unitOfWork)
    : IRequestHandler<RefreshCommand, IResponse<AuthenticateResponse>>
{
    public async Task<IResponse<AuthenticateResponse>> Handle(RefreshCommand request,
        CancellationToken cancellationToken)
    {
        var refreshRequest = request.RefreshRequest;
        var isValidRefreshToken = refreshTokenValidator.Validate(refreshRequest.RefreshToken);

        if (!isValidRefreshToken)
            return new Response<AuthenticateResponse>("Invalid Refresh Token", false);

        var refreshToken =
            await unitOfWork.RefreshTokensRepository.GetSingleAsync(x => x.Token == refreshRequest.RefreshToken);


        if (refreshToken == null)
            return new Response<AuthenticateResponse>("Invalid Refresh Token", false);

        unitOfWork.RefreshTokensRepository.Remove(refreshToken);
        await unitOfWork.CommitAsync();

        return Response.Success(await authenticateService.Authenticate(refreshToken.UserId, cancellationToken));
    }
}