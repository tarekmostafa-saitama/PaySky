using PaySky.Application.Repositories;
using PaySky.Infrastructure.Persistence;

namespace PaySky.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
 
    private readonly ApplicationDbContext _context;
    private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Lazy<IVacancyRepository> _vacancyRepository;
    private readonly Lazy<IArchivedVacancyRepository> _archivedVacancyRepository;
    private readonly Lazy<IApplicationRepository> _applicationRepository;


    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _refreshTokenRepository = new Lazy<IRefreshTokenRepository>(() => new RefreshTokenRepository(_context));
        _vacancyRepository = new Lazy<IVacancyRepository>(() => new VacancyRepository (_context));
        _applicationRepository =  new Lazy<IApplicationRepository>(() => new ApplicationRepository(_context)); ;
        _archivedVacancyRepository =  new Lazy<IArchivedVacancyRepository>(() => new ArchivedVacancyRepository(_context)); ;

    }


    public IVacancyRepository VacancyRepository => _vacancyRepository.Value;

    public IApplicationRepository ApplicationRepository => _applicationRepository.Value;

    public IRefreshTokenRepository RefreshTokensRepository => _refreshTokenRepository.Value;
    public IArchivedVacancyRepository ArchivedVacancyRepository => _archivedVacancyRepository.Value;
    


    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}