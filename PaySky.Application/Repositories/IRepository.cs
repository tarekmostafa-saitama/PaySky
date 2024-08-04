﻿using System.Linq.Expressions;
using PaySky.Application.Repositories.Specifications;

namespace PaySky.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> criteria,
        bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includes);


    Task<IEnumerable<TEntity>> GetAllAsync(
        bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includes);


    Task<IEnumerable<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        params Expression<Func<TEntity, object>>[] includes);


    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);

    void RemoveRange(
        Expression<Func<TEntity, bool>> criteria);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria = null);
}