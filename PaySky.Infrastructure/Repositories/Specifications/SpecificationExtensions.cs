using Microsoft.EntityFrameworkCore;
using PaySky.Application.Repositories.Specifications;

namespace PaySky.Infrastructure.Repositories.Specifications;

public static class SpecificationExtensions
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec) where T : class
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = spec.OrderBy(query);
        }

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        query = spec.IsTrackingEnabled ? query : query.AsNoTracking();

        return query;
    }
}