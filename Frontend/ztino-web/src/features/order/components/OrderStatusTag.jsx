import React, { memo, useMemo } from 'react';
import { Tag } from 'antd';
import {
    ClockCircleOutlined,
    CheckCircleOutlined,
    CarOutlined,
    GiftOutlined,
    CloseCircleOutlined,
    RollbackOutlined
} from '@ant-design/icons';

const ORDER_STATUS_CONFIG = {
    Pending: {
        color: 'orange',
        icon: ClockCircleOutlined,
        label: 'Pending'
    },
    Confirmed: {
        color: 'blue',
        icon: CheckCircleOutlined,
        label: 'Confirmed'
    },
    Shipping: {
        color: 'cyan',
        icon: CarOutlined,
        label: 'Shipping'
    },
    Delivered: {
        color: 'green',
        icon: GiftOutlined,
        label: 'Delivered'
    },
    Cancelled: {
        color: 'red',
        icon: CloseCircleOutlined,
        label: 'Cancelled'
    },
    Returned: {
        color: 'purple',
        icon: RollbackOutlined,
        label: 'Returned'
    }
};

const DEFAULT_STATUS = {
    color: 'default',
    icon: ClockCircleOutlined,
    label: 'Unknown'
};

const OrderStatusTag = memo(({ status }) => {
    const config = useMemo(() => {
        return ORDER_STATUS_CONFIG[status] || DEFAULT_STATUS;
    }, [status]);

    const IconComponent = config.icon;

    return (
        <Tag 
            color={config.color} 
            icon={<IconComponent />}
            className="m-0"
        >
            {config.label}
        </Tag>
    );
});

OrderStatusTag.displayName = 'OrderStatusTag';

export default OrderStatusTag;
