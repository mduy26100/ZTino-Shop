# Layer Responsibilities

This document explains the purpose and contents of each layer in the Onion Architecture.

## Domain Layer

**Location**: `ZTino_Shop/src/Domain/`

**Purpose**: Contains the core business entities and rules. This is the heart of the application with zero external dependencies.

**Contents**:
| Folder | Purpose |
|--------|---------|
| `Models/` | Entity classes organized by aggregate (Products, Carts, Orders, Stats, Finances) |
| `Enums/` | Domain enumerations (e.g., OrderStatus) |
| `Consts/` | Domain constants |

**Key Entities**:
- `Product`, `ProductColor`, `ProductVariant`, `ProductImage`, `Category`, `Color`, `Size`
- `Cart`, `CartItem`
- `Order`, `OrderItem`, `OrderAddress`, `OrderPayment`, `OrderStatusHistory`

**Rules**:
- No dependencies on other layers
- No reference to EF Core or any ORM
- Contains business logic methods on entities when appropriate

---

## Application.Common Layer

**Location**: `ZTino_Shop/src/Application.Common/`

**Purpose**: Shared abstractions, behaviors, and contracts used across the Application layer. Defines interfaces that Infrastructure implements.

**Contents**:
| Folder | Purpose |
|--------|---------|
| `Abstractions/Persistence/` | `IRepository<T>`, `IApplicationDbContext` |
| `Abstractions/Identity/` | `ICurrentUser`, `IIdentityService` |
| `Abstractions/Caching/` | Cache service interfaces |
| `Abstractions/ExternalServices/` | External service interfaces |
| `Behaviors/` | MediatR pipeline behaviors (e.g., `ValidationBehavior`) |
| `Exceptions/` | Custom exception types |
| `Contracts/` | Shared contracts and models |

**Key Abstractions**:
- `IRepository<T, TKey>` - Generic repository interface
- `ICurrentUser` - Access to authenticated user context
- `ValidationBehavior` - Automatic FluentValidation in MediatR pipeline

---

## Application Layer

**Location**: `ZTino_Shop/src/Application/`

**Purpose**: Contains all business use cases implemented as Commands and Queries (CQRS). This is where business logic orchestration happens.

**Contents**:
| Folder | Purpose |
|--------|---------|
| `Features/{Feature}/v1/Commands/` | Write operations (Create, Update, Delete) |
| `Features/{Feature}/v1/Queries/` | Read operations (Get, List, Search) |
| `Features/{Feature}/v1/DTOs/` | Data transfer objects |
| `Features/{Feature}/v1/Mappings/` | AutoMapper profiles |
| `Features/{Feature}/v1/Repositories/` | Feature-specific repository interfaces |
| `Features/{Feature}/v1/Services/` | Feature-specific service interfaces |

**Features**:
- `Auth` - Authentication commands/queries
- `Products` - Product catalog management
- `Carts` - Shopping cart operations
- `Orders` - Order processing

**Handler Pattern**:
Each command/query has:
1. Request class (implements `IRequest<T>`)
2. Handler class (implements `IRequestHandler<TRequest, TResponse>`)
3. Validator class (optional, implements `IValidator<TRequest>`)

---

## Infrastructure Layer

**Location**: `ZTino_Shop/src/Infrastructure/`

**Purpose**: Implements all external concerns - database access, caching, external services, authentication.

**Contents**:
| Folder | Purpose |
|--------|---------|
| `Persistence/` | EF Core DbContext, configurations, migrations, generic repository |
| `Auth/` | Identity models, JWT services, OAuth providers |
| `Caching/` | Redis cache implementation |
| `ExternalServices/` | Cloudinary, email (Resend), AI (OpenRouter) |
| `Identity/` | Current user implementation |
| `Products/`, `Carts/`, `Orders/` | Feature-specific repository implementations |

**Key Implementations**:
- `ApplicationDbContext` - EF Core DbContext with all DbSets
- `Repository<T, TKey>` - Generic repository base class
- `RedisCacheService` - Redis caching
- JWT token generation and validation

---

## WebAPI Layer

**Location**: `ZTino_Shop/src/WebAPI/`

**Purpose**: HTTP interface layer. Handles HTTP requests, routing, and response formatting.

**Contents**:
| Folder | Purpose |
|--------|---------|
| `Controllers/v1/` | API controllers organized by feature |
| `DependencyInjection/` | Service registration extensions |
| `Middleware/` | Exception handling middleware |
| `Filters/` | API response wrapping filter |
| `Requests/` | API request models |
| `Responses/` | API response models |

**Key Components**:
- `Program.cs` - Application entry point and middleware configuration
- `ServiceCollectionExtensions` - Orchestrates all DI registrations
- `ExceptionHandlingMiddleware` - Global exception to HTTP response mapping
- `ApiResponseFilter` - Wraps all responses in standard envelope

---

## Dependency Flow Summary

```
WebAPI ─────────────────────────────────────────────┐
    │                                               │
    ▼                                               │
Application ──────────────────────────────────────┐ │
    │                                             │ │
    ▼                                             │ │
Application.Common ◄─────────────────────────────┐│ │
    │                                            ││ │
    ▼                                            ││ │
Domain                                           ││ │
                                                 ││ │
Infrastructure ──────────────────────────────────┴┴─┘
    (implements Application.Common interfaces)
```
