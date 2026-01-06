# ZTino_Shop Backend

A .NET 8 Web API backend for an e-commerce platform, built with **Onion Architecture** and **CQRS pattern**.

## Overview

ZTino_Shop Backend is the core server-side application powering the ZTino e-commerce platform.  
It provides RESTful APIs to manage users, authentication, products, carts, orders, and related business workflows.

The system is designed with a clean, modular architecture to ensure scalability, maintainability, and long-term extensibility.  
By applying Onion Architecture and the CQRS pattern, business logic is clearly separated from infrastructure concerns, making the codebase easier to evolve and test.

This backend serves multiple clients, including the customer-facing web application and potential future mobile or admin applications.

## Tech Stack

- **.NET 8** - Web API framework
- **Entity Framework Core** - ORM with SQL Server
- **MediatR** - CQRS implementation
- **FluentValidation** - Request validation
- **JWT + OAuth** - Authentication (Facebook, Google)
- **Redis** - Distributed caching
- **Cloudinary** - Image storage
- **xUnit** - Unit testing

## Quick Start

### Prerequisites
- .NET 8 SDK
- SQL Server
- Redis (optional, for caching)

### Configuration
1. Copy `appsettings.template.json` to `appsettings.json`
2. Configure your connection strings and secrets:
   - `ConnectionStrings:DefaultConnection` - SQL Server connection
   - `Jwt:Secret`, `ValidIssuer`, `ValidAudience` - JWT settings
   - `Redis:ConnectionString` - Redis connection (optional)
   - OAuth credentials for Facebook/Google (optional)

### Running Locally
```bash
cd Backend/ZTino_Shop/src/WebAPI
dotnet restore
dotnet run
```

The API will be available at: `https://localhost:<ZTino_SHOP-PORT>/api/v1/`

Swagger UI: `https://localhost:<ZTino_SHOP-PORT>/swagger`

## Project Structure

```
ZTino_Shop/
├── src/
│   ├── Domain/              # Core entities and business models
│   ├── Application/         # CQRS handlers, DTOs, validators
│   ├── Application.Common/  # Shared abstractions and behaviors
│   ├── Infrastructure/      # EF Core, caching, external services
│   └── WebAPI/              # Controllers, middleware, DI config
└── tests/
    └── Application.Tests/   # Unit tests for handlers
```

## Features

| Module | Description |
|--------|-------------|
| **Auth** | JWT authentication, refresh tokens, OAuth (Facebook/Google) |
| **Products** | Product catalog, categories, colors, sizes, variants, images |
| **Carts** | Shopping cart management |
| **Orders** | Order processing, payment, status tracking |

## Documentation

Detailed documentation is available in the [docs/](ZTino_Shop/docs/) folder:

### Architecture
- [Architecture Overview](ZTino_Shop/docs/architecture/architecture-overview.md) - System design and data flow
- [Layer Responsibilities](ZTino_Shop/docs/architecture/layer-responsibilities.md) - What each layer does
- [Request Flow](ZTino_Shop/docs/architecture/request-flow.md) - Request lifecycle
- [Directory Structure](ZTino_Shop/docs/architecture/directory-structure.md) - Project organization

### Domain
- [Domain Modeling](ZTino_Shop/docs/domain/domain-modeling.md) - Entities, aggregates, business logic

### API
- [API Standards](ZTino_Shop/docs/api/api-standards.md) - REST conventions, response format, versioning

### Features
- [Feature Organization](ZTino_Shop/docs/features/feature-organization.md) - Module structure
- [CQRS Pattern](ZTino_Shop/docs/features/cqrs-pattern.md) - Command/Query separation

### Security
- [Authentication](ZTino_Shop/docs/security/authentication.md) - JWT, OAuth providers
- [Authorization](ZTino_Shop/docs/security/authorization.md) - Roles and policies

### Data Access
- [Database Access](ZTino_Shop/docs/data-access/database-access.md) - EF Core setup
- [Repository Pattern](ZTino_Shop/docs/data-access/repository-pattern.md) - Data access layer
- [Entity Configurations](ZTino_Shop/docs/data-access/entity-configurations.md) - Fluent API

### Cross-Cutting Concerns
- [Dependency Injection](ZTino_Shop/docs/cross-cutting/dependency-injection.md) - Service registration
- [Exception Handling](ZTino_Shop/docs/cross-cutting/exception-handling.md) - Error handling
- [Validation](ZTino_Shop/docs/cross-cutting/validation.md) - Request validation
- [Caching](ZTino_Shop/docs/cross-cutting/caching.md) - Redis caching

### Testing & Setup
- [Testing Strategy](ZTino_Shop/docs/testing/testing-strategy.md) - Test organization
- [Local Setup](ZTino_Shop/docs/setup/local-setup.md) - Detailed setup guide

## API Versioning

All endpoints are versioned under `/api/v1/`. Example:
- `GET /api/v1/products`
- `POST /api/v1/auth/login`
- `GET /api/v1/carts/{id}`

## License

Private - ZTino Shop
