# Exception Handling

This document explains the centralized exception handling strategy.

## Overview

All exceptions are handled by middleware that:
- Catches unhandled exceptions
- Maps to appropriate HTTP status codes
- Returns consistent error responses
- Logs errors appropriately

## ExceptionHandlingMiddleware

**Location**: `ZTino_Shop/src/WebAPI/Middleware/ExceptionHandling/ExceptionHandlingMiddleware.cs`

### Pipeline Position

```
Request
    │
    ▼
Authentication
    │
    ▼
┌─────────────────────────────┐
│  ExceptionHandlingMiddleware │  ← Catches all exceptions
└─────────────────────────────┘
    │
    ▼
Controllers / Handlers
```

Registered in `Program.cs`:
```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

---

## Exception Mapping

| Exception Type | HTTP Status | Error Type |
|---------------|-------------|------------|
| `ValidationException` | 400 | `validation-error` |
| `BusinessRuleException` | 400 | `business-rule-violation` |
| `UnauthorizedAccessException` | 401 | `unauthorized` |
| `ForbiddenException` | 403 | `forbidden` |
| `NotFoundException` | 404 | `not-found` |
| `ConflictException` | 409 | `conflict` |
| Other exceptions | 500 | `internal-server-error` |

---

## Custom Exceptions

**Location**: `ZTino_Shop/src/Application.Common/Exceptions/`

### Base Class

```csharp
public abstract class ApplicationExceptionBase : Exception
{
    protected ApplicationExceptionBase(string message) : base(message) { }
}
```

### Exception Types

| Exception | Purpose | Example Usage |
|-----------|---------|---------------|
| `NotFoundException` | Resource not found | Product with ID doesn't exist |
| `ConflictException` | Duplicate or conflict | Email already registered |
| `ForbiddenException` | Insufficient permissions | User accessing other's order |
| `BusinessRuleException` | Business rule violation | Insufficient stock |

---

## Error Response Format

All errors follow the standard envelope:

```json
{
  "success": false,
  "error": {
    "type": "not-found",
    "message": "Product with ID 123 was not found."
  },
  "meta": {
    "timestamp": "2025-01-06T12:00:00Z",
    "path": "/api/v1/products/123",
    "method": "GET",
    "statusCode": 404,
    "traceId": "abc123",
    "requestId": "req-456",
    "clientIp": "192.168.1.1"
  }
}
```

### Validation Error Format

```json
{
  "success": false,
  "error": {
    "type": "validation-error",
    "message": "Validation failed.",
    "details": {
      "Name": ["Name is required"],
      "Price": ["Price must be greater than 0"]
    }
  },
  "meta": { ... }
}
```

---

## Non-Exception Status Codes

Middleware also handles HTTP status responses without exceptions:

| Status | Response |
|--------|----------|
| 401 | Unauthorized message |
| 403 | Access denied message |
| 404 | Resource not found message |
| 405 | Method not allowed message |

---

## Throwing Exceptions

### In Handlers

```csharp
var product = await _repository.GetByIdAsync(request.Id);
if (product is null)
    throw new NotFoundException($"Product with ID {request.Id} was not found.");
```

### In Validators

FluentValidation throws `ValidationException` automatically when validation fails.

---

## Logging

| Severity | When |
|----------|------|
| Warning | Response already started |
| Error | Unhandled exceptions (500) |
| (none) | Expected exceptions (4xx) |

---

## Best Practices

1. **Use specific exceptions**: Choose the right exception type
2. **Meaningful messages**: Help clients understand the error
3. **Don't leak internals**: Hide implementation details in 500 errors
4. **Include identifiers**: Help with debugging
5. **Log appropriately**: More detail for unexpected errors
