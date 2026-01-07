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

## 🐳 Docker

### Quick Start with Docker

The recommended way to run the backend is via Docker Compose from the project root:

```bash
# Production
docker compose up backend --build

# Development
docker compose -f docker-compose.dev.yml up backend --build
```

### Standalone Docker Build

```bash
# Build the image
docker build -t ztino-backend .

# Run the container
docker run -d \
  -p 8080:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal;Database=ZTinoShop;User=sa;Password=YourPassword;TrustServerCertificate=True" \
  -e "Redis__ConnectionString=host.docker.internal:6379" \
  --name ztino-backend \
  ztino-backend
```

### Multi-Stage Dockerfile

The Dockerfile uses a multi-stage build for optimal image size:

| Stage | Base Image | Purpose |
|-------|------------|---------|
| `build` | `mcr.microsoft.com/dotnet/sdk:8.0` | Compile code, create migration bundle |
| `runtime` | `mcr.microsoft.com/dotnet/aspnet:8.0-alpine` | Run the application (~100MB) |

### Database Migrations

Migrations run automatically when the container starts via `entrypoint.sh`:

```bash
# The entrypoint executes:
./efbundle --connection "$ConnectionStrings__DefaultConnection"
```

The migration bundle is a self-contained executable created during the build stage, eliminating the need for .NET SDK in the runtime image.

### Environment Variables in Docker

| Variable | Description | Example |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Development`, `Production` |
| `ConnectionStrings__DefaultConnection` | SQL Server connection | `Server=sqlserver;Database=...` |
| `Redis__ConnectionString` | Redis connection | `redis:6379` |
| `Jwt__Secret` | JWT signing key | 32+ character string |
| `Jwt__ValidIssuer` | JWT issuer | `ZTinoShop` |
| `Jwt__ValidAudience` | JWT audience | `ZTinoShopUsers` |

### Health Check

The container exposes port 8080 and the API provides health endpoints:

```bash
# Check if API is running
curl http://localhost:8080/health
```

### Docker Networking Note

> [!IMPORTANT]
> Inside Docker, use container names as hostnames:
> - ❌ `Server=localhost` 
> - ✅ `Server=sqlserver`

For more details, see the [Docker Guide](../../docs/docker.md).

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

Detailed documentation is available in the [docs/](./docs/) folder:

### Architecture

- [Architecture Overview](./docs/architecture/architecture-overview.md) - System design and data flow
- [Layer Responsibilities](./docs/architecture/layer-responsibilities.md) - What each layer does
- [Request Flow](./docs/architecture/request-flow.md) - Request lifecycle
- [Directory Structure](./docs/architecture/directory-structure.md) - Project organization

### Domain

- [Domain Modeling](./docs/domain/domain-modeling.md) - Entities, aggregates, business logic

### API

- [API Standards](./docs/api/api-standards.md) - REST conventions, response format, versioning

### Features

- [Feature Organization](./docs/features/feature-organization.md) - Module structure
- [CQRS Pattern](./docs/features/cqrs-pattern.md) - Command/Query separation

### Security

- [Authentication](./docs/security/authentication.md) - JWT, OAuth providers
- [Authorization](./docs/security/authorization.md) - Roles and policies

### Data Access

- [Database Access](./docs/data-access/database-access.md) - EF Core setup
- [Repository Pattern](./docs/data-access/repository-pattern.md) - Data access layer
- [Entity Configurations](./docs/data-access/entity-configurations.md) - Fluent API

### Cross-Cutting Concerns

- [Dependency Injection](./docs/cross-cutting/dependency-injection.md) - Service registration
- [Exception Handling](./docs/cross-cutting/exception-handling.md) - Error handling
- [Validation](./docs/cross-cutting/validation.md) - Request validation
- [Caching](./docs/cross-cutting/caching.md) - Redis caching

### Testing & Setup

- [Testing Strategy](./docs/testing/testing-strategy.md) - Test organization
- [Local Setup](./docs/setup/local-setup.md) - Detailed setup guide

## API Versioning

All endpoints are versioned under `/api/v1/`. Example:
- `GET /api/v1/products`
- `POST /api/v1/auth/login`
- `GET /api/v1/carts/{id}`

## License

Private - ZTino Shop
