import React, { memo, useMemo, useCallback } from 'react';
import { Typography, Button, Tooltip, message, Card, Divider } from 'antd';
import { ArrowLeftOutlined, CopyOutlined, ReloadOutlined, CalendarOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import OrderStatusTag from './OrderStatusTag';
import OrderPaymentStatusTag from './OrderPaymentStatusTag';

const { Title, Text } = Typography;

const formatDate = (dateString) => {
    return dayjs(dateString).format('DD/MM/YYYY HH:mm');
};

const StatusItem = memo(({ label, children }) => (
    <div className="flex flex-col gap-1">
        <Text className="text-gray-400 text-xs uppercase tracking-wide font-medium">
            {label}
        </Text>
        {children}
    </div>
));

StatusItem.displayName = 'StatusItem';

const OrderDetailHeader = memo(({ order, onRefresh }) => {
    const navigate = useNavigate();
    const { orderCode, status, paymentStatus, createdAt } = order || {};

    const handleBack = useCallback(() => {
        navigate('/orders');
    }, [navigate]);

    const handleCopyOrderCode = useCallback(() => {
        if (orderCode) {
            navigator.clipboard.writeText(orderCode);
            message.success('Order code copied');
        }
    }, [orderCode]);

    const formattedDate = useMemo(() => {
        return createdAt ? formatDate(createdAt) : '';
    }, [createdAt]);

    return (
        <Card className="shadow-sm rounded-xl mb-6">
            <div className="flex flex-col gap-4">
                <div className="flex items-center justify-between">
                    <div className="flex items-center gap-3">
                        <Button 
                            type="text" 
                            icon={<ArrowLeftOutlined />}
                            onClick={handleBack}
                            className="p-0 text-gray-500 hover:text-indigo-600"
                        />
                        <div className="flex items-center gap-2 flex-wrap">
                            <Title level={4} className="!mb-0 font-mono text-indigo-600">
                                {orderCode}
                            </Title>
                            <Tooltip title="Copy order code">
                                <Button 
                                    type="text" 
                                    size="small"
                                    icon={<CopyOutlined />}
                                    onClick={handleCopyOrderCode}
                                    className="text-gray-400 hover:text-indigo-600"
                                />
                            </Tooltip>
                        </div>
                    </div>
                    <Button
                        icon={<ReloadOutlined />}
                        onClick={onRefresh}
                    >
                        Refresh
                    </Button>
                </div>

                <Divider className="!my-2" />

                <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 lg:gap-8">
                    <StatusItem label="Order Status">
                        <OrderStatusTag status={status} />
                    </StatusItem>

                    <StatusItem label="Payment Status">
                        <OrderPaymentStatusTag status={paymentStatus} />
                    </StatusItem>

                    <StatusItem label="Order Date">
                        <div className="flex items-center gap-2">
                            <CalendarOutlined className="text-gray-400" />
                            <Text className="text-gray-700 font-medium">
                                {formattedDate}
                            </Text>
                        </div>
                    </StatusItem>
                </div>
            </div>
        </Card>
    );
});

OrderDetailHeader.displayName = 'OrderDetailHeader';

export default OrderDetailHeader;
