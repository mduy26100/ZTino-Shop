# Features Documentation

This directory contains detailed documentation for each feature module in the application.

## Available Features

| Feature | Description | Documentation |
|---------|-------------|---------------|
| **Auth** | User authentication (login, register) | [auth.md](./auth.md) |
| **Cart** | Shopping cart management | [cart.md](./cart.md) |
| **Product** | Product catalog and details | [product.md](./product.md) |
| **Home** | Homepage components | [home.md](./home.md) |
| **Order** | Order management and checkout | [order.md](./order.md) |

## Feature Module Pattern

Each feature follows the same internal structure:

```
feature/
├── api/           # API service functions
├── hooks/         # Custom React hooks
├── components/    # UI components
└── index.js       # Barrel export
```

This pattern ensures:
- **Encapsulation**: Feature logic is self-contained
- **Scalability**: Easy to add new features
- **Maintainability**: Clear boundaries between features
- **Testability**: Isolated units for testing

## Adding New Features

1. Create folder in `src/features/[feature-name]/`
2. Add `api/`, `hooks/`, `components/` subdirectories
3. Create `index.js` barrel files at each level
4. Update `src/features/index.js` to export new feature
5. Add documentation in `docs/features/[feature-name].md`
