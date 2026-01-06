# Database Access

This document explains the database layer implementation using Entity Framework Core.

## Overview

- **ORM**: Entity Framework Core 8
- **Database**: SQL Server
- **Context**: `ApplicationDbContext`
- **Pattern**: Repository Pattern

## ApplicationDbContext

**Location**: `ZTino_Shop/src/Infrastructure/Persistence/ApplicationDbContext.cs`

### Inheritance

```
ApplicationDbContext
    └── IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
            └── DbContext
```

The context extends `IdentityDbContext` to integrate ASP.NET Core Identity tables.

### DbSets

| Category | DbSets |
|----------|--------|
| Auth | `RefreshTokens` |
| Products | `Products`, `ProductColors`, `ProductVariants`, `ProductImages`, `Categories`, `Colors`, `Sizes` |
| Carts | `Carts`, `CartItems` |
| Orders | `Orders`, `OrderItems`, `OrderAddresses`, `OrderPayments`, `OrderStatusHistories` |
| Finances | `Invoices` |
| Stats | `DailyRevenueStats`, `ProductSalesStats` |

---

## Connection Configuration

### Template

**Location**: `ZTino_Shop/src/WebAPI/appsettings.template.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=;Database=ZTinoShop;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Registration

**Location**: `ZTino_Shop/src/WebAPI/DependencyInjection/Infrastructure/PersistenceRegistration.cs`

The DbContext is registered with the connection string from configuration.

---

## Migrations

**Location**: `ZTino_Shop/src/Infrastructure/Persistence/Migrations/`

### Commands

```bash
# Add new migration
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/WebAPI

# Update database
dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI

# Remove last migration
dotnet ef migrations remove --project src/Infrastructure --startup-project src/WebAPI
```

### Design-Time Factory

**Location**: `ZTino_Shop/src/Infrastructure/Persistence/ApplicationDbContextFactory.cs`

Enables migrations without running the application.

---

## Data Seeding

**Location**: `ZTino_Shop/src/Infrastructure/Persistence/Seeds/`

Initial data is seeded on application startup via `app.SeedDataAsync()` in `Program.cs`.

---

## Custom Table Names

Identity tables are renamed for consistency:

| Default | Custom |
|---------|--------|
| `AspNetUsers` | `AppUsers` |
| `AspNetRoles` | `AppRoles` |

Configured in `OnModelCreating`.

---

## Transaction Handling

### Implicit Transactions

EF Core wraps `SaveChangesAsync()` in a transaction by default.

### Explicit Transactions

For complex operations spanning multiple saves:

```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // Multiple operations
    await _context.SaveChangesAsync();
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

---

## Query Optimization

### AsNoTracking

Read-only queries use `AsNoTracking()` for performance:
- Repository `GetAll()` defaults to no tracking
- Reduces memory usage
- Faster query execution

### Projections

Use `Select()` to project to DTOs instead of loading full entities.

### Eager Loading

Use `Include()` for related data:
- Configured in repository methods
- Avoids N+1 queries

---

## Database Abstraction

### IApplicationDbContext

**Location**: `ZTino_Shop/src/Application.Common/Abstractions/Persistence/IApplicationDbContext.cs`

Abstracts the DbContext for testability. Application layer depends on this interface, not concrete context.

---

## Related Documentation

- [Repository Pattern](repository-pattern.md) - Data access patterns
- [Entity Configurations](entity-configurations.md) - Fluent API configurations
