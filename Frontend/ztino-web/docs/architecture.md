# Architecture Overview

This document describes the architectural design and core concepts of the ZTino Web frontend application.

## Design Philosophy

The application follows a **feature-based architecture** that groups related functionality together, making the codebase scalable and maintainable. Each feature is self-contained with its own components, hooks, and API layer.

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                          Browser                                 │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                         React App                                │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                      App.jsx                               │  │
│  │  ┌──────────────┐    ┌────────────────────────────────┐   │  │
│  │  │ AuthProvider │ -> │         AppRouter              │   │  │
│  │  └──────────────┘    └────────────────────────────────┘   │  │
│  └───────────────────────────────────────────────────────────┘  │
│                                │                                 │
│                                ▼                                 │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                      MainLayout                            │  │
│  │  ┌─────────────┐  ┌──────────────┐  ┌─────────────────┐   │  │
│  │  │  AppHeader  │  │   Content    │  │    AppFooter    │   │  │
│  │  └─────────────┘  │   (Outlet)   │  └─────────────────┘   │  │
│  │                   └──────────────┘                         │  │
│  └───────────────────────────────────────────────────────────┘  │
│                                │                                 │
│                                ▼                                 │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │                     Feature Modules                        │  │
│  │  ┌──────┐  ┌──────┐  ┌─────────┐  ┌──────────────────┐    │  │
│  │  │ Auth │  │ Cart │  │  Home   │  │     Product      │    │  │
│  │  └──────┘  └──────┘  └─────────┘  └──────────────────┘    │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                        Axios Client                              │
│                    (Request/Response Interceptors)               │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼
┌─────────────────────────────────────────────────────────────────┐
│                       Backend API Server                         │
└─────────────────────────────────────────────────────────────────┘
```

## Core Layers

### 1. Application Entry
- **`src/main.jsx`**: Bootstraps the React application with StrictMode
- **`src/App.jsx`**: Root component that wraps the app with AuthProvider and AppRouter

### 2. Context Layer
- **AuthContext**: Manages user authentication state globally
- Located in `src/contexts/AuthContext.jsx`
- Provides: `user`, `isAuthenticated`, `login()`, `logout()`, `hasRole()`

### 3. Routing Layer
- Uses React Router v6 with `createBrowserRouter`
- Centralized route definitions in `src/routes/AppRouter.jsx`
- Route guards: `PublicRoute` (redirects authenticated users)

### 4. Layout Layer
- **MainLayout**: Provides consistent page structure with header, content area (via `Outlet`), and footer
- Uses Ant Design's Layout component

### 5. Feature Modules
Each feature follows a consistent internal structure:
```
feature/
├── api/           # API service functions
├── hooks/         # Custom React hooks
├── components/    # UI components
└── index.js       # Barrel export
```

### 6. Configuration Layer
- **`src/config/axiosClient.js`**: Centralized HTTP client with interceptors
- **`src/constants/apiEndpoints.js`**: API endpoint constants

### 7. Utility Layer
- **`src/utils/localStorage.js`**: Token and user persistence
- **`src/utils/jwtDecode.js`**: JWT parsing and validation

## Data Flow Pattern

```
┌──────────────┐    ┌───────────────┐    ┌─────────────┐
│   Component  │ -> │  Custom Hook  │ -> │   API Layer │ -> Backend
│              │ <- │  (state mgmt) │ <- │ (axiosClient│ <- 
└──────────────┘    └───────────────┘    └─────────────┘
```

1. **Components** render UI and invoke hooks for data
2. **Hooks** manage loading/error states and call API functions
3. **API Layer** uses axiosClient to make HTTP requests
4. **Axios interceptors** handle token injection and error transformation

## Key Architectural Decisions

| Decision | Rationale |
|----------|-----------|
| Feature-based structure | Enables team scaling and code isolation |
| Custom hooks for data fetching | Encapsulates loading/error states, reusable logic |
| Centralized Axios client | Consistent request/response handling |
| Barrel exports (index.js) | Cleaner imports, easier refactoring |
| Context for auth only | Lightweight state management for global auth |
| Ant Design + Tailwind | Component library for consistency + utility CSS for customization |

## Related Documentation

- [Folder Structure](./folder-structure.md) - Detailed directory breakdown
- [State Management](./state-management.md) - Data flow patterns
- [API Communication](./api-communication.md) - Backend integration
