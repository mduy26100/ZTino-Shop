# Dependency Injection

This document explains the service registration organization in ZTino_Shop.

## Overview

DI configuration is modular and organized by concern:

```
DependencyInjection/
├── ServiceCollectionExtensions.cs  # Main entry point
├── Application/                    # Application layer
├── Infrastructure/                 # Infrastructure layer
├── Security/                       # Authentication
├── Features/                       # Feature modules
└── CrossCutting/                   # CORS, versioning
```

**Location**: `ZTino_Shop/src/WebAPI/DependencyInjection/`

---

## Main Entry Point

**File**: `ServiceCollectionExtensions.cs`

```csharp
public static IServiceCollection AddApplicationServices(
    this IServiceCollection services, IConfiguration configuration)
{
    return services
        .AddInfrastructure(configuration)
        .AddApplicationCore()
        .AddSecurity(configuration)
        .AddFeatures()
        .AddCrossCutting();
}
```

Called from `Program.cs`:
```csharp
builder.Services.AddApplicationServices(builder.Configuration);
```

---

## Registration Groups

### Infrastructure

**Folder**: `Infrastructure/`

| File | Purpose |
|------|---------|
| `PersistenceRegistration.cs` | DbContext, generic repositories |
| `CachingRegistration.cs` | Redis cache service |
| `ExternalServicesRegistration.cs` | Cloudinary, email, etc. |

### Application

**Folder**: `Application/`

| File | Purpose |
|------|---------|
| `MediatRRegistration.cs` | MediatR, pipeline behaviors |
| `MappingRegistration.cs` | AutoMapper profiles |

### Security

**Folder**: `Security/`

| File | Purpose |
|------|---------|
| `IdentityRegistration.cs` | ASP.NET Core Identity |
| `JwtRegistration.cs` | JWT authentication |

### Features

**Folder**: `Features/`

| File | Purpose |
|------|---------|
| `AuthFeatureRegistration.cs` | Auth services, repositories |
| `ProductsFeatureRegistration.cs` | Product repositories |
| `CartsFeatureRegistration.cs` | Cart repositories |
| `OrdersFeatureRegistration.cs` | Order repositories |

### Cross-Cutting

**Folder**: `CrossCutting/`

| File | Purpose |
|------|---------|
| `CorsRegistration.cs` | CORS policies |
| `ApiVersioningRegistration.cs` | API versioning |

---

## Registration Order

Order matters for some registrations:

1. **Infrastructure first**: Database context needed by others
2. **Application core**: MediatR, mapping
3. **Security**: Identity depends on DbContext
4. **Features**: Depend on all above
5. **Cross-cutting**: Configuration last

---

## Service Lifetimes

| Lifetime | Usage |
|----------|-------|
| **Singleton** | Stateless services, configuration |
| **Scoped** | DbContext, repositories, per-request |
| **Transient** | Lightweight, stateless per-use |

### Common Registrations

| Service | Lifetime |
|---------|----------|
| `ApplicationDbContext` | Scoped |
| `IRepository<T>` | Scoped |
| `ICurrentUser` | Scoped |
| `ICacheService` | Singleton |
| `IImageService` | Singleton |

---

## Adding New Services

### New Repository

1. Define interface in `Application/Features/{Feature}/v1/Repositories/`
2. Implement in `Infrastructure/{Feature}/`
3. Register in `WebAPI/DependencyInjection/Features/{Feature}FeatureRegistration.cs`

### New External Service

1. Define interface in `Application.Common/Abstractions/ExternalServices/`
2. Implement in `Infrastructure/ExternalServices/`
3. Register in `WebAPI/DependencyInjection/Infrastructure/ExternalServicesRegistration.cs`

---

## Configuration Options

Services that need configuration use the Options pattern:

```csharp
services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
```

**Location**: `ZTino_Shop/src/Infrastructure/Auth/Options/JwtOptions.cs`

---

## Application Builder Extensions

**File**: `ApplicationBuilderExtensions.cs`

For middleware and app configuration:
- Data seeding: `app.SeedDataAsync()`

---

## Best Practices

1. **One file per concern**: Easy to find and modify
2. **Extension methods**: Clean fluent API
3. **Configuration injection**: Use IConfiguration for settings
4. **Group related services**: In same registration file
5. **Document dependencies**: Comment special ordering needs
