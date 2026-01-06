# Feature Organization

This document explains how business features are organized in the Application layer.

## Feature Structure

Features are organized by business domain, not technical concern:

```
Application/Features/
├── Auth/           # Authentication & authorization
├── Products/       # Product catalog management
├── Carts/          # Shopping cart
└── Orders/         # Order processing
```

Each feature is self-contained with its own:
- Commands (write operations)
- Queries (read operations)
- DTOs (data transfer objects)
- Validators (request validation)
- Repositories (data access interfaces)
- Mappings (AutoMapper profiles)

## Feature Internal Structure

```
Features/{FeatureName}/
└── v1/                           # API version
    ├── Commands/                 # Write operations
    │   ├── CreateProduct/
    │   │   ├── CreateProductCommand.cs
    │   │   ├── CreateProductHandler.cs
    │   │   └── CreateProductValidator.cs
    │   └── UpdateProduct/
    │       └── ...
    ├── Queries/                  # Read operations
    │   ├── GetProductById/
    │   │   ├── GetProductByIdQuery.cs
    │   │   └── GetProductByIdHandler.cs
    │   └── GetProducts/
    │       └── ...
    ├── DTOs/                     # Data transfer objects
    │   ├── ProductDto.cs
    │   └── ProductListItemDto.cs
    ├── Repositories/             # Repository interfaces
    │   └── IProductRepository.cs
    ├── Mappings/                 # AutoMapper profiles
    │   └── ProductMappingProfile.cs
    └── Services/                 # Feature-specific services
        └── IProductService.cs
```

## Feature Overview

### Auth Feature
**Location**: `ZTino_Shop/src/Application/Features/Auth/v1/`

| Folder | Contents |
|--------|----------|
| `Commands/` | Login, Register, Logout, RefreshToken, ChangePassword, OAuth flows |
| `Queries/` | GetCurrentUser |
| `DTOs/` | AuthResponse, TokenDto, UserDto |
| `Services/` | JWT services, OAuth services |

### Products Feature
**Location**: `ZTino_Shop/src/Application/Features/Products/v1/`

| Folder | Contents |
|--------|----------|
| `Commands/` | CRUD for Product, Category, Color, Size, ProductColor, ProductVariant, ProductImage |
| `Queries/` | GetProductById, GetProducts (list), GetCategories, etc. |
| `DTOs/` | ProductDto, CategoryDto, ProductListItemDto, etc. |
| `Repositories/` | IProductRepository, ICategoryRepository, etc. |
| `Mappings/` | AutoMapper profiles for all entities |
| `Expressions/` | Reusable query expressions |

### Carts Feature
**Location**: `ZTino_Shop/src/Application/Features/Carts/v1/`

| Folder | Contents |
|--------|----------|
| `Commands/` | CreateCart (add item), UpdateCartItem, DeleteCartItem |
| `Queries/` | GetCartById, GetMyCart |
| `DTOs/` | CartDto, CartItemDto |
| `Repositories/` | ICartRepository |

### Orders Feature
**Location**: `ZTino_Shop/src/Application/Features/Orders/v1/`

| Folder | Contents |
|--------|----------|
| `Commands/` | CreateOrder, UpdateOrderStatus |
| `Queries/` | GetOrderById, GetOrders, GetMyOrders |
| `DTOs/` | OrderDto, OrderItemDto, OrderAddressDto |
| `Repositories/` | IOrderRepository |

---

## Versioning Strategy

Features are versioned with folder structure:
```
Features/Products/
├── v1/     # Current version
└── v2/     # Future version (when needed)
```

**When to create v2**:
- Breaking changes to request/response format
- Major business logic changes
- Deprecated endpoints

**During transition**:
- v1 and v2 coexist
- Controllers route to appropriate version
- Gradual client migration

---

## Cross-Feature Communication

Features should minimize direct dependencies. When one feature needs another:

1. **Preferred**: Use shared DTOs or domain entities
2. **Acceptable**: Inject another feature's repository
3. **Avoid**: Calling another feature's handler directly

Example: Order creation needs product info
```
OrderHandler → IProductRepository → Get product prices
```

---

## Adding a New Feature

1. Create folder structure:
   ```
   Features/NewFeature/
   └── v1/
       ├── Commands/
       ├── Queries/
       ├── DTOs/
       └── Repositories/
   ```

2. Create repository interface in `Repositories/`

3. Implement repository in `Infrastructure/NewFeature/`

4. Register services in `WebAPI/DependencyInjection/Features/`

5. Create controller in `WebAPI/Controllers/v1/NewFeature/`

6. Add tests in `tests/Application.Tests/NewFeature/`
