# Shopping Cart Feature

This document describes the shopping cart functionality and its components.

## Overview

The cart feature manages the user's shopping cart, supporting both authenticated users and guest checkout via localStorage cart ID persistence.

## Structure

```
src/features/cart/
├── api/
│   ├── cart.api.js      # Cart CRUD operations
│   └── index.js
├── hooks/
│   ├── useGetMyCart.js
│   ├── useGetCartById.js
│   ├── useCreateCart.js
│   ├── useUpdateCart.js
│   ├── useDeleteCart.js
│   └── index.js
├── components/
│   ├── CartItemCard.jsx
│   ├── CartItemList.jsx
│   ├── CartSummary.jsx
│   └── index.js
└── index.js
```

## API Functions

**File**: `src/features/cart/api/cart.api.js`

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `getMyCart()` | GET | `/carts` | Fetch authenticated user's cart |
| `getCartById(id)` | GET | `/carts/:id` | Fetch cart by ID (guest users) |
| `createCart(cart)` | POST | `/carts` | Add item to cart |
| `updateCart(cart)` | PUT | `/carts/:id` | Update cart item quantity |
| `deleteCart(cartItemId)` | DELETE | `/carts/:id` | Remove item from cart |

### createCart Payload

```javascript
{
  productVariantId: string,  // Required: variant to add
  quantity: number,          // Required: quantity to add
  cartId: string            // Optional: for guest carts
}
```

## Hooks

### useGetMyCart

Fetches the cart for authenticated users.

**Usage**: For logged-in users, call this hook to get their cart.

**Returns**:
- `cart` - Cart object with items
- `loading` - Loading state
- `error` - Error object if failed
- `refetch` - Function to refresh cart data

### useGetCartById

Fetches a specific cart by ID, primarily for guest users.

**Input**: Cart ID (from localStorage `guestCartId`)

**Returns**:
- `cart` - Cart object with items
- `loading` - Loading state
- `error` - Error object if failed

### useCreateCart

Adds an item to the cart.

**Returns**:
- `create(payload)` - Function to add item
- `isLoading` - Loading state

**Guest Cart Flow**:
1. Check if user is authenticated
2. If not, check localStorage for `guestCartId`
3. Include `cartId` in payload if available
4. On successful response, store returned `cartId` if new
5. Automatically invalidates cart cache

### useUpdateCart

Updates cart item quantity.

**Returns**:
- `update(payload)` - Update function
- `isLoading` - Loading state
- `updatingItemId` - ID of currently updating item

**Payload**:
```javascript
{
  cartId: string,           // Required
  productVariantId: string, // Required
  quantity: number          // New quantity
}
```

Automatically invalidates cart cache after successful update.

### useDeleteCart

Removes an item from the cart.

**Returns**:
- `remove(cartItemId)` - Remove function
- `isLoading` - Loading state
- `deletingItemId` - ID of currently deleting item

Automatically invalidates cart cache after successful removal.

## Cache Invalidation

Cart hooks use `invalidateCartCacheByAuth` from `src/hooks/utils/` to automatically refresh cart data after mutations.

**Location**: `src/hooks/utils/invalidateCartCache.js`

### invalidateCartCacheByAuth

```javascript
invalidateCartCacheByAuth(isAuthenticated, cartIdOverride?)
```

| Parameter | Type | Description |
|-----------|------|-------------|
| `isAuthenticated` | `boolean` | From `useAuth()` context |
| `cartIdOverride` | `string?` | Optional guest cart ID |

**Behavior**:
- **Authenticated users**: Invalidates `my-cart` cache key
- **Guest users**: Invalidates `guest-cart-{id}` cache key

**Used by**:
- `useCreateCart` - After adding item
- `useUpdateCart` - After quantity change
- `useDeleteCart` - After item removal
- `useCreateOrder` - After order placement (clears cart)

## Components

### CartItemCard

Individual cart item display with:
- Product image, name, variant info
- Price display
- Quantity controls
- Remove button

### CartItemList

Container for cart items:
- Maps over cart items to render `CartItemCard`
- Handles empty cart state
- Loading skeleton

### CartSummary

Order summary sidebar:
- Subtotal calculation
- Discount display (if applicable)
- Total amount
- Checkout button

## Page

### CartPage (`src/pages/Cart/CartPage.jsx`)

Full cart view with:
- `CartItemList` for items
- `CartSummary` for totals
- Empty cart handling
- Loading states

## Guest vs Authenticated Flow

### Authenticated Users
```
User adds item
    ↓
useCreateCart calls API (no cartId)
    ↓
Backend creates/updates user's cart
    ↓
useGetMyCart fetches updated cart
```

### Guest Users
```
Guest adds item
    ↓
Check localStorage for guestCartId
    ↓
useCreateCart calls API (with cartId if exists)
    ↓
Backend returns cart with cartId
    ↓
Store cartId in localStorage (if new)
    ↓
useGetCartById fetches cart by stored ID
```

## Cart Persistence

**Key**: `guestCartId` in localStorage

**Functions** (in `src/utils/localStorage.js`):
- `setGuestCartId(cartId)` - Store cart ID
- `getGuestCartId()` - Retrieve cart ID
- `removeGuestCartId()` - Clear cart ID

## Related Documentation

- [State Management](../state-management.md) - Data flow patterns
- [API Communication](../api-communication.md) - HTTP patterns
