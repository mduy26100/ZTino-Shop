# Authentication

This document describes the authentication system of the ZTino Manager application.

## Overview

The application uses **JWT (JSON Web Token)** authentication with role-based access control. Only users with the "Manager" role can access the application.

## Authentication Flow

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│  Login Page │ ──> │  useLogin   │ ──> │  loginAPI   │
└─────────────┘     └─────────────┘     └─────────────┘
                           │                    │
                           │                    ▼
                           │            ┌─────────────┐
                           │            │   Backend   │
                           │            └─────────────┘
                           │                    │
                           ▼                    ▼
                    ┌─────────────┐     Access Token
                    │ AuthContext │ <───────────────
                    │   .login()  │
                    └─────────────┘
                           │
                           ▼
                    ┌─────────────┐
                    │ localStorage│
                    │ + navigate  │
                    └─────────────┘
```

## Key Components

### AuthContext

Location: `src/contexts/AuthContext.jsx`

Provides authentication state and methods to the entire application:

| Export | Type | Description |
|--------|------|-------------|
| `user` | Object | Current user info (id, email, name, roles) |
| `isAuthenticated` | Boolean | Whether user is logged in |
| `isInitialized` | Boolean | Whether auth check is complete |
| `login(token, user)` | Function | Store credentials and update state |
| `logout()` | Function | Clear credentials and reset state |
| `hasRole(roleName)` | Function | Check if user has specific role |

### useLogin Hook

Location: `src/features/auth/hooks/auth/useLogin.js`

Handles the login process:

1. Validates input (email/username format, password)
2. Calls login API with credentials
3. Extracts roles from JWT
4. Validates Manager role
5. Stores token and user in localStorage
6. Updates AuthContext

### Utility Functions

Location: `src/utils/jwtDecode.js`

| Function | Purpose |
|----------|---------|
| `decodeToken(token)` | Decode JWT payload |
| `getRolesFromToken(token)` | Extract role list from token |
| `hasRole(token, roleName)` | Check for specific role |
| `isTokenExpired(token)` | Check token expiry |

Location: `src/utils/localStorage.js`

| Function | Purpose |
|----------|---------|
| `setToken(token)` | Store access token |
| `getToken()` | Retrieve access token |
| `setUser(user)` | Store user object |
| `getUser()` | Retrieve user object |
| `clearAuth()` | Remove auth data |

## Session Initialization

On application startup (`AuthContext` mount):

1. Retrieve token from localStorage
2. Check if token exists and is not expired
3. Validate user has Manager role
4. If valid: restore user state
5. If invalid: clear auth and remain logged out

## Token Handling

### Storage

| Key | Value |
|-----|-------|
| `accessToken` | JWT access token |
| `user` | JSON-serialized user object |

### Auto-injection

The Axios client (`src/config/axiosClient.js`) automatically adds the Authorization header:

```
Authorization: Bearer <accessToken>
```

### Auto-logout

On receiving a 401 Unauthorized response (except on login page):

1. Clear authentication data
2. Redirect to login page

## Role Requirements

The application requires the **Manager** role:

- Validated during login (rejects non-managers)
- Validated by PrivateRoute (redirects non-managers)
- Stored in user object for UI decisions

## Related Documentation

- [Routing](./routing.md) - Route guards implementation
- [API Communication](./api-communication.md) - Token injection details
