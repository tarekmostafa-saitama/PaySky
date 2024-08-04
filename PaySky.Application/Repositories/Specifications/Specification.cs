﻿using System.Linq.Expressions;

namespace PaySky.Application.Repositories.Specifications;

public class Specification<T> : ISpecification<T>
{
    // Where Setup
    public Expression<Func<T, bool>> Criteria { get; private set; }

    // Order Setup
    public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; private set; }

    // Pagination Setup
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    // Tracking Setup
    public bool IsTrackingEnabled { get; private set; }

    public Specification<T> ApplyOrderByAsc(Expression<Func<T, object>> orderBy)
    {
        OrderBy = query => query.OrderBy(orderBy);
        return this;
    }

    public Specification<T> ApplyOrderByDesc(Expression<Func<T, object>> orderBy)
    {
        OrderBy = query => query.OrderByDescending(orderBy);
        return this;
    }

    public Specification<T> ApplyTracking(bool isTracked)
    {
        IsTrackingEnabled = isTracked;
        return this;
    }

    public Specification<T> ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
        return this;
    }

    public Specification<T> ApplyCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
        return this;
    }
}