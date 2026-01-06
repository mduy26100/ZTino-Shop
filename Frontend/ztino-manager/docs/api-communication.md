# API Communication

This document describes how the application communicates with the backend API.

## Overview

The application uses **Axios** as the HTTP client with a centralized configuration that handles authentication, error normalization, and response transformation.

## Axios Client Configuration

Location: `src/config/axiosClient.js`

### Base Configuration

| Setting | Value |
|---------|-------|
| Base URL | From `VITE_API_URL` env variable |
| Timeout | 30 seconds |
| Content-Type | application/json |
| Accept | application/json |

### Request Interceptor

Automatically injects the JWT access token from localStorage:

- Reads token from `localStorage.getItem("accessToken")`
- Adds `Authorization: Bearer <token>` header if token exists

### Response Interceptor

Handles successful and error responses:

**Success (2xx)**:
- Returns `null` for 204 No Content
- Checks for error in response body (API-level errors)
- Extracts and returns `response.data.data`

**Error Handling**:

| Scenario | Action |
|----------|--------|
| Timeout | Returns structured error with type "timeout" |
| 401 Unauthorized | Clears auth and redirects to login |
| Other HTTP errors | Returns API error response |
| Network errors | Returns structured error with type "network-error" |

## API Endpoints

Location: `src/constants/apiEndpoints.js`

Centralized endpoint definitions:

| Constant | Path |
|----------|------|
| `ENDPOINTS.AUTH` | /auth |
| `ENDPOINTS.PROFILE` | /profile |
| `ENDPOINTS.PRODUCTS` | /products |
| `ENDPOINTS.CATEGORIES` | /categories |
| `ENDPOINTS.COLORS` | /colors |
| `ENDPOINTS.SIZES` | /sizes |
| `ENDPOINTS.ADMIN.PRODUCTS` | /admin/products |
| `ENDPOINTS.ADMIN.CATEGORIES` | /admin/categories |
| `ENDPOINTS.ADMIN.COLORS` | /admin/colors |
| `ENDPOINTS.ADMIN.SIZES` | /admin/sizes |
| `ENDPOINTS.ADMIN.PRODUCT_VARIANTS` | /admin/product-variants |
| `ENDPOINTS.ADMIN.PRODUCT_IMAGES` | /admin/product-images |
| `ENDPOINTS.ADMIN.PRODUCT_COLORS` | /admin/product-colors |

## API Layer Structure

Each feature has its own API file in `src/features/[feature]/api/`:

### Naming Convention

| Operation | Function Pattern | HTTP Method |
|-----------|------------------|-------------|
| List | `get[Entity]s()` | GET |
| Get One | `get[Entity]ById(id)` | GET |
| Create | `create[Entity](data)` | POST |
| Update | `update[Entity](data)` | PUT |
| Delete | `delete[Entity](id)` | DELETE |

### Example: Product API

Location: `src/features/product/api/product.api.js`

| Function | Endpoint | Description |
|----------|----------|-------------|
| `getProducts()` | GET /products | List all products |
| `getProductDetailById(id)` | GET /products/:id | Get product details |
| `createProduct(data)` | POST /admin/products | Create product (FormData) |
| `updateProduct(data)` | PUT /admin/products/:id | Update product (FormData) |
| `deleteProduct(id)` | DELETE /admin/products/:id | Delete product |

### File Upload Handling

Product and image operations use FormData for file uploads:

- Content-Type is set to `multipart/form-data`
- Files are appended to FormData object

## Custom Hooks Pattern

Each API function has a corresponding hook for state management:

| Hook | Responsibility |
|------|----------------|
| `useGet*` | Fetch data on mount, manage loading/error/data |
| `useCreate*` | Expose create function, manage loading/error |
| `useUpdate*` | Expose update function, manage loading/error |
| `useDelete*` | Expose delete function, manage loading/error |

Hooks are located in `src/features/[feature]/hooks/`.

## Error Response Format

The application expects backend errors in this format:

```json
{
  "error": {
    "type": "string",
    "message": "Human readable message",
    "details": null
  },
  "meta": {
    "timestamp": "ISO date string"
  }
}
```

## Related Documentation

- [Authentication](./authentication.md) - Token handling
- [Features Guide](./features/README.md) - Feature-specific APIs
