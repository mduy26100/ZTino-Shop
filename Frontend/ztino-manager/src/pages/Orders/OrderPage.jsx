import React, { useMemo } from 'react';
import { Typography, Spin, Alert, Button } from 'antd';
import { ReloadOutlined } from '@ant-design/icons';
import { useGetAllOrders } from '../../features/order';
import { OrderTable, OrderEmptyState } from '../../features/order';

const { Title } = Typography;

const OrderPage = () => {
    const { data: orders, isLoading, error, refetch } = useGetAllOrders();

    const hasOrders = useMemo(() => {
        return Array.isArray(orders) && orders.length > 0;
    }, [orders]);

    const renderContent = () => {
        if (isLoading) {
            return (
                <div className="flex justify-center items-center py-20">
                    <Spin size="large" tip="Loading orders..." />
                </div>
            );
        }

        if (error) {
            return (
                <Alert
                    type="error"
                    message="Failed to load orders"
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
                    className="mb-4"
                />
            );
        }

        if (!hasOrders) {
            return <OrderEmptyState />;
        }

        return <OrderTable orders={orders} loading={isLoading} />;
    };

    return (
        <div className="container mx-auto px-4 py-8">
            <div className="mb-6 flex items-center justify-between">
                <Title level={2} className="!mb-0">
                    All Orders
                </Title>
                {hasOrders && (
                    <Button
                        icon={<ReloadOutlined />}
                        onClick={refetch}
                    >
                        Refresh
                    </Button>
                )}
            </div>

            <div className="bg-white rounded-xl shadow-sm p-6">
                {renderContent()}
            </div>
        </div>
    );
};

export default OrderPage;