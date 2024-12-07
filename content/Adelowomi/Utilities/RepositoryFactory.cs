using System;
using Adelowomi.Repositories;
using Adelowomi.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Adelowomi.Utilities;

public interface IRepositoryFactory
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}

public class RepositoryFactory : IRepositoryFactory
{
    private readonly DbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    public RepositoryFactory(DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);

        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<TEntity>(_context);
        }

        return (IRepository<TEntity>)_repositories[type];
    }
}