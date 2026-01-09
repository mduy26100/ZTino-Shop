import React, { useMemo, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { Spin, Alert, Button, Row, Col, message } from 'antd';
import { ReloadOutlined } from '@ant-design/icons';
import { useGetOrderDetail, useUpdateMyOrderStatus } from '../../features/order';
import { 
    OrderDetailHeader,
    OrderDetailInfo,
    OrderDetailItems,
    OrderDetailSummary,
    OrderDetailHistory
} from '../../features/order';

const OrderDetailPage = () => {
    const { orderCode } = useParams();
    const [messageApi, contextHolder] = message.useMessage();
    const { data: order, isLoading, error, refetch } = useGetOrderDetail(orderCode);
    const { updateStatus, isUpdating } = useUpdateMyOrderStatus();

    const items = useMemo(() => {
        return order?.items || [];
    }, [order]);

    const histories = useMemo(() => {
        return order?.histories || [];
    }, [order]);

    const handleUpdateStatus = useCallback((payload, options = {}) => {
        const { onSuccess: externalOnSuccess } = options;

        updateStatus(payload, {
            onSuccess: (response) => {
                messageApi.open({
                    type: 'success',
                    content: response?.message || 'Order status updated successfully',
                });
                refetch();
                if (typeof externalOnSuccess === 'function') {
                    externalOnSuccess(response);
                }
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.error?.message || error?.message || 'Failed to update order status',
                });
            }
        });
    }, [updateStatus, refetch, messageApi]);

    if (isLoading) {
        return (
            <div className="container mx-auto px-4 py-8">
                {contextHolder}
                <div className="flex justify-center items-center py-20">
                    <Spin size="large" tip="Loading order details..." />
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="container mx-auto px-4 py-8">
                {contextHolder}
                <Alert
                    type="error"
                    message="Failed to load order"
                    description={error?.message || 'An error occurred. Please try again later.'}
                    action={
                        <Button 
                            type="primary" 
                            icon={<ReloadOutlined />}
                            onClick={refetch}
                        >
                            Retry
                        </Button>
                    }
                    showIcon
                />
            </div>
        );
    }

    if (!order) {
        return (
            <div className="container mx-auto px-4 py-8">
                {contextHolder}
                <Alert
                    type="warning"
                    message="Order not found"
                    description={`No order found with code: ${orderCode}`}
                    showIcon
                />
            </div>
        );
    }

    return (
        <div className="container mx-auto px-4 py-8 max-w-7xl">
            {contextHolder}
            <OrderDetailHeader 
                order={order} 
                onRefresh={refetch}
                onUpdateStatus={handleUpdateStatus}
                isUpdating={isUpdating}
            />

            <Row gutter={[24, 24]}>
                <Col xs={24} xl={16}>
                    <OrderDetailItems items={items} />
                </Col>
                <Col xs={24} xl={8}>
                    <div className="xl:sticky xl:top-4">
                        <OrderDetailInfo order={order} />
                        <OrderDetailSummary order={order} />
                        <OrderDetailHistory histories={histories} />
                    </div>
                </Col>
            </Row>
        </div>
    );
};

export default OrderDetailPage;