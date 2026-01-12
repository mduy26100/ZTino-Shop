# Architecture Overview

This document describes the high-level architecture and design philosophy of the ZTino Manager application.

## Architectural Pattern

The application follows a **Feature-based Architecture** combined with a **Custom Hooks Pattern** for data management. This approach provides:

- Clear separation of concerns
- Scalable module organization
- Reusable business logic
- Easy onboarding for new developers

## Application Layers

```
┌─────────────────────────────────────────────────────────┐
│                      Pages Layer                        │
│   Composes features + layouts into complete views       │
├─────────────────────────────────────────────────────────┤
│                    Features Layer                       │
│   Self-contained modules with API, hooks, components    │
├─────────────────────────────────────────────────────────┤
│                    Shared Layer                         │
│   Layouts, contexts, config, utils, constants           │
├─────────────────────────────────────────────────────────┤
│                   Infrastructure                        │
│   Axios client, routing, authentication                 │
└─────────────────────────────────────────────────────────┘
```

## Core Design Decisions

### 1. Feature Modules

Each feature (e.g., `auth`, `product`) is a self-contained module with:

| Folder | Responsibility |
|--------|----------------|
| `api/` | API service functions |
| `hooks/` | Data fetching and mutation hooks |
| `components/` | Feature-specific UI components |
| `index.js` | Barrel exports for clean imports |

This encapsulation allows features to evolve independently while maintaining clear boundaries.

### 2. Custom Hooks Pattern

All data operations use a **two-layer hook architecture**:

```
┌───────────────────────────────────────────────────────────┐
│                   Base Hooks Layer                         │
│                 (src/hooks/utils/)                         │
│  ┌─────────────────────┐  ┌───────────────────────────┐   │
│  │     useQuery        │  │      useMutation          │   │
│  │  - Global caching   │  │  - Loading state          │   │
│  │  - TTL support      │  │  - Error handling         │   │
│  │  - Abort control    │  │  - Lifecycle callbacks    │   │
│  │  - Refetch          │  │  - Variables tracking     │   │
│  └─────────────────────┘  └───────────────────────────┘   │
└───────────────────────────────────────────────────────────┘
                            │
                            ▼
┌───────────────────────────────────────────────────────────┐
│                 Feature Hooks Layer                        │
│              (src/features/*/hooks/)                       │
│  useGetProducts, useCreateCategory, useUpdateOrder, ...   │
└───────────────────────────────────────────────────────────┘
```

**Base Hooks** abstract common patterns:
- `useQuery(key, queryFn, options)` - Data fetching with caching
- `useMutation(mutationFn, options)` - Mutation operations

**Feature Hooks** naming convention:
- `useGet*` - Wraps `useQuery` for GET requests
- `useCreate*` - Wraps `useMutation` for POST requests
- `useUpdate*` - Wraps `useMutation` for PUT/PATCH requests
- `useDelete*` - Wraps `useMutation` for DELETE requests

Each hook manages its own loading, error, and data states, providing a clean interface for components.

### 3. Centralized API Client

A single Axios instance (`src/config/axiosClient.js`) handles:

- Base URL configuration via environment variables
- Automatic JWT token injection on requests
- Consistent error response transformation
- Automatic logout on 401 responses
- Timeout handling

### 4. Context-based Authentication

Authentication state is managed via React Context (`src/contexts/AuthContext.jsx`):

- Provides `user`, `isAuthenticated`, `login`, `logout`
- Validates token expiration on app initialization
- Enforces role-based access (Manager role required)

### 5. Layout Composition

The UI uses Ant Design's Layout system with:

- `MainLayout` - Shell with sidebar, header, content area
- `AppSider` - Navigation menu (collapsible, responsive)
- `AppHeader` - Top bar with user actions

## Data Flow

```
User Action → Page Component → Feature Hook → API Function → Backend
                    ↓
             UI State Update ← Response Processing
```

1. User interacts with a Page component
2. Page invokes a feature hook (e.g., `useCreateProduct`)
3. Hook calls the corresponding API function
4. API function uses axiosClient to make the request
5. Response is processed through interceptors
6. Hook updates its internal state
7. Component re-renders with new data

## State Management Strategy

| State Type | Solution |
|------------|----------|
| Auth State | React Context (AuthContext) |
| Server State | Custom hooks with useState |
| UI State | Component-local useState |
| Form State | Ant Design Form |

The application intentionally avoids external state management libraries (Redux, Zustand) to keep complexity low, as the data requirements are primarily CRUD operations with straightforward patterns.

## Error Handling Philosophy

Errors are handled at multiple levels:

1. **API Layer**: Axios interceptors normalize error responses
2. **Hook Layer**: Hooks capture and expose error state
3. **Component Layer**: Components display errors via Ant Design messages

This layered approach ensures consistent error handling without duplicating logic.

## Related Documentation

- [Folder Structure](./folder-structure.md) - Detailed directory explanation
- [API Communication](./api-communication.md) - HTTP layer details
- [Authentication](./authentication.md) - Auth flow documentation
