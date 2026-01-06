# Features Guide

This directory contains documentation for the feature modules of the ZTino Manager application.

## Feature Modules Overview

The application is organized into self-contained feature modules located in `src/features/`.

| Feature | Description |
|---------|-------------|
| [auth](./auth.md) | Authentication and login functionality |
| [product](./product.md) | Product management including categories, colors, sizes, variants, and images |

## Feature Module Structure

Each feature follows a consistent structure:

```
src/features/[feature]/
├── api/           # API service functions
│   ├── [entity].api.js
│   └── index.js
├── hooks/         # Custom hooks for data operations
│   ├── [entity]/
│   │   ├── useGet[Entity].js
│   │   ├── useCreate[Entity].js
│   │   ├── useUpdate[Entity].js
│   │   └── useDelete[Entity].js
│   └── index.js
├── components/    # Feature-specific UI components
│   ├── [entity]/
│   └── index.js
└── index.js       # Feature barrel export
```

## Design Principles

### Encapsulation

Each feature owns its:
- API definitions
- State management (hooks)
- UI components

This allows features to evolve independently.

### Reusability

Features export their public API through `index.js`:
- Hooks for data operations
- Components for UI reuse
- API functions (if needed externally)

### Consistency

All features follow the same patterns:
- Naming conventions for hooks
- Folder structure
- Export patterns

## Related Documentation

- [Architecture](../architecture.md) - Overall design philosophy
- [Folder Structure](../folder-structure.md) - Complete directory guide
