# Directory Structure

Complete breakdown of the project folder organization and naming conventions.

## Root Structure

```
ZTino_Shop/
├── src/                          # Source code
│   ├── Domain/                   # Core entities
│   ├── Application/              # Business logic (CQRS)
│   ├── Application.Common/       # Shared abstractions
│   ├── Infrastructure/           # External implementations
│   └── WebAPI/                   # HTTP layer
├── tests/                        # Test projects
│   └── Application.Tests/        # Unit tests
├── docs/                         # Documentation
├── README.md                     # Project overview
└── ZTino_Shop.sln               # Solution file
```

## Domain Layer

```
Domain/
├── Models/                       # Entity classes
│   ├── Products/                 # Product aggregate
│   │   ├── Product.cs
│   │   ├── ProductColor.cs
│   │   ├── ProductVariant.cs
│   │   ├── ProductImage.cs
│   │   ├── Category.cs
│   │   ├── Color.cs
│   │   └── Size.cs
│   ├── Carts/                    # Cart aggregate
│   │   ├── Cart.cs
│   │   └── CartItem.cs
│   ├── Orders/                   # Order aggregate
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   ├── OrderAddress.cs
│   │   ├── OrderPayment.cs
│   │   └── OrderStatusHistory.cs
│   ├── Finances/
│   │   └── Invoice.cs
│   └── Stats/
│       ├── DailyRevenueStats.cs
│       └── ProductSalesStats.cs
├── Enums/                        # Domain enumerations
└── Consts/                       # Domain constants
```

## Application Layer

```
Application/
├── Features/                     # Organized by business feature
│   ├── Auth/
│   │   └── v1/                   # API version
│   │       ├── Commands/         # Write operations
│   │       │   ├── Login/
│   │       │   │   ├── LoginCommand.cs
│   │       │   │   ├── LoginHandler.cs
│   │       │   │   └── LoginValidator.cs
│   │       │   └── Register/
│   │       ├── Queries/          # Read operations
│   │       ├── DTOs/             # Data transfer objects
│   │       └── Services/         # Feature-specific services
│   ├── Products/
│   │   └── v1/
│   │       ├── Commands/
│   │       ├── Queries/
│   │       ├── DTOs/
│   │       ├── Mappings/         # AutoMapper profiles
│   │       ├── Repositories/     # Repository interfaces
│   │       ├── Expressions/      # Query expressions
│   │       └── Services/
│   ├── Carts/
│   │   └── v1/
│   └── Orders/
│       └── v1/
└── GlobalUsings.cs               # Global using directives
```

## Application.Common Layer

```
Application.Common/
├── Abstractions/                 # Interface definitions
│   ├── Persistence/
│   │   ├── IRepository.cs        # Generic repository
│   │   └── IApplicationDbContext.cs
│   ├── Identity/
│   │   ├── ICurrentUser.cs
│   │   └── IIdentityService.cs
│   ├── Caching/
│   │   └── ICacheService.cs
│   └── ExternalServices/
│       └── IImageService.cs
├── Behaviors/                    # MediatR pipeline behaviors
│   └── ValidationBehavior.cs
├── Exceptions/                   # Custom exception types
│   ├── ApplicationExceptionBase.cs
│   ├── NotFoundException.cs
│   ├── ConflictException.cs
│   ├── ForbiddenException.cs
│   └── BusinessRuleException.cs
└── Contracts/                    # Shared contracts
```

## Infrastructure Layer

```
Infrastructure/
├── Persistence/                  # Database layer
│   ├── ApplicationDbContext.cs   # EF Core DbContext
│   ├── ApplicationDbContextFactory.cs
│   ├── Repository.cs             # Generic repository base
│   ├── Configurations/           # EF Core Fluent API
│   │   ├── Products/
│   │   ├── Carts/
│   │   ├── Orders/
│   │   └── Auth/
│   ├── Migrations/               # EF Core migrations
│   └── Seeds/                    # Data seeding
├── Auth/                         # Authentication
│   ├── Models/                   # Identity models
│   │   ├── ApplicationUser.cs
│   │   ├── ApplicationRole.cs
│   │   └── RefreshToken.cs
│   ├── Options/                  # Configuration options
│   │   └── JwtOptions.cs
│   └── Services/                 # Auth service implementations
│       ├── Command/              # Auth commands
│       ├── Query/                # Auth queries
│       └── Jwt/                  # JWT generation
├── Caching/
│   └── RedisCacheService.cs
├── ExternalServices/             # Third-party integrations
├── Identity/                     # Identity implementations
├── Products/                     # Product repositories
├── Carts/                        # Cart repositories
└── Orders/                       # Order repositories
```

## WebAPI Layer

```
WebAPI/
├── Controllers/
│   └── v1/                       # API version
│       ├── Auth/
│       ├── Products/
│       ├── Carts/
│       ├── Orders/
│       └── Manager/              # Admin endpoints
├── DependencyInjection/          # Service registration
│   ├── ServiceCollectionExtensions.cs  # Main entry point
│   ├── Application/              # App layer registration
│   ├── Infrastructure/           # Infra layer registration
│   ├── Security/                 # Auth registration
│   ├── Features/                 # Feature registration
│   └── CrossCutting/             # CORS, versioning
├── Middleware/
│   └── ExceptionHandling/
│       └── ExceptionHandlingMiddleware.cs
├── Filters/
│   └── Response/
│       └── ApiResponseFilter.cs
├── Requests/                     # API request models
├── Responses/                    # API response models
├── Program.cs                    # Entry point
├── appsettings.json             # Configuration (gitignored)
└── appsettings.template.json    # Configuration template
```

## Tests Structure

```
tests/
└── Application.Tests/
    ├── Products/
    │   └── v1/
    │       ├── Commands/         # Command handler tests
    │       └── Queries/          # Query handler tests
    ├── Carts/
    │   └── v1/
    └── Orders/
        └── v1/
```

## Naming Conventions

| Type | Convention | Example |
|------|------------|---------|
| Entity | PascalCase, singular | `Product`, `CartItem` |
| Command | Verb + Noun + Command | `CreateProductCommand` |
| Query | Get/List + Noun + Query | `GetProductByIdQuery` |
| Handler | Command/Query name + Handler | `CreateProductHandler` |
| Validator | Command/Query name + Validator | `CreateProductValidator` |
| DTO | Entity + Dto or purpose | `ProductDto`, `ProductListItemDto` |
| Repository Interface | I + Entity + Repository | `IProductRepository` |
| Controller | Plural noun + Controller | `ProductsController` |
| Configuration | Entity + Configuration | `ProductConfiguration` |
