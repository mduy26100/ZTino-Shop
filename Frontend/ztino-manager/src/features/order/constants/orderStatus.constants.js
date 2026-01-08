// Order status values
export const ORDER_STATUS = {
    PENDING: 'Pending',
    CONFIRMED: 'Confirmed',
    SHIPPING: 'Shipping',
    DELIVERED: 'Delivered',
    CANCELLED: 'Cancelled',
    RETURNED: 'Returned'
};

// Status color config (for dropdown Tag/Badge styling)
export const STATUS_COLOR = {
    Pending: 'orange',
    Confirmed: 'blue',
    Shipping: 'cyan',
    Delivered: 'green',
    Cancelled: 'red',
    Returned: 'purple'
};

// Final states - no further transitions allowed
export const FINAL_STATUSES = [ORDER_STATUS.CANCELLED, ORDER_STATUS.RETURNED];

// Status transition map
export const STATUS_TRANSITIONS = {
    Pending: ['Confirmed', 'Cancelled'],
    Confirmed: ['Shipping', 'Cancelled'],
    Shipping: ['Delivered', 'Returned'],
    Delivered: ['Returned'],
    Cancelled: [],
    Returned: []
};
