using FluentValidation;
using Mapster;
using PaySky.Application.Common.Caching;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Application.Repositories.Specifications;
using PaySky.Application.Requests.Applications.Models;
using PaySky.Application.Requests.Vacancies.Commands;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;
using PaySky.Shared.CQRS;
using PaySky.Shared.Exceptions;
using ApplicationId = System.ApplicationId;

namespace PaySky.Application.Requests.Applications.Commands;

public record CreateApplicationCommand(Guid VacancyId) : ICommand<CreateApplicationResult>;

public record CreateApplicationResult(ApplicationVm ApplicationVm);

public class CreateApplicationCommandValidator : AbstractValidator<CreateApplicationCommand>
{
    public CreateApplicationCommandValidator()
    {
        RuleFor(x => x.VacancyId).NotEmpty().WithMessage("Vacancy Id is required");
       
    }
}

internal class CreateApplicationCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, ICurrentUserService currentUserService)
    : ICommandHandler<CreateApplicationCommand, CreateApplicationResult>
{
    public async Task<CreateApplicationResult> Handle(CreateApplicationCommand command, CancellationToken cancellationToken)
    {
        var vacancy =await unitOfWork.VacancyRepository
            .GetSingleAsync(x => x.Id == VacancyId.Of(command.VacancyId ) , false , x=>x.Applications);
        

        if (vacancy == null) throw new NotFoundException($"Vacancy with id = {command.VacancyId} not found");

        if(!vacancy.IsActive) throw new NotFoundException($"Vacancy with id = {command.VacancyId} is not active now");

        if (vacancy.ExpirationDate <= DateTimeOffset.Now) throw new NotFoundException($"Vacancy with id = {command.VacancyId} is expired");


        if(vacancy.Applications.Count>= vacancy.MaxApplications) throw new NotFoundException($"Vacancy with id = {command.VacancyId} reached max applications requests");


        var application =
            Domain.Entities.Application.Create(
                Domain.ValueObjects.ApplicationId.Of(Guid.NewGuid()),
                VacancyId.Of(command.VacancyId)
            );

        unitOfWork.ApplicationRepository.Add(application);
        await unitOfWork.CommitAsync();


        return new CreateApplicationResult(application.Adapt<ApplicationVm>());
    }
}