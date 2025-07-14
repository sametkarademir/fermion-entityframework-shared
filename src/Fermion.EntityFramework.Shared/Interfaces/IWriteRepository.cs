using Fermion.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fermion.EntityFramework.Shared.Interfaces;

/// <summary>
/// Defines write operations for entity repository with typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The key type</typeparam>
public interface IWriteRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    /// <summary>
    /// Gets the database context
    /// </summary>
    /// <returns>The database context</returns>
    DbContext GetDbContext();
    
    /// <summary>
    /// Gets the DbSet for the entity
    /// </summary>
    /// <returns>The DbSet for the entity</returns>
    DbSet<TEntity> GetDbSet();

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    /// <returns>The database transaction</returns>
    IDbContextTransaction BeginTransaction();
    
    /// <summary>
    /// Begins a database transaction asynchronously
    /// </summary>
    /// <returns>The database transaction</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Adds a new entity to the repository
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The added entity</returns>
    Task<TEntity> AddAsync(TEntity entity, bool saveImmediately = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a collection of entities to the repository
    /// </summary>
    /// <param name="entities">The collection of entities to add</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The added entities</returns>
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the repository
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated entity</returns>
    Task<TEntity> UpdateAsync(TEntity entity, bool saveImmediately = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a collection of entities in the repository
    /// </summary>
    /// <param name="entities">The collection of entities to update</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated entities</returns>
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity from the repository
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <param name="permanent">Indicates whether to permanently delete or soft delete</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The deleted entity</returns>
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, bool saveImmediately = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a collection of entities from the repository
    /// </summary>
    /// <param name="entities">The collection of entities to delete</param>
    /// <param name="permanent">Indicates whether to permanently delete or soft delete</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The deleted entities</returns>
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, bool saveImmediately = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Defines write operations for entity repository without typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public interface IWriteRepository<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// Gets the database context
    /// </summary>
    /// <returns>The database context</returns>
    DbContext GetDbContext();
    
    /// <summary>
    /// Gets the DbSet for the entity
    /// </summary>
    /// <returns>The DbSet for the entity</returns>
    DbSet<TEntity> GetDbSet();

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    /// <returns>The database transaction</returns>
    IDbContextTransaction BeginTransaction();
    
    /// <summary>
    /// Begins a database transaction asynchronously
    /// </summary>
    /// <returns>The database transaction</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Adds a new entity to the repository
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The added entity</returns>
    Task<TEntity> AddAsync(TEntity entity, bool saveImmediately = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a collection of entities to the repository
    /// </summary>
    /// <param name="entities">The collection of entities to add</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The added entities</returns>
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the repository
    /// </summary>
    /// <param name="entity">The entity to update</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated entity</returns>
    Task<TEntity> UpdateAsync(TEntity entity, bool saveImmediately = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a collection of entities in the repository
    /// </summary>
    /// <param name="entities">The collection of entities to update</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated entities</returns>
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an entity from the repository
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    /// <param name="permanent">Indicates whether to permanently delete or soft delete</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The deleted entity</returns>
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, bool saveImmediately = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a collection of entities from the repository
    /// </summary>
    /// <param name="entities">The collection of entities to delete</param>
    /// <param name="permanent">Indicates whether to permanently delete or soft delete</param>
    /// <param name="saveImmediately">Indicates whether to save changes immediately</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The deleted entities</returns>
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, bool saveImmediately = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous save operation</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}