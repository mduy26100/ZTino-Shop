# API Communication

This document describes how the frontend communicates with the backend API.

## Overview

The application uses **Axios** as the HTTP client with a centralized configuration that handles authentication, error transformation, and consistent response formatting.

## Axios Client Configuration

**Location**: `src/config/axiosClient.js`

### Base Configuration

| Setting | Value | Purpose |
|---------|-------|---------|
| `baseURL` | `VITE_API_URL` env variable | API server URL |
| `timeout` | 30000ms | Request timeout |
| `Content-Type` | `application/json` | Request body format |
| `Accept` | `application/json` | Expected response format |

### Request Interceptor

Every outgoing request is intercepted to:
1. Check localStorage for `accessToken`
2. If token exists, add `Authorization: Bearer <token>` header

### Response Interceptor

#### Success Handling
- **204 No Content**: Returns `null`
- **Normal response**: Extracts `response.data.data` (unwraps API envelope)
- **API-level error** (error in response body): Rejects with error object

#### Error Handling

| Error Type | Status | Behavior |
|------------|--------|----------|
| Timeout | - | Returns standardized timeout error |
| 401 Unauthorized | 401 | Clears auth, redirects to `/login` |
| Other HTTP errors | 4xx/5xx | Returns API error response |
| Network error | - | Returns standardized network error |

### Error Response Format

All errors follow a consistent structure:

```json
{
  "error": {
    "type": "error-type",
    "message": "Human-readable message",
    "details": null
  },
  "meta": {
    "timestamp": "ISO-8601 date"
  }
}
```

## API Endpoints

**Location**: `src/constants/apiEndpoints.js`

| Constant | Value | Description |
|----------|-------|-------------|
| `AUTH_LOGIN` | `/auth/login` | User authentication |
| `AUTH_REGISTER` | `/auth/register` | User registration |
| `CATEGORIES` | `/categories` | Product categories |
| `PRODUCTS` | `/products` | Product operations |
| `CART` | `/carts` | Shopping cart operations |

## API Layer Structure

Each feature has its own API file that exports functions for backend operations.

### Auth API (`src/features/auth/api/auth.api.js`)

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `loginAPI(payload)` | POST | `/auth/login` | Authenticate user |
| `registerAPI(payload)` | POST | `/auth/register` | Create new account |

### Cart API (`src/features/cart/api/cart.api.js`)

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `getMyCart()` | GET | `/carts` | Get authenticated user's cart |
| `getCartById(id)` | GET | `/carts/:id` | Get cart by ID (guests) |
| `createCart(cart)` | POST | `/carts` | Add item to cart |

### Product API (`src/features/product/api/product.api.js`)

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `getProductsByCategoryId(id)` | GET | `/products/category/:id` | List products by category |
| `getProductDetailBySlug(slug)` | GET | `/products/:slug` | Get single product details |

### Category API (`src/features/product/api/category.api.js`)

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `getCategories()` | GET | `/categories` | List all categories |

## Request/Response Pattern

### Standard Request Flow

```
Component
    ↓
Custom Hook (useGetProducts)
    ↓
API Function (getProductsByCategoryId)
    ↓
axiosClient.get()
    ↓
Request Interceptor (adds token)
    ↓
HTTP Request to Backend
    ↓
HTTP Response
    ↓
Response Interceptor (unwraps data)
    ↓
API Function returns data
    ↓
Hook updates state
    ↓
Component re-renders
```

### Request Payload Convention

The backend expects requests wrapped in a `dto` object:

```javascript
// Example from auth.api.js
const dto = {
  identifier: payload.identifier,
  password: payload.password
};
return axiosClient.post(endpoint, { dto });
```

## Environment Configuration

API URL is configured via environment variable:

```env
VITE_API_URL=http://localhost:<BACKEND_PORT>/api/<API_VERSION>
```

For production, this should point to the deployed backend URL.

## Error Handling in Components

Hooks provide `error` state that components can use:

```javascript
const { products, loading, error } = useGetProducts(categoryId);

if (loading) return <Skeleton />;
if (error) return <ErrorMessage message={error.message} />;
return <ProductGrid products={products} />;
```

## Authentication Flow

### Login Request

```
Login Form Submit
    ↓
useLogin.login(credentials)
    ↓
loginAPI(credentials)
    ↓
POST /auth/login { dto: { identifier, password } }
    ↓
Backend validates, returns { accessToken, user }
    ↓
Hook stores token + user in localStorage
    ↓
authContext.login(token, user)
```

### Authenticated Requests

After login, all subsequent requests:
1. Pass through request interceptor
2. Get `Authorization` header automatically added
3. If token expires (401), user is logged out and redirected

## Related Documentation

- [State Management](./state-management.md) - How API data is stored
- [Architecture](./architecture.md) - System overview
