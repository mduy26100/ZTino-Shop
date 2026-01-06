# Repository Pattern

This document explains the repository pattern implementation for data access.

## Overview

The repository pattern:
- Abstracts data access from business logic
- Provides a collection-like interface for entities
- Enables unit testing with mock repositories

## Architecture

```
Application Layer                 Infrastructure Layer
       │                                  │
       ▼                                  ▼
┌─────────────────┐              ┌─────────────────────┐
│  IRepository<T> │◄─────────────│  Repository<T>      │
│  (interface)    │   implements │  (abstract base)    │
└─────────────────┘              └─────────────────────┘
       ▲                                  ▲
       │                                  │
┌─────────────────┐              ┌─────────────────────┐
│IProductRepository│◄────────────│  ProductRepository  │
│  (interface)    │   implements │  (concrete)         │
└─────────────────┘              └─────────────────────┘
```

---

## Generic Repository

### Interface

**Location**: `ZTino_Shop/src/Application.Common/Abstractions/Persistence/IRepository.cs`

| Method | Purpose |
|--------|---------|
| `GetAll(filter?, asNoTracking)` | Query with optional filter |
| `GetAllAsync()` | Get all entities |
| `GetByIdAsync(id)` | Find by primary key |
| `FindAsync(predicate)` | Find by condition |
| `FindOneAsync(predicate)` | Find single by condition |
| `AddAsync(entity)` | Add new entity |
| `AddRangeAsync(entities)` | Add multiple entities |
| `Update(entity)` | Update existing |
| `Remove(entity)` | Delete entity |
| `RemoveRange(entities)` | Delete multiple |
| `AnyAsync(predicate)` | Check existence |

### Implementation

**Location**: `ZTino_Shop/src/Infrastructure/Persistence/Repository.cs`

The abstract base class:
- Injects `ApplicationDbContext`
- Gets `DbSet<T>` for the entity type
- Implements all interface methods

---

## Feature Repositories

Each feature has specific repositories extending the base:

### Products

**Interface**: `ZTino_Shop/src/Application/Features/Products/v1/Repositories/`
- `IProductRepository`
- `ICategoryRepository`
- `IColorRepository`
- `ISizeRepository`
- `IProductColorRepository`
- `IProductVariantRepository`
- `IProductImageRepository`

**Implementation**: `ZTino_Shop/src/Infrastructure/Products/`

### Carts

**Interface**: `ZTino_Shop/src/Application/Features/Carts/v1/Repositories/ICartRepository.cs`

**Implementation**: `ZTino_Shop/src/Infrastructure/Carts/`

### Orders

**Interface**: `ZTino_Shop/src/Application/Features/Orders/v1/Repositories/IOrderRepository.cs`

**Implementation**: `ZTino_Shop/src/Infrastructure/Orders/`

---

## Repository Methods

### Query Methods

```csharp
// Get all with optional filter
IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, bool asNoTracking = true);

// Find by predicate
Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);

// Find single
Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true);
```

### Command Methods

```csharp
// Add
Task AddAsync(T entity);

// Update
void Update(T entity);

// Delete
void Remove(T entity);
```

---

## Unit of Work

The system uses an implicit Unit of Work via `DbContext`:
- All changes tracked by EF Core
- Single `SaveChangesAsync()` commits all changes
- Transaction wraps the save

### Saving Changes

Handlers call `SaveChangesAsync()` on the DbContext after operations:

```
Handler
    │
    ├── Repository.AddAsync(entity)
    ├── Repository.Update(entity)
    │
    └── _context.SaveChangesAsync()  ← Commits all changes
```

---

## Feature-Specific Methods

Feature repositories add domain-specific methods:

### ProductRepository Example

```csharp
interface IProductRepository : IRepository<Product, int>
{
    Task<Product?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<bool> SkuExistsAsync(string sku);
}
```

---

## Registration

**Location**: `ZTino_Shop/src/WebAPI/DependencyInjection/Features/`

Each feature registers its repositories:
- `ProductsFeatureRegistration.cs`
- `CartsFeatureRegistration.cs`
- `OrdersFeatureRegistration.cs`

Repositories are registered as **Scoped** (per-request lifetime).

---

## Best Practices

1. **Keep repositories focused**: One repository per aggregate root
2. **Use IQueryable for complex queries**: Compose queries in handlers
3. **Avoid generic repository anti-pattern**: Add domain-specific methods
4. **Include related data explicitly**: Use eager loading judiciously
5. **Return IEnumerable for materialized results**: Or IQueryable for deferred execution

---

## Testing

Repositories can be mocked:
- Create mock implementing interface
- Inject into handler under test
- Verify repository method calls

Or use in-memory database:
- Create `DbContextOptions` with in-memory provider
- Instantiate real repository with test context
