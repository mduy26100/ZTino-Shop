# API Standards

This document describes API conventions for Frontend and Mobile developers.

## Base URL

```
https://localhost:<ZTino_SHOP-PORT>/api/v1/
```

## RESTful URL Conventions

### Resource Naming
- Use **plural nouns** for collections: `/products`, `/carts`, `/orders`
- Use **kebab-case** for multi-word resources: `/order-items`
- Nest sub-resources: `/products/{id}/images`

### HTTP Methods

| Method | Purpose | Example |
|--------|---------|---------|
| `GET` | Read resource(s) | `GET /products` |
| `POST` | Create resource | `POST /carts` |
| `PUT` | Full update | `PUT /products/{id}` |
| `PATCH` | Partial update | `PATCH /orders/{id}/status` |
| `DELETE` | Remove resource | `DELETE /carts/{id}/items/{itemId}` |

### URL Patterns

```
GET    /api/v1/products              # List all products
GET    /api/v1/products/{id}         # Get single product
POST   /api/v1/products              # Create product
PUT    /api/v1/products/{id}         # Update product
DELETE /api/v1/products/{id}         # Delete product

GET    /api/v1/products/{id}/images  # Get product images (nested)
POST   /api/v1/auth/login            # Action-based (exception)
POST   /api/v1/auth/refresh-token    # Action-based (exception)
```

---

## Response Envelope

All responses use a standard envelope format:

### Success Response

```json
{
  "success": true,
  "data": { ... },
  "meta": {
    "timestamp": "2025-01-06T12:00:00Z",
    "path": "/api/v1/products/1",
    "method": "GET",
    "statusCode": 200,
    "traceId": "abc123",
    "requestId": "req-456",
    "clientIp": "192.168.1.1"
  }
}
```

### Error Response

```json
{
  "success": false,
  "error": {
    "type": "validation-error",
    "message": "Validation failed.",
    "details": {
      "Name": ["Name is required", "Name must be at least 3 characters"],
      "Price": ["Price must be greater than 0"]
    }
  },
  "meta": {
    "timestamp": "2025-01-06T12:00:00Z",
    "path": "/api/v1/products",
    "method": "POST",
    "statusCode": 400,
    "traceId": "abc123"
  }
}
```

---

## Error Types

| Type | HTTP Status | When |
|------|-------------|------|
| `validation-error` | 400 | Request validation failed |
| `business-rule-violation` | 400 | Business rule not satisfied |
| `unauthorized` | 401 | Missing/invalid authentication |
| `forbidden` | 403 | Authenticated but not authorized |
| `not-found` | 404 | Resource doesn't exist |
| `conflict` | 409 | Duplicate or conflict |
| `internal-server-error` | 500 | Unexpected error |

---

## Authentication

### JWT Bearer Token

Include in Authorization header:
```
Authorization: Bearer <access_token>
```

### Token Endpoints

| Endpoint | Purpose |
|----------|---------|
| `POST /api/v1/auth/login` | Get access + refresh tokens |
| `POST /api/v1/auth/refresh-token` | Refresh access token |
| `POST /api/v1/auth/register` | Create new account |
| `POST /api/v1/auth/logout` | Invalidate refresh token |

### Token Expiry

| Token | Default Expiry |
|-------|----------------|
| Access Token | 15 minutes |
| Refresh Token | 7 days |

---

## Pagination

List endpoints support pagination:

```
GET /api/v1/products?page=1&pageSize=20
```

Response includes pagination info in `data`:
```json
{
  "success": true,
  "data": {
    "items": [...],
    "totalCount": 150,
    "page": 1,
    "pageSize": 20,
    "totalPages": 8
  }
}
```

---

## Filtering and Sorting

### Query Parameters

```
GET /api/v1/products?categoryId=5&minPrice=100&maxPrice=500
GET /api/v1/products?sortBy=price&sortOrder=desc
GET /api/v1/products?search=shirt
```

---

## API Versioning

### Current Version: v1

All endpoints are prefixed with `/api/v1/`.

### Future Versions

New versions will be introduced as `/api/v2/` when breaking changes are needed. Old versions will be supported during transition.

### Version in Controllers

Controllers are organized by version:
- `ZTino_Shop/src/WebAPI/Controllers/v1/Products/`
- `ZTino_Shop/src/WebAPI/Controllers/v1/Auth/`

---

## Swagger UI

### Access
```
https://localhost:<ZTino_SHOP-PORT>/swagger
```

### Features
- Interactive API documentation
- Try endpoints directly from browser
- View request/response schemas
- Test authentication

### Authorize in Swagger
1. Click "Authorize" button
2. Enter: `Bearer <your_access_token>`
3. Click "Authorize"
4. All subsequent requests include the token

---

## Content Types

| Request Body | Content-Type |
|--------------|--------------|
| JSON | `application/json` |
| File Upload | `multipart/form-data` |

All responses return `application/json`.

---

## Common Endpoints

### Auth
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/auth/login` | Login | No |
| POST | `/auth/register` | Register | No |
| POST | `/auth/refresh-token` | Refresh token | No |
| POST | `/auth/logout` | Logout | Yes |
| POST | `/auth/facebook` | Facebook OAuth | No |
| POST | `/auth/google` | Google OAuth | No |

### Products
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/products` | List products | No |
| GET | `/products/{id}` | Get product | No |
| POST | `/products` | Create product | Admin |
| PUT | `/products/{id}` | Update product | Admin |
| DELETE | `/products/{id}` | Delete product | Admin |

### Cart
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/carts/{id}` | Get cart | Optional |
| POST | `/carts` | Add to cart | Optional |
| DELETE | `/carts/{cartId}/items/{itemId}` | Remove item | Optional |

### Orders
| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/orders` | List user orders | Yes |
| GET | `/orders/{id}` | Get order | Yes |
| POST | `/orders` | Create order | Yes |
