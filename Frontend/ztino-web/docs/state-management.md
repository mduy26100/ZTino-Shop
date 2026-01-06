# State Management

This document describes how application state is managed and data flows through the application.

## Overview

The application uses a **lightweight state management approach**:
- **React Context** for global authentication state
- **Custom hooks with local state** for data fetching and UI state
- **localStorage** for persistence across sessions

## Authentication State (AuthContext)

**Location**: `src/contexts/AuthContext.jsx`

AuthContext provides global authentication state accessible throughout the application.

### Provided Values

| Value | Type | Description |
|-------|------|-------------|
| `user` | `object \| null` | Current user data |
| `isAuthenticated` | `boolean` | Whether user is logged in |
| `isInitialized` | `boolean` | Whether auth state has been loaded |
| `login(token, user)` | `function` | Handle successful login |
| `logout()` | `function` | Clear auth state |
| `hasRole(roleName)` | `function` | Check if user has specific role |

### Initialization Flow

```
App Mounts
    ↓
AuthProvider useEffect
    ↓
Check localStorage for token + user
    ↓
Validate token (not expired)
    ↓
If valid: setUser(savedUser)
If invalid: handleLogout()
    ↓
setIsInitialized(true)
```

### Usage Example

```javascript
import { useAuth } from '../contexts';

function Component() {
  const { user, isAuthenticated, logout } = useAuth();
  
  if (!isAuthenticated) {
    return <LoginPrompt />;
  }
  
  return <UserDashboard user={user} onLogout={logout} />;
}
```

## Data Fetching with Custom Hooks

Instead of a centralized store, the application uses custom hooks that encapsulate data fetching logic with local state.

### Hook Pattern

Each data-fetching hook follows a consistent pattern:

```
┌─────────────────────────────────────────────────────────┐
│                    Custom Hook                          │
│  ┌─────────────────────────────────────────────────┐   │
│  │  State: data, loading, error                    │   │
│  │  Effect: fetch on mount or dependency change    │   │
│  │  Return: { data, loading, error, refetch }      │   │
│  └─────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

### Available Hooks by Feature

#### Auth Hooks (`src/features/auth/hooks/`)

| Hook | Purpose | Returns |
|------|---------|---------|
| `useLogin` | Handle login form submission | `{ login, loading, error }` |
| `useRegister` | Handle registration form submission | `{ register, loading, error }` |

#### Cart Hooks (`src/features/cart/hooks/`)

| Hook | Purpose | Returns |
|------|---------|---------|
| `useGetMyCart` | Fetch authenticated user's cart | `{ cart, loading, error, refetch }` |
| `useGetCartById` | Fetch cart by ID (for guests) | `{ cart, loading, error }` |
| `useCreateCart` | Add item to cart | `{ addToCart, loading, error }` |

#### Product Hooks (`src/features/product/hooks/`)

| Hook | Purpose | Returns |
|------|---------|---------|
| `useGetCategories` | Fetch all categories | `{ categories, loading, error }` |
| `useGetProductsByCategoryId` | Fetch products by category | `{ products, loading, error }` |
| `useGetProductDetailBySlug` | Fetch single product | `{ product, loading, error }` |

## Persistence Layer

**Location**: `src/utils/localStorage.js`

### Persisted Data

| Key | Purpose | Functions |
|-----|---------|-----------|
| `accessToken` | JWT authentication token | `setToken`, `getToken`, `removeToken` |
| `user` | User profile data (JSON) | `setUser`, `getUser`, `removeUser` |
| `guestCartId` | Cart ID for guest users | `setGuestCartId`, `getGuestCartId`, `removeGuestCartId` |

### Guest Cart Pattern

For unauthenticated users:
1. First add-to-cart creates a new cart on the backend
2. Backend returns `cartId` which is stored in localStorage
3. Subsequent cart operations include this `cartId`
4. On login, guest cart can be merged with user cart

## State Flow Diagrams

### Login Flow

```
LoginPage
    ↓
useLogin hook
    ↓
loginAPI(credentials)
    ↓
On success:
    ├── setToken(accessToken)
    ├── setUser(userPayload)
    └── authContext.login(token, user)
          ↓
        setUser(user) in context
          ↓
        isAuthenticated = true
          ↓
        Components re-render
```

### Data Fetching Flow

```
Page/Component mounts
    ↓
useGetProducts() hook
    ↓
useEffect triggers
    ↓
setLoading(true)
    ↓
API call via axiosClient
    ↓
Response received
    ↓
setData(response.data)
setLoading(false)
    ↓
Component re-renders with data
```

## Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| Context for auth only | Auth is the only truly global state; other data is route-scoped |
| No Redux/Zustand | App complexity doesn't warrant a full state library |
| Hooks for data fetching | Encapsulates async logic, reusable, testable |
| localStorage for persistence | Simple, no backend session management required |

## Related Documentation

- [Architecture](./architecture.md) - Overall system design
- [API Communication](./api-communication.md) - How hooks call the backend
