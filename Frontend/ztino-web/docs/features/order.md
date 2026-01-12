# Order Feature

This document describes the order functionality for customer-facing operations.

## Overview

The order feature handles order creation (checkout), order history, order details, and user-initiated status updates.

## Structure

```
src/features/order/
├── api/
│   ├── order.api.js      # Order operations
│   └── index.js
├── hooks/
│   ├── useCreateOrder.js
│   ├── useGetMyOrders.js
│   ├── useGetOrderDetail.js
│   ├── useUpdateMyOrderStatus.js
│   └── index.js
├── components/
│   ├── CheckoutForm.jsx
│   ├── CheckoutItemCard.jsx
│   ├── CheckoutSummary.jsx
│   ├── OrderTable.jsx
│   ├── OrderStatusTag.jsx
│   ├── OrderPaymentStatusTag.jsx
│   ├── OrderEmptyState.jsx
│   ├── OrderDetailHeader.jsx
│   ├── OrderDetailInfo.jsx
│   ├── OrderDetailItems.jsx
│   ├── OrderDetailSummary.jsx
│   ├── OrderDetailHistory.jsx
│   ├── UpdateStatusButton.jsx
│   ├── UpdateStatusModal.jsx
│   └── index.js
├── constants/
│   ├── orderStatus.constants.js
│   └── index.js
└── index.js
```

## API Functions

**File**: `src/features/order/api/order.api.js`

| Function | Method | Endpoint | Purpose |
|----------|--------|----------|---------|
| `createOrder(payload)` | POST | `/orders` | Create new order |
| `getMyOrders()` | GET | `/orders/my-orders` | Get user's orders |
| `getOrderDetail(orderCode)` | GET | `/orders/:orderCode` | Get order details |
| `updateMyOrderStatus(payload)` | PATCH | `/orders/:id/status` | Update order status |

### createOrder Payload

```javascript
{
  cartId: string,
  selectedCartItemIds: string[],
  customerName: string,
  customerPhone: string,
  customerEmail: string,
  shippingAddress: {
    recipientName: string,
    phoneNumber: string,
    street: string,
    ward: string,
    district: string,
    city: string
  },
  note: string
}
```

## Hooks

All hooks use base hooks from `src/hooks/utils/`.

### useCreateOrder

Creates a new order from cart items.

```javascript
const { create, isLoading } = useCreateOrder();

await create({
  cartId: '...',
  selectedCartItemIds: ['item1', 'item2'],
  customerName: 'John Doe',
  // ... other fields
});
```

Automatically invalidates cart cache after successful order creation.

### useGetMyOrders

Fetches authenticated user's order history.

```javascript
const { data: orders, isLoading, error, refetch } = useGetMyOrders();
```

- Cache key: `my-orders`
- Cache TTL: 5 minutes
- Transforms response to ensure array

### useGetOrderDetail

Fetches order details by order code.

```javascript
const { data: order, isLoading, error, refetch } = useGetOrderDetail(orderCode);
```

- Only fetches when `orderCode` is provided

### useUpdateMyOrderStatus

Allows user to update order status (cancel, return, confirm delivery).

```javascript
const { updateStatus, isUpdating } = useUpdateMyOrderStatus();

await updateStatus({
  orderId: '...',
  newStatus: 'Cancelled',
  cancelReason: 'Changed my mind'
});
```

## Constants

**File**: `src/features/order/constants/orderStatus.constants.js`

### ORDER_STATUS

```javascript
{
  PENDING: 'Pending',
  CONFIRMED: 'Confirmed',
  SHIPPING: 'Shipping',
  DELIVERED: 'Delivered',
  CANCELLED: 'Cancelled',
  RETURNED: 'Returned'
}
```

### STATUS_TRANSITIONS (User Actions)

| Current Status | User Can Change To |
|----------------|--------------------|
| Pending | Cancelled |
| Confirmed | Cancelled |
| Shipping | Delivered |
| Delivered | Returned |
| Cancelled | (none) |
| Returned | (none) |

> [!NOTE]
> User status transitions are more restricted than admin transitions. Users can only cancel (before shipping), confirm delivery, or request returns.

### STATUS_COLOR

Color mapping for UI badges:

| Status | Color |
|--------|-------|
| Pending | orange |
| Confirmed | blue |
| Shipping | cyan |
| Delivered | green |
| Cancelled | red |
| Returned | purple |

## Components

### Checkout Components

#### CheckoutForm

Customer and shipping information form with validation.

#### CheckoutItemCard

Individual checkout item display showing product details.

#### CheckoutSummary

Order totals summary with subtotal, shipping, and grand total.

### Order List Components

#### OrderTable

Order history listing table with:
- Order code, date, total, status columns
- Click to view details

#### OrderStatusTag

Colored badge displaying order status.

#### OrderPaymentStatusTag

Colored badge displaying payment status (Pending, Paid, Refunded).

#### OrderEmptyState

Empty state when user has no orders.

### Order Detail Components

#### OrderDetailHeader

Order detail page header with order code and status.

#### OrderDetailInfo

Customer and shipping information display.

#### OrderDetailItems

Order line items with product details.

#### OrderDetailSummary

Order totals section.

#### OrderDetailHistory

Status change history timeline.

#### UpdateStatusButton

Button to trigger status update modal (when allowed).

#### UpdateStatusModal

Modal for changing order status with confirmation.

## Pages

### CheckoutPage

**Path**: `/checkout`

Order creation flow:
- Selected items from cart (via URL params)
- `CheckoutForm` for customer/shipping info
- `CheckoutSummary` for totals
- Order submission

### OrderPage

**Path**: `/orders`

Order history listing with `OrderTable` and empty state.

### OrderDetailPage

**Path**: `/orders/:orderCode`

Detailed order view with status update capabilities (when allowed).

### OrderSuccessPage

**Path**: `/order-success`

Post-checkout success page with order confirmation.

## Related Documentation

- [Cart Feature](./cart.md) - Shopping cart integration
- [API Communication](../api-communication.md) - HTTP patterns
