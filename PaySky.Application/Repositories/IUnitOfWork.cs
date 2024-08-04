using PaySky.Shared.ServiceContracts;

namespace PaySky.Application.Repositories;

public interface IUnitOfWork : IDisposable, IScopedService
{
    public IVacancyRepository VacancyRepository { get; }
    public IArchivedVacancyRepository ArchivedVacancyRepository { get; }
    public IApplicationRepository ApplicationRepository { get; }

    public IRefreshTokenRepository RefreshTokensRepository { get; }
    Task<int> CommitAsync();
}