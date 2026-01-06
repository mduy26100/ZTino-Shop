# Authentication Feature

This document describes the authentication system and its components.

## Overview

The auth feature handles user login and registration, JWT token management, and role-based access control.

## Structure

```
src/features/auth/
├── api/
│   ├── auth.api.js      # Login, register API calls
│   └── index.js
├── hooks/
│   ├── auth/
│   │   ├── useLogin.js
│   │   └── useRegister.js
│   └── index.js
├── components/
│   ├── auth/            # Form components
│   └── profile/         # User profile components
└── index.js
```

## API Functions

**File**: `src/features/auth/api/auth.api.js`

| Function | Method | Endpoint | Payload |
|----------|--------|----------|---------|
| `loginAPI` | POST | `/auth/login` | `{ identifier, password }` |
| `registerAPI` | POST | `/auth/register` | `{ firstName, lastName, userName, phoneNumber, password, confirmPassword }` |

## Hooks

### useLogin

Handles the login process including API call, token storage, and context update.

**Returns**:
- `login(credentials)` - Async function to perform login
- `loading` - Boolean loading state
- `error` - Error object if failed

**Flow**:
1. Call `loginAPI` with credentials
2. Store `accessToken` in localStorage
3. Decode token for roles
4. Build user payload
5. Call `authContext.login()` to update global state
6. Navigate to home page

### useRegister

Handles user registration.

**Returns**:
- `register(formData)` - Async function to perform registration
- `loading` - Boolean loading state
- `error` - Error object if failed

## Pages

### LoginPage (`src/pages/Auth/LoginPage.jsx`)

- Form with identifier (email/username) and password fields
- Uses `useLogin` hook for submission
- Redirects to registration page link
- Protected by `PublicRoute` (redirects if already authenticated)

### RegisterPage (`src/pages/Auth/RegisterPage.jsx`)

- Form with name, username, phone, password fields
- Uses `useRegister` hook for submission
- Redirects to login page link
- Protected by `PublicRoute`

## Token Management

JWT tokens are stored in localStorage and include:
- Standard claims (`sub`, `exp`, `iat`)
- Role claims (checked at multiple possible keys for compatibility)

**Utilities**: `src/utils/jwtDecode.js`
- `decodeToken(token)` - Decode JWT payload
- `getRolesFromToken(token)` - Extract roles array
- `isTokenExpired(token)` - Check if token is expired

## Security Considerations

- Tokens are stored in localStorage (standard for SPAs)
- 401 responses trigger automatic logout via Axios interceptor
- Token expiration is checked on app initialization
- Passwords are never stored client-side

## Related Documentation

- [State Management](../state-management.md) - AuthContext details
- [API Communication](../api-communication.md) - Request patterns
