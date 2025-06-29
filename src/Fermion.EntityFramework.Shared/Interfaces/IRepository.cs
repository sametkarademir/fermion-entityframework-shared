using Fermion.Domain.Shared.Interfaces;

namespace Fermion.EntityFramework.Shared.Interfaces;

public interface IRepository<TEntity, TKey> :
    IReadRepository<TEntity, TKey>,
    IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
}

public interface IRepository<TEntity> :
    IReadRepository<TEntity>,
    IWriteRepository<TEntity>
    where TEntity : class, IEntity
{
}