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

## Data Fetching with Base Hooks

The application uses **base hooks** that abstract common data fetching patterns.

**Location**: `src/hooks/utils/`

### Base Hook Architecture

```
┌───────────────────────────────────────────────────────────────┐
│                     Base Hooks Layer                           │
│  ┌────────────────────────┐  ┌──────────────────────────────┐ │
│  │        useQuery        │  │        useMutation           │ │
│  │  - Global caching      │  │  - Loading state             │ │
│  │  - TTL expiration      │  │  - Error handling            │ │
│  │  - Abort controller    │  │  - Lifecycle callbacks       │ │
│  │  - Auto-refetch        │  │  - Variable tracking         │ │
│  └────────────────────────┘  └──────────────────────────────┘ │
│                                                                │
│  ┌────────────────────────────────────────────────────────┐   │
│  │           invalidateCartCacheByAuth                     │   │
│  │  Cart-specific helper for guest/authenticated flows    │   │
│  └────────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌───────────────────────────────────────────────────────────────┐
│                    Feature Hooks Layer                         │
│  useGetMyCart, useCreateOrder, useGetProducts, useLogin ...   │
└───────────────────────────────────────────────────────────────┘
```

### useQuery Options

| Option | Default | Description |
|--------|---------|-------------|
| `ttl` | 300000 (5 min) | Cache time-to-live in milliseconds |
| `enabled` | `true` | Whether to auto-fetch on mount |
| `initialData` | `null` | Initial data before first fetch |
| `transformResponse` | - | Transform API response |
| `onSuccess` | - | Callback on successful fetch |
| `onError` | - | Callback on error |

### useMutation Options

| Option | Description |
|--------|-------------|
| `onMutate` | Called before mutation starts |
| `onSuccess` | Called on success with `(result, variables)` |
| `onError` | Called on error with `(error, variables)` |
| `onSettled` | Called after mutation completes (success or error) |

### Cache Invalidation

```javascript
import { invalidateCache, clearGlobalCache } from '../../../hooks/utils';

// Invalidate specific cache
invalidateCache('my-cart');
invalidateCache('products-123');

// Clear all cache
clearGlobalCache();
```

### Cart Cache Invalidation Helper

For cart-related mutations, use the dedicated helper:

```javascript
import { invalidateCartCacheByAuth } from '../../../hooks/utils';

// Automatically handles guest vs authenticated user cache
invalidateCartCacheByAuth(isAuthenticated, cartId);
```

This helper:
- Invalidates `my-cart` cache for authenticated users
- Invalidates `guest-cart-{id}` cache for guest users

### Available Hooks by Feature

#### Auth Hooks (`src/features/auth/hooks/`)

| Hook | Purpose | Returns |
|------|---------|---------|
| `useLogin` | Handle login form submission | `{ login, loading, error }` |
| `useRegister` | Handle registration form submission | `{ register, loading, error }` |

#### Cart Hooks (`src/features/cart/hooks/`)

| Hook | Purpose | Returns |
|------|---------|---------|
| `useGetMyCart` | Fetch authenticated user's cart | `{ data, isLoading, error, refetch }` |
| `useGetCartById` | Fetch cart by ID (for guests) | `{ data, isLoading, error }` |
| `useCreateCart` | Add item to cart | `{ create, isLoading }` |
| `useUpdateCart` | Update cart item quantity | `{ update, isLoading, updatingItemId }` |
| `useDeleteCart` | Remove item from cart | `{ remove, isLoading, deletingItemId }` |

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
