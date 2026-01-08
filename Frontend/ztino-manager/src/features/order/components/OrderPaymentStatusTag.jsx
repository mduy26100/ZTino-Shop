import React, { memo, useMemo } from 'react';
import { Tag } from 'antd';
import {
    ClockCircleOutlined,
    CheckCircleOutlined,
    CloseCircleOutlined,
    RollbackOutlined
} from '@ant-design/icons';

const PAYMENT_STATUS_CONFIG = {
    Pending: {
        color: 'orange',
        icon: ClockCircleOutlined,
        label: 'Pending'
    },
    Completed: {
        color: 'green',
        icon: CheckCircleOutlined,
        label: 'Completed'
    },
    Failed: {
        color: 'red',
        icon: CloseCircleOutlined,
        label: 'Failed'
    },
    Refunded: {
        color: 'purple',
        icon: RollbackOutlined,
        label: 'Refunded'
    }
};

const DEFAULT_STATUS = {
    color: 'default',
    icon: ClockCircleOutlined,
    label: 'Unknown'
};

const OrderPaymentStatusTag = memo(({ status }) => {
    const config = useMemo(() => {
        return PAYMENT_STATUS_CONFIG[status] || DEFAULT_STATUS;
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

OrderPaymentStatusTag.displayName = 'OrderPaymentStatusTag';

export default OrderPaymentStatusTag;
