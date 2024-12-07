using System;
using System.Linq.Expressions;
using Adelowomi.Models.UtilityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Adelowomi.Extensions;

public static class PaginationExtensions
{
    /// <summary>
    /// Creates a PagedCollection from an IQueryable
    /// </summary>
    public static async Task<PagedCollection<T>> ToPagedCollectionAsync<T>(
        this IQueryable<T> query,
        PagingOptions options,
        IUrlHelper urlHelper,
        string routeName,
        object? routeValues = null)
    {
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((options.PageNumber - 1) * options.PageSize)
            .Take(options.PageSize)
            .ToListAsync();

        return new PagedCollection<T>(
            items, 
            totalCount, 
            options.PageNumber, 
            options.PageSize,
            urlHelper,
            routeName,
            routeValues);
    }

    /// <summary>
    /// Applies sorting to a query based on pagination parameters
    /// </summary>
    public static IQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        string? sortBy,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortBy)) return query;

        var property = typeof(T).GetProperty(sortBy, 
            System.Reflection.BindingFlags.IgnoreCase | 
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.Instance);

        if (property == null) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        var methodName = sortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
        var resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExp));

        return query.Provider.CreateQuery<T>(resultExp);
    }

    /// <summary>
    /// Applies search filtering to a query
    /// </summary>
    public static IQueryable<T> ApplySearch<T>(
        this IQueryable<T> query,
        string? searchTerm,
        params Expression<Func<T, string>>[] searchProperties)
    {
        if (string.IsNullOrEmpty(searchTerm) || searchProperties.Length == 0)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var searchMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var searchValue = Expression.Constant(searchTerm.ToLower());

        Expression? combinedExpression = null;
        foreach (var propertyExp in searchProperties)
        {
            var propertyName = (propertyExp.Body as MemberExpression)?.Member.Name;
            if (propertyName == null) continue;

            var property = Expression.Property(parameter, propertyName);
            var toLower = Expression.Call(property, "ToLower", Type.EmptyTypes);
            var contains = Expression.Call(toLower, searchMethod!, searchValue);

            combinedExpression = combinedExpression == null
                ? contains
                : Expression.OrElse(combinedExpression, contains);
        }

        if (combinedExpression != null)
        {
            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }
}