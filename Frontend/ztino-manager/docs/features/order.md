# Order Feature

This document describes the order management functionality for administrators.

## Overview

The order feature enables administrators to view, manage, and update order statuses in the ZTino Manager dashboard.

## Structure

```
src/features/order/
├── api/
│   ├── order.api.js      # Order CRUD operations
│   └── index.js
├── hooks/
│   ├── useGetAllOrders.js
│   ├── useGetOrderDetail.js
│   ├── useUpdateOrderStatus.js
│   └── index.js
├── components/
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
| `getAllOrders()` | GET | `/admin/orders` | Get all orders |
| `getOrderDetail(orderCode)` | GET | `/admin/orders/:orderCode` | Get order details |
| `updateOrderStatus(payload)` | PATCH | `/admin/orders/:id` | Update order status |

### updateOrderStatus Payload

```javascript
{
  orderId: string,
  newStatus: string,   // 'Confirmed' | 'Shipping' | 'Delivered' | 'Cancelled' | 'Returned'
  note: string,        // Optional note
  cancelReason: string // Required if cancelling
}
```

## Hooks

All hooks use base hooks from `src/hooks/utils/`.

### useGetAllOrders

```javascript
const { data, isLoading, error, refetch } = useGetAllOrders();
```

Fetches all orders with caching (key: `orders`).

### useGetOrderDetail

```javascript
const { data, isLoading, error, refetch } = useGetOrderDetail(orderCode);
```

Fetches order details by order code. Only fetches when `orderCode` is provided.

### useUpdateOrderStatus

```javascript
const { updateStatus, isUpdating } = useUpdateOrderStatus();

await updateStatus({
  orderId: '...',
  newStatus: 'Confirmed',
  note: 'Order confirmed by admin'
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

### STATUS_TRANSITIONS

Defines valid status transitions for order workflow:

| Current Status | Allowed Transitions |
|----------------|---------------------|
| Pending | Confirmed, Cancelled |
| Confirmed | Shipping, Cancelled |
| Shipping | Delivered, Returned |
| Delivered | Returned |
| Cancelled | (none) |
| Returned | (none) |

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

### OrderTable

Main order listing table with:
- Sortable columns (order code, date, total, status)
- Status filters
- Row click for details navigation

### OrderStatusTag

Colored badge displaying order status using `STATUS_COLOR` mapping.

### OrderPaymentStatusTag

Colored badge displaying payment status (Pending, Paid, Refunded).

### OrderDetailHeader

Order detail page header with:
- Order code
- Status tag
- Update status button

### UpdateStatusModal

Modal for changing order status:
- Dropdown with valid next statuses (from `STATUS_TRANSITIONS`)
- Note input field
- Cancel reason input (when cancelling)

### OrderDetailInfo

Customer and shipping information display.

### OrderDetailItems

Order line items table with product details, quantities, and prices.

### OrderDetailSummary

Order totals section showing subtotal, shipping, discounts, and grand total.

### OrderDetailHistory

Status change history timeline.

## Pages

### OrderPage

**Path**: `/orders`

Order listing with `OrderTable`, filters, and empty state handling.

### OrderDetailPage

**Path**: `/orders/:orderCode`

Detailed order view with:
- `OrderDetailHeader`
- `OrderDetailInfo` (customer info, addresses)
- `OrderDetailItems` (line items)
- `OrderDetailSummary` (totals)
- `OrderDetailHistory` (status change history)

## Related Documentation

- [API Communication](../api-communication.md) - HTTP patterns
- [Folder Structure](../folder-structure.md) - File organization
