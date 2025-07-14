# Fermion.EntityFramework.Shared

Fermion.EntityFramework.Shared is a library that provides Entity Framework Core integration with advanced repository pattern implementation, query building, and data access utilities.

## Features

- Repository pattern implementation
- Read/Write repository separation
- Query building support
- Pagination and sorting
- Soft delete filtering
- Audit logging
- Aggregate root support
- Model configuration

## Installation

```bash
   dotnet add package Fermion.EntityFramework.Shared
```

## Content

### Repository Pattern

#### Core Interfaces
- `IRepository<TEntity, TKey>`: Base repository interface
    - Basic CRUD operations
    - Entity tracking
    - Save changes

- `IReadRepository<TEntity, TKey>`: Read-only repository interface
    - Query operations
    - Filtering
    - Sorting
    - Pagination
    - Include support
    - Projection support

- `IWriteRepository<TEntity, TKey>`: Write-only repository interface
    - Insert operations
    - Update operations
    - Delete operations
    - Bulk operations
    - Transaction support

#### Repository Implementations
- `EfRepositoryBase<TEntity, TKey>`: Base repository implementation
    - DbContext integration
    - Entity tracking
    - Change detection
    - Concurrency handling

- `ReadRepository<TEntity, TKey>`: Read-only repository implementation
    - Query building
    - Filtering
    - Sorting
    - Pagination
    - Include support
    - Projection support

- `WriteRepository<TEntity, TKey>`: Write-only repository implementation
    - Insert operations
    - Update operations
    - Delete operations
    - Bulk operations
    - Transaction support

### Query Building

#### Queryable Extensions
- `ApplySort`: Applies sorting to queryable
    - Single field sorting
    - Multiple field sorting
    - Dynamic sorting support

- `ToPageableAsync`: Converts queryable to pageable resource
    - Page size control
    - Page number control
    - Total count calculation
    - Pagination metadata

### DTOs

#### Pagination
- `PageableResponseMetaDto`: Pagination metadata
    - Current page
    - Page size
    - Total pages
    - Total items

- `PageableResourceDto<T>`: Pageable resource
    - Items
    - Total count
    - Pagination request

#### Sorting
- `SortRequestDto`: Sort request parameters
    - Field name
    - Sort order
    - Validation

- `SortOrderTypes`: Sort order types
    - Ascending
    - Descending

## Usage

### Repository Implementation

```csharp
public class UserRepository : EfRepositoryBase<User, Guid>, IUserRepository
{
    public UserRepository(YourDbContext context) : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await Query.GetAsync(x => x.Email == email);
    }
}
```

## Features

- Repository pattern implementation
- Read/Write separation
- Query building
- Pagination and sorting
- Soft delete filtering
- Model configuration
- Concurrency handling
- Transaction support