using System.Linq.Expressions;
using Fermion.Domain.Shared.Interfaces;
using Fermion.EntityFramework.Shared.DTOs.Pagination;
using Fermion.EntityFramework.Shared.DTOs.Sorting;
using Microsoft.EntityFrameworkCore.Query;

namespace Fermion.EntityFramework.Shared.Interfaces;

/// <summary>
/// Defines read operations for entity repository with typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The key type</typeparam>
public interface IReadRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets a queryable instance for the entity
    /// </summary>
    /// <param name="ignoreFilters">Indicates whether to ignore query filters</param>
    /// <returns>A queryable instance of the entity</returns>
    IQueryable<TEntity> GetQueryable(bool ignoreFilters = false);

    /// <summary>
    /// Gets a single entity by predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The entity that matches the predicate</returns>
    /// <exception cref="AppEntityNotFoundException">Thrown when entity is not found</exception>
    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a single entity by its key
    /// </summary>
    /// <param name="id">The entity key</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The entity with the specified key</returns>
    /// <exception cref="AppEntityNotFoundException">Thrown when entity is not found</exception>
    Task<TEntity> GetAsync(
        TKey id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds an entity by predicate, returns null if not found
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The entity that matches the predicate or null if not found</returns>
    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the first entity that matches the predicate or null if not found
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The first entity that matches the predicate or null if not found</returns>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the single entity that matches the predicate or null if not found
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The single entity that matches the predicate or null if not found</returns>
    /// <exception cref="InvalidOperationException">Thrown when more than one entity matches the predicate</exception>
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets all entities that match the specified criteria
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="orderBy">The ordering function</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of entities that match the criteria</returns>
    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a paged list of entities that match the specified criteria
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="orderBy">The ordering function</param>
    /// <param name="index">The page index (0-based)</param>
    /// <param name="size">The page size</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A pageable response containing the entities and metadata</returns>
    Task<PageableResponseDto<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a paged list of entities with custom sorting
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="sorts">The list of sort requests</param>
    /// <param name="index">The page index (0-based)</param>
    /// <param name="size">The page size</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A pageable response containing the entities and metadata</returns>
    Task<PageableResponseDto<TEntity>> GetListWithSortAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        List<SortRequestDto>? sorts = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Checks if any entity matches the predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if any entity matches the predicate; otherwise, false</returns>
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the count of entities that match the predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The count of entities that match the predicate</returns>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
}

/// <summary>
/// Defines read operations for entity repository without typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public interface IReadRepository<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// Gets a queryable instance for the entity
    /// </summary>
    /// <param name="ignoreFilters">Indicates whether to ignore query filters</param>
    /// <returns>A queryable instance of the entity</returns>
    IQueryable<TEntity> GetQueryable(bool ignoreFilters = false);

    /// <summary>
    /// Gets a single entity by predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The entity that matches the predicate</returns>
    /// <exception cref="AppEntityNotFoundException">Thrown when entity is not found</exception>
    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finds an entity by predicate, returns null if not found
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The entity that matches the predicate or null if not found</returns>
    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the first entity that matches the predicate or null if not found
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The first entity that matches the predicate or null if not found</returns>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the single entity that matches the predicate or null if not found
    /// </summary>
    /// <param name="predicate">The predicate to filter the entity</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The single entity that matches the predicate or null if not found</returns>
    /// <exception cref="InvalidOperationException">Thrown when more than one entity matches the predicate</exception>
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets all entities that match the specified criteria
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="orderBy">The ordering function</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of entities that match the criteria</returns>
    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a paged list of entities that match the specified criteria
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="orderBy">The ordering function</param>
    /// <param name="index">The page index (0-based)</param>
    /// <param name="size">The page size</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A pageable response containing the entities and metadata</returns>
    Task<PageableResponseDto<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a paged list of entities with custom sorting
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="include">The include function for related entities</param>
    /// <param name="sorts">The list of sort requests</param>
    /// <param name="index">The page index (0-based)</param>
    /// <param name="size">The page size</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A pageable response containing the entities and metadata</returns>
    Task<PageableResponseDto<TEntity>> GetListWithSortAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        List<SortRequestDto>? sorts = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Checks if any entity matches the predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if any entity matches the predicate; otherwise, false</returns>
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the count of entities that match the predicate
    /// </summary>
    /// <param name="predicate">The predicate to filter entities</param>
    /// <param name="withDeleted">Indicates whether to include soft-deleted entities</param>
    /// <param name="enableTracking">Indicates whether to enable change tracking</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The count of entities that match the predicate</returns>
    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
}