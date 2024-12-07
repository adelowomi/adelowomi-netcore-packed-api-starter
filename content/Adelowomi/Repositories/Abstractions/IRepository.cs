using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Adelowomi.Repositories.Abstractions;

/// <summary>
/// Interface for generic repository operations
/// </summary>
public interface IRepository<T> where T : class
{
    // Query Methods
    Task<T?> GetByIdAsync(int id);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
        bool asNoTracking = true);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    // Command Methods
    Task<T> AddAsync(T entity);
    Task<List<T>> AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);

    // Pagination
    Task<(List<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

    // Advanced Query Methods
    IQueryable<T> Query(bool asNoTracking = true);
    Task<List<TResult>> SelectAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
}
