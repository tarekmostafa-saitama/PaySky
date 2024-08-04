using Microsoft.EntityFrameworkCore;
using PaySky.Application.Common.Services;
using PaySky.Application.Repositories;
using PaySky.Domain.Entities;
using PaySky.Domain.ValueObjects;
using PaySky.Infrastructure.Persistence;

namespace PaySky.Infrastructure.Repositories;

public class VacancyRepository(ApplicationDbContext context) : EfRepository<Vacancy>(context), IVacancyRepository;