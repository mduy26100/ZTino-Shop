# Architecture Overview

ZTino_Shop backend implements **Onion Architecture** (also known as Clean Architecture), ensuring separation of concerns and testability.

## Layer Diagram

```mermaid
graph TB
    subgraph "Outer Layers"
        API["WebAPI<br/>(Controllers, Middleware)"]
        INFRA["Infrastructure<br/>(EF Core, Redis, Cloudinary)"]
    end
    
    subgraph "Core Layers"
        APP["Application<br/>(Handlers, DTOs, Validators)"]
        COMMON["Application.Common<br/>(Abstractions, Behaviors)"]
        DOMAIN["Domain<br/>(Entities, Enums)"]
    end
    
    API --> APP
    INFRA --> APP
    APP --> COMMON
    APP --> DOMAIN
    COMMON --> DOMAIN
    INFRA -.->|implements| COMMON
```

**Dependency Rule**: Dependencies always point inward. Domain has no dependencies; outer layers depend on inner layers.

## Request Flow

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant MediatR
    participant ValidationBehavior
    participant Handler
    participant Repository
    participant Database

    Client->>Controller: HTTP Request
    Controller->>MediatR: Send(Command/Query)
    MediatR->>ValidationBehavior: Pipeline Behavior
    ValidationBehavior->>ValidationBehavior: FluentValidation
    alt Validation Fails
        ValidationBehavior-->>Controller: ValidationException
        Controller-->>Client: 400 Bad Request
    else Validation Passes
        ValidationBehavior->>Handler: Handle()
        Handler->>Repository: Data Access
        Repository->>Database: SQL Query
        Database-->>Repository: Data
        Repository-->>Handler: Entities/DTOs
        Handler-->>MediatR: Response
        MediatR-->>Controller: Result
        Controller-->>Client: HTTP Response
    end
```

## Layer Communication

| From | To | How |
|------|-----|-----|
| WebAPI | Application | MediatR commands/queries |
| Application | Domain | Direct entity usage |
| Application | Infrastructure | Via abstractions in Application.Common |
| Infrastructure | Application.Common | Implements interfaces |

## Key Design Decisions

1. **CQRS without Event Sourcing**: Commands and queries are separated for clarity, but share the same database for simplicity.

2. **Feature-based organization**: Code is organized by business feature (Auth, Products, Carts, Orders), not by technical concern.

3. **Pipeline Behaviors**: Cross-cutting concerns (validation, logging) are handled via MediatR pipeline behaviors.

4. **Repository per Aggregate**: Each feature has its own repository interface, implemented in Infrastructure.

## Related Documentation

- [Layer Responsibilities](layer-responsibilities.md) - Detailed breakdown of each layer
- [Request Flow](request-flow.md) - Step-by-step request tracing
- [Directory Structure](directory-structure.md) - Project folder organization
