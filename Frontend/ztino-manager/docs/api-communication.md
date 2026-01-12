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
| `ENDPOINTS.ADMIN.ORDER` | /admin/orders |

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

## Base Hook Utilities

Location: `src/hooks/utils/`

Feature hooks are built on reusable base hooks that eliminate boilerplate code.

### useQuery

Caching-enabled hook for data fetching.

**Signature:**
```javascript
const { data, isLoading, error, refetch, isCached } = useQuery(key, queryFn, options);
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `key` | `string \| null` | Unique cache key. Pass `null` to disable. |
| `queryFn` | `(ctx) => Promise` | Async function receiving `{ signal }` for abort |
| `options.ttl` | `number` | Cache TTL in ms (default: 5 min) |
| `options.enabled` | `boolean` | Enable auto-fetch (default: `true`) |
| `options.initialData` | `any` | Data before first fetch |
| `options.transformResponse` | `function` | Transform raw response |
| `options.onSuccess` | `function` | Success callback |
| `options.onError` | `function` | Error callback |

**Returns:**

| Property | Type | Description |
|----------|------|-------------|
| `data` | `any` | Fetched/cached data |
| `isLoading` | `boolean` | Loading state |
| `error` | `Error \| null` | Error if failed |
| `refetch` | `function` | Force refetch (bypasses cache) |
| `isCached` | `boolean` | Whether data came from cache |

**Cache Management:**
```javascript
import { invalidateCache, clearGlobalCache } from '../hooks/utils';

invalidateCache('products');  // Clear specific key
clearGlobalCache();           // Clear all cached data
```

### useMutation

Hook for mutation operations (create/update/delete).

**Signature:**
```javascript
const { mutate, isLoading, error, mutatingVariables, reset } = useMutation(mutationFn, options);
```

**Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `mutationFn` | `(variables) => Promise` | Async mutation function |
| `options.onMutate` | `function` | Called before mutation |
| `options.onSuccess` | `function` | Called on success with `(result, variables)` |
| `options.onError` | `function` | Called on error with `(error, variables)` |
| `options.onSettled` | `function` | Called after mutation completes |

**Returns:**

| Property | Type | Description |
|----------|------|-------------|
| `mutate` | `function` | Trigger mutation with variables |
| `isLoading` | `boolean` | Loading state |
| `error` | `Error \| null` | Error if failed |
| `mutatingVariables` | `any` | Current mutation variables |
| `reset` | `function` | Reset hook state |

### Example: Feature Hook Implementation

```javascript
// Query hook - just 6 lines!
import { useQuery } from '../../../hooks/utils';
import { getCategories } from '../api';

export const useGetCategories = () => {
    return useQuery('categories', getCategories, { initialData: [] });
};

// Mutation hook - just 7 lines!
import { useMutation } from '../../../hooks/utils';
import { createCategory } from '../api';

export const useCreateCategory = () => {
    const { mutate, isLoading } = useMutation(createCategory);
    return { create: mutate, isLoading };
};
```

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
