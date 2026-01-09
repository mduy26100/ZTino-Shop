export const ORDER_STATUS = {
    PENDING: 'Pending',
    CONFIRMED: 'Confirmed',
    SHIPPING: 'Shipping',
    DELIVERED: 'Delivered',
    CANCELLED: 'Cancelled',
    RETURNED: 'Returned'
};

export const STATUS_COLOR = {
    Pending: 'orange',
    Confirmed: 'blue',
    Shipping: 'cyan',
    Delivered: 'green',
    Cancelled: 'red',
    Returned: 'purple'
};

export const FINAL_STATUSES = [ORDER_STATUS.CANCELLED, ORDER_STATUS.RETURNED];

export const STATUS_TRANSITIONS = {
    Pending: ['Cancelled'],
    Confirmed: ['Cancelled'],
    Shipping: ['Delivered'], 
    Delivered: ['Returned'],
    Cancelled: [],
    Returned: []
};