using Fermion.Domain.Shared.Interfaces;

namespace Fermion.EntityFramework.Shared.Interfaces;

/// <summary>
/// Defines repository operations combining read and write operations with typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The key type</typeparam>
public interface IRepository<TEntity, TKey> :
    IReadRepository<TEntity, TKey>,
    IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}

/// <summary>
/// Defines repository operations combining read and write operations without typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public interface IRepository<TEntity> :
    IReadRepository<TEntity>,
    IWriteRepository<TEntity>
    where TEntity : class, IEntity
{
}