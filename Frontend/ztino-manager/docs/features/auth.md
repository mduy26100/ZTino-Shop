# Auth Feature

The auth feature handles user authentication for the ZTino Manager application.

## Location

`src/features/auth/`

## Structure

```
auth/
├── api/
│   ├── auth.api.js      # Login API function
│   └── index.js
├── hooks/
│   ├── auth/
│   │   └── useLogin.js  # Login hook
│   ├── account/         # (Reserved for account management)
│   └── index.js
├── components/
│   └── (Login form components)
└── index.js
```

## API Functions

### auth.api.js

| Function | Endpoint | Description |
|----------|----------|-------------|
| `loginAPI(data)` | POST /auth/login | Authenticate with identifier and password |

**Request Payload:**
```json
{
  "dto": {
    "identifier": "email or username",
    "password": "password"
  }
}
```

**Response:**
```json
{
  "accessToken": "JWT token string"
}
```

## Hooks

### useLogin

Location: `src/features/auth/hooks/auth/useLogin.js`

Manages the complete login flow:

**Returns:**
| Property | Type | Description |
|----------|------|-------------|
| `login` | Function | Execute login with credentials |
| `isLoading` | Boolean | Request in progress |
| `error` | String | Error message if failed |

**Login Function Flow:**

1. Validates input format (email/username, password)
2. Calls `loginAPI` with credentials
3. Extracts roles from JWT response
4. Validates Manager role presence
5. Builds user payload from JWT claims
6. Calls `AuthContext.login` to store session
7. Returns login response

**Validation Rules:**
- Identifier: Valid email format OR username (3-50 chars, alphanumeric + _ .)
- Password: Required, minimum 6 characters

**JWT Claims Extraction:**
- User ID from `nameidentifier` claim
- Email from `emailaddress` claim
- Name from `name` claim
- Roles from `role` or `roles` claim

## Integration with AuthContext

The `useLogin` hook integrates with `AuthContext`:

1. Imports `useAuth()` for context access
2. Calls `contextLogin(accessToken, userPayload)` on success
3. This triggers localStorage persistence and state update

## Related Documentation

- [Authentication](../authentication.md) - Full auth system documentation
- [API Communication](../api-communication.md) - API layer details
