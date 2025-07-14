using System.Collections;
using Fermion.Domain.Shared.Interfaces;
using Fermion.EntityFramework.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fermion.EntityFramework.Shared.Repositories;

/// <summary>
/// Implementation of write repository pattern for Entity Framework with typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The key type</typeparam>
/// <typeparam name="TContext">The database context type</typeparam>
public class WriteRepository<TEntity, TKey, TContext>(TContext context) :
    IWriteRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TContext : DbContext
{
    public DbContext GetDbContext() => context;
    public DbSet<TEntity> GetDbSet() => context.Set<TEntity>();

    public IDbContextTransaction BeginTransaction() { return context.Database.BeginTransaction(); }
    public async Task<IDbContextTransaction> BeginTransactionAsync() { return await context.Database.BeginTransactionAsync(); }

    public async Task<TEntity> AddAsync(
        TEntity entity,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(
        ICollection<TEntity> entities,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        await context.AddRangeAsync(entities, cancellationToken);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<TEntity> UpdateAsync(
        TEntity entity,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(
        ICollection<TEntity> entities,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        context.UpdateRange(entities);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<TEntity> DeleteAsync(
        TEntity entity,
        bool permanent = false,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(
        ICollection<TEntity> entities,
        bool permanent = false,
        bool saveImmediately = false,
        CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeletedAsync(entities, permanent);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    #region Delete Protected Method

    private async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);

            if (entity is ISoftDelete fullAuditedEntity)
            {
                await SetEntityAsSoftDeletedAsync(fullAuditedEntity);
            }
            else
            {
                context.Remove(entity);
            }
        }
        else
        {
            context.Remove(entity);
        }
    }

    private void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        var hasEntityHaveOneToOneRelation =
            context
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException("Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key.");
    }

    private async Task SetEntityAsSoftDeletedAsync(ISoftDelete entity)
    {
        if (entity.IsDeleted)
            return;

        var navigations = context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is
            {
                IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
            })
            .ToList();
        foreach (var navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            var navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    var query = context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType()).ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (ISoftDelete navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    var query = context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((ISoftDelete)navValue);
            }
        }

        entity.IsDeleted = true;
        context.Update(entity);
    }

    private IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        var queryProviderType = query.Provider.GetType();
        var createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                .MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((ISoftDelete)x).IsDeleted);
    }

    private async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }

    #endregion
}

/// <summary>
/// Implementation of write repository pattern for Entity Framework without typed key
/// </summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TContext">The database context type</typeparam>
public class WriteRepository<TEntity, TContext>(TContext context) :
    IWriteRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    public DbContext GetDbContext() => context;
    public DbSet<TEntity> GetDbSet() => context.Set<TEntity>();

    public IDbContextTransaction BeginTransaction() => context.Database.BeginTransaction();
    public async Task<IDbContextTransaction> BeginTransactionAsync() => await context.Database.BeginTransactionAsync();

    public async Task<TEntity> AddAsync(TEntity entity, bool saveImmediately = false, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        if (saveImmediately) await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false, CancellationToken cancellationToken = default)
    {
        await context.AddRangeAsync(entities, cancellationToken);
        if (saveImmediately) await this.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, bool saveImmediately = false, CancellationToken cancellationToken = default)
    {
        context.Update(entity);
        if (saveImmediately) await this.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, bool saveImmediately = false, CancellationToken cancellationToken = default)
    {
        context.UpdateRange(entities);
        if (saveImmediately) await this.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, bool saveImmediately = false, CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        if (saveImmediately) await this.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false, bool saveImmediately = false, CancellationToken cancellationToken = default)
    {
        await SetEntityAsDeletedAsync(entities, permanent);
        if (saveImmediately) await this.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    #region Delete Protected Method

    private async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);

            if (entity is ISoftDelete fullAuditedEntity)
            {
                await SetEntityAsSoftDeletedAsync(fullAuditedEntity);
            }
            else
            {
                context.Remove(entity);
            }
        }
        else
        {
            context.Remove(entity);
        }
    }

    private void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        var hasEntityHaveOneToOneRelation =
            context
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException("Entity has one-to-one relationship. Soft Delete causes problems if you try to create entry again by same foreign key.");
    }

    private async Task SetEntityAsSoftDeletedAsync(ISoftDelete entity)
    {
        if (entity.IsDeleted)
            return;

        var navigations = context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is
            {
                IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade
            })
            .ToList();
        foreach (var navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            var navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    var query = context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType()).ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (ISoftDelete navValueItem in (IEnumerable)navValue)
                    await SetEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    var query = context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await SetEntityAsSoftDeletedAsync((ISoftDelete)navValue);
            }
        }

        entity.IsDeleted = true;
        context.Update(entity);
    }

    private IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        var queryProviderType = query.Provider.GetType();
        var createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                .MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException("CreateQuery<TElement> method is not found in IQueryProvider.");
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, [query.Expression])!;
        return queryProviderQuery.Where(x => !((ISoftDelete)x).IsDeleted);
    }

    private async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (var entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }

    #endregion
}