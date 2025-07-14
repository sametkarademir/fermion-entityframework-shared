using System.Linq.Expressions;
using Fermion.Domain.Exceptions.Types;
using Fermion.Domain.Shared.Interfaces;
using Fermion.EntityFramework.Shared.DTOs.Pagination;
using Fermion.EntityFramework.Shared.DTOs.Sorting;
using Fermion.EntityFramework.Shared.Extensions;
using Fermion.EntityFramework.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Fermion.EntityFramework.Shared.Repositories;

/// <summary>
/// Implementation of read-only repository pattern for Entity Framework with typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The key type</typeparam>
/// <typeparam name="TContext">The database context type</typeparam>
public class ReadRepository<TEntity, TKey, TContext>(TContext context) :
    IReadRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TContext : DbContext
{
    public IQueryable<TEntity> GetQueryable(bool ignoreFilters = false)
    {
        var queryable = context.Set<TEntity>();
        return ignoreFilters ? queryable.IgnoreQueryFilters() : queryable;
    }

    public async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        var entity = await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        if (entity is null) throw new AppEntityNotFoundException($"{typeof(TEntity).Name} not found");
        return entity;
    }

    public async Task<TEntity> GetAsync(
        TKey id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false, bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        var entity = await queryable.FirstOrDefaultAsync(item => Equals(item.Id, id), cancellationToken);
        if (entity is null) throw new AppEntityNotFoundException($"{typeof(TEntity).Name} not found");
        return entity;
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (orderBy != null) queryable = orderBy(queryable);
        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<PageableResponseDto<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (orderBy != null) queryable = orderBy(queryable);
        return await queryable.ToPageableAsync(index, size, cancellationToken);
    }

    public async Task<PageableResponseDto<TEntity>> GetListWithSortAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        List<SortRequestDto>? sorts = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (sorts != null) queryable = queryable.ApplySort(sorts);
        return await queryable.ToPageableAsync(index, size, cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.CountAsync(cancellationToken);
    }
}

/// <summary>
/// Implementation of read-only repository pattern for Entity Framework without typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TContext">The database context type</typeparam>
public class ReadRepository<TEntity, TContext>(TContext context) :
    IReadRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    public IQueryable<TEntity> GetQueryable(bool ignoreFilters = false)
    {
        var queryable = context.Set<TEntity>();

        if (ignoreFilters)
        {
            return queryable.IgnoreQueryFilters();
        }

        return queryable;
    }

    public async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        var entity = await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        if (entity is null) throw new AppEntityNotFoundException($"{typeof(TEntity).Name} not found");
        return entity;
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (orderBy != null) queryable = orderBy(queryable);
        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<PageableResponseDto<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (orderBy != null) queryable = orderBy(queryable);
        return await queryable.ToPageableAsync(index, size, cancellationToken);
    }

    public async Task<PageableResponseDto<TEntity>> GetListWithSortAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        List<SortRequestDto>? sorts = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (include != null) queryable = include(queryable);
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (sorts != null) queryable = queryable.ApplySort(sorts);
        return await queryable.ToPageableAsync(index, size, cancellationToken);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
         bool enableTracking = true,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable();
        if (predicate != null) queryable = queryable.Where(predicate);
        if (!enableTracking) queryable = queryable.AsNoTracking();
        if (withDeleted) queryable = queryable.IgnoreQueryFilters();
        return await queryable.CountAsync(cancellationToken);
    }
}