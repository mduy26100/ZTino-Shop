# Caching

This document explains the caching strategy using Redis.

## Overview

- **Cache Provider**: Redis
- **Implementation**: `RedisCacheService`
- **Purpose**: Reduce database load, improve response times

## Configuration

**Location**: `ZTino_Shop/src/WebAPI/appsettings.template.json`

```json
{
  "Redis": {
    "ConnectionString": "localhost:6379"
  }
}
```

---

## Cache Service

### Interface

**Location**: `ZTino_Shop/src/Application.Common/Abstractions/Caching/ICacheService.cs`

Common operations:
- `GetAsync<T>(key)` - Retrieve cached value
- `SetAsync<T>(key, value, expiry)` - Store value
- `RemoveAsync(key)` - Invalidate cache
- `ExistsAsync(key)` - Check existence

### Implementation

**Location**: `ZTino_Shop/src/Infrastructure/Caching/RedisCacheService.cs`

Uses `StackExchange.Redis` for Redis communication.

---

## Registration

**Location**: `ZTino_Shop/src/WebAPI/DependencyInjection/Infrastructure/CachingRegistration.cs`

Redis connection is configured and `ICacheService` is registered as singleton.

---

## Cache Usage Patterns

### Cache-Aside Pattern

Most common approach:

```
1. Check cache
2. If hit → return cached
3. If miss → fetch from DB
4. Store in cache
5. Return data
```

### Cache Invalidation

On data modification:
- Delete related cache keys
- Or set short TTL and accept staleness

---

## Cache Key Conventions

Suggested format:
```
{entity}:{id}           → product:123
{entity}:list:{params}  → products:list:category=5
{user}:{entity}:{id}    → user:abc:cart
```

---

## When to Cache

| Scenario | Cache? | TTL |
|----------|--------|-----|
| Product catalog | Yes | Long (hours) |
| User-specific data | Careful | Short (minutes) |
| Real-time data | No | - |
| Expensive queries | Yes | Medium |

---

## Best Practices

1. **Meaningful keys**: Include entity and identifier
2. **Appropriate TTL**: Balance freshness vs. performance
3. **Invalidate on write**: Keep cache consistent
4. **Handle cache failures gracefully**: Fall back to database
5. **Monitor cache hit rates**: Tune as needed

---

## Optional Feature

Redis caching is optional:
- If Redis not configured, application works without caching
- Graceful degradation to database-only
