using Fermion.Domain.Shared.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fermion.EntityFramework.Shared.Repositories.Abstractions;

public interface IWriteRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task<TEntity> AddAsync(TEntity entity, bool saveImmediately = false);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false);

    Task<TEntity> UpdateAsync(TEntity entity, bool saveImmediately = false);
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false);

    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, bool saveImmediately = false);
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, bool saveImmediately = false);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
}


public interface IWriteRepository<TEntity> where TEntity : class, IEntity
{
    Task<TEntity> AddAsync(TEntity entity, bool saveImmediately = false);
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false);

    Task<TEntity> UpdateAsync(TEntity entity, bool saveImmediately = false);
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false);

    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, bool saveImmediately = false);
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, bool saveImmediately = false);

    Task SaveChangesAsync();
    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
}