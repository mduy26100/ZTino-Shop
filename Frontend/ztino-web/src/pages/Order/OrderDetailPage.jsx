import React, { useMemo } from 'react';
import { useParams } from 'react-router-dom';
import { Spin, Alert, Button, Row, Col } from 'antd';
import { ReloadOutlined } from '@ant-design/icons';
import { useGetOrderDetail } from '../../features/order';
import { 
    OrderDetailHeader,
    OrderDetailInfo,
    OrderDetailItems,
    OrderDetailSummary,
    OrderDetailHistory
} from '../../features/order';

const OrderDetailPage = () => {
    const { orderCode } = useParams();
    const { data: order, isLoading, error, refetch } = useGetOrderDetail(orderCode);

    const items = useMemo(() => {
        return order?.items || [];
    }, [order]);

    const histories = useMemo(() => {
        return order?.histories || [];
    }, [order]);

    if (isLoading) {
        return (
            <div className="container mx-auto px-4 py-8">
                <div className="flex justify-center items-center py-20">
                    <Spin size="large" tip="Loading order details..." />
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="container mx-auto px-4 py-8">
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
            <OrderDetailHeader order={order} onRefresh={refetch} />

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
