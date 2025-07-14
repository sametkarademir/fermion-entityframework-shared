using Fermion.Domain.Extensions.Claims;
using Fermion.Domain.Shared.Interfaces;
using Fermion.Domain.Shared.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Fermion.EntityFramework.Shared.Extensions;

/// <summary>
/// Provides extension methods for DbContext to handle aggregate root operations
/// </summary>
public static class DbContextAggregateRootExtensions
{
    /// <summary>
    /// Sets creation timestamps for entities that implement ICreationAuditedObject
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="httpContextAccessor">The HTTP context accessor to get user information</param>
    public static void SetCreationTimestamps(this DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e is
            {
                Entity: ICreationAuditedObject,
                State: EntityState.Added
            });

        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetCustomAttributes(typeof(ExcludeFromProcessingAttribute), true).Any())
            {
                continue;
            }

            var entity = (ICreationAuditedObject)entry.Entity;
            entity.CreationTime = DateTime.UtcNow;
            entity.CreatorId = httpContextAccessor.HttpContext?.User.GetUserIdToGuid();
        }
    }

    /// <summary>
    /// Sets modification timestamps for entities that implement IAuditedObject or IHasConcurrencyStamp
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="httpContextAccessor">The HTTP context accessor to get user information</param>
    public static void SetModificationTimestamps(this DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e => e is
            {
                Entity: IAuditedObject or IHasConcurrencyStamp,
                State: EntityState.Modified
            });

        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetCustomAttributes(typeof(ExcludeFromProcessingAttribute), true).Any())
            {
                continue;
            }

            if (entry.Entity is IAuditedObject)
            {
                var entity = (IAuditedObject)entry.Entity;
                entity.LastModificationTime = DateTime.UtcNow.ToUniversalTime();
                entity.LastModifierId = httpContextAccessor.HttpContext?.User.GetUserIdToGuid();
            }

            if (entry.Entity is IHasConcurrencyStamp)
            {
                var entity = (IHasConcurrencyStamp)entry.Entity;
                entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            }
        }
    }

    /// <summary>
    /// Sets soft delete timestamps for entities that implement IDeletionAuditedObject
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="httpContextAccessor">The HTTP context accessor to get user information</param>
    public static void SetSoftDelete(this DbContext context, IHttpContextAccessor httpContextAccessor)
    {
        var entries = context.ChangeTracker.Entries()
            .Where(e =>
                e is { Entity: IDeletionAuditedObject, State: EntityState.Modified } &&
                e.CurrentValues["IsDeleted"]!.Equals(true));

        foreach (var entry in entries)
        {
            if (entry.Entity.GetType().GetCustomAttributes(typeof(ExcludeFromProcessingAttribute), true).Any())
            {
                continue;
            }

            var entity = (IDeletionAuditedObject)entry.Entity;
            entity.DeletionTime = DateTime.UtcNow.ToUniversalTime();
            entity.DeleterId = httpContextAccessor.HttpContext?.User.GetUserIdToGuid();
        }
    }
}