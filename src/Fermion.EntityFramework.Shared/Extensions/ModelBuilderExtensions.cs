using System.Linq.Expressions;
using Fermion.Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fermion.EntityFramework.Shared.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyGlobalConfigurations(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var entityClrType = entityType.ClrType;
            var entityInterfaces = entityClrType.GetInterfaces();
            var isIEntity =
                entityInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>));

            if (isIEntity)
            {
                var idProperty = entityClrType.GetProperty("Id");
                if (idProperty != null)
                {
                    builder.Entity(entityClrType)
                        .Property(idProperty.Name)
                        .ValueGeneratedOnAdd();
                }
            }

            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var filter = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)),
                        Expression.Constant(false)
                    ),
                    parameter
                );

                builder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }

            if (typeof(IHasConcurrencyStamp).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                    .HasMaxLength(256)
                    .IsRequired()
                    .IsConcurrencyToken();
            }

            if (typeof(ICreationAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property
                        (nameof(ICreationAuditedObject.CreationTime))
                    .IsRequired();

                builder.Entity(entityType.ClrType)
                    .Property(nameof(ICreationAuditedObject.CreatorId))
                    .HasMaxLength(256)
                    .IsRequired(false);
            }

            if (typeof(IAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IAuditedObject.LastModificationTime))
                    .IsRequired(false);

                builder.Entity(entityType.ClrType)
                    .Property(nameof(IAuditedObject.LastModifierId))
                    .HasMaxLength(256)
                    .IsRequired(false);
            }

            if (typeof(IDeletionAuditedObject).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IDeletionAuditedObject.DeletionTime))
                    .IsRequired(false);

                builder.Entity(entityType.ClrType)
                    .Property(nameof(IDeletionAuditedObject.DeleterId))
                    .HasMaxLength(256)
                    .IsRequired(false);
            }

            if (typeof(IEntityCorrelationId).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IEntityCorrelationId.CorrelationId))
                    .HasMaxLength(100)
                    .IsRequired(false);
                builder.Entity(entityType.ClrType).HasIndex(nameof(IEntityCorrelationId.CorrelationId));
            }

            if (typeof(IEntitySnapshotId).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IEntitySnapshotId.SnapshotId))
                    .HasMaxLength(100)
                    .IsRequired(false);
                builder.Entity(entityType.ClrType).HasIndex(nameof(IEntitySnapshotId.SnapshotId));
            }

            if (typeof(IEntitySessionId).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType)
                    .Property(nameof(IEntitySessionId.SessionId))
                    .HasMaxLength(100)
                    .IsRequired(false);
                builder.Entity(entityType.ClrType).HasIndex(nameof(IEntitySessionId.SessionId));
            }
        }
    }

    public static void ApplyGlobalEntityConfigurations<T>(this EntityTypeBuilder<T> builder) where T : class
    {
        var entityType = typeof(T);
        var entityInterfaces = entityType.GetInterfaces();
        var isIEntity = entityInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>));

        if (isIEntity)
        {
            var idProperty = entityType.GetProperty("Id");
            if (idProperty != null)
            {
                builder.Property(idProperty.Name)
                    .ValueGeneratedOnAdd();
            }
        }

        if (typeof(ISoftDelete).IsAssignableFrom(entityType))
        {
            var parameter = Expression.Parameter(entityType, "entity");
            var filter = Expression.Lambda(
                Expression.Equal(
                    Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)),
                    Expression.Constant(false)
                ),
                parameter
            );

            builder.HasQueryFilter((LambdaExpression)filter);
        }

        if (typeof(IHasConcurrencyStamp).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .HasMaxLength(256)
                .IsRequired()
                .IsConcurrencyToken();
        }

        if (typeof(ICreationAuditedObject).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(ICreationAuditedObject.CreationTime))
                .IsRequired();

            builder.Property(nameof(ICreationAuditedObject.CreatorId))
                .HasMaxLength(256)
                .IsRequired(false);
        }

        if (typeof(IAuditedObject).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(IAuditedObject.LastModificationTime))
                .IsRequired(false);

            builder.Property(nameof(IAuditedObject.LastModifierId))
                .HasMaxLength(256)
                .IsRequired(false);
        }

        if (typeof(IDeletionAuditedObject).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(IDeletionAuditedObject.DeletionTime))
                .IsRequired(false);

            builder.Property(nameof(IDeletionAuditedObject.DeleterId))
                .HasMaxLength(256)
                .IsRequired(false);
        }

        if (typeof(IEntityCorrelationId).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(IEntityCorrelationId.CorrelationId))
                .HasMaxLength(100)
                .IsRequired(false);
            builder.HasIndex(nameof(IEntityCorrelationId.CorrelationId));
        }

        if (typeof(IEntitySnapshotId).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(IEntitySnapshotId.SnapshotId))
                .HasMaxLength(100)
                .IsRequired(false);
            builder.HasIndex(nameof(IEntitySnapshotId.SnapshotId));
        }

        if (typeof(IEntitySessionId).IsAssignableFrom(entityType))
        {
            builder.Property(nameof(IEntitySessionId.SessionId))
                .HasMaxLength(100)
                .IsRequired(false);
            builder.HasIndex(nameof(IEntitySessionId.SessionId));
        }
    }
}