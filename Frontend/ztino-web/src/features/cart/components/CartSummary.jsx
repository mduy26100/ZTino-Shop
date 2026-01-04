import React, { memo, useMemo } from 'react';
import { Card, Typography, Button, Divider, Statistic } from 'antd';
import { ShoppingOutlined, SafetyOutlined, TruckOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

const { Text, Title } = Typography;

const CartSummary = memo(({ 
    totalItems = 0, 
    totalPrice = 0,
    isLoading = false,
    onCheckout 
}) => {
    const navigate = useNavigate();

    const formattedTotalPrice = useMemo(() => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(totalPrice);
    }, [totalPrice]);

    const handleCheckout = () => {
        if (onCheckout) {
            onCheckout();
        } else {
            navigate('/checkout');
        }
    };

    return (
        <Card 
            className="border-0 shadow-lg rounded-2xl sticky top-24"
            styles={{ body: { padding: '24px' } }}
        >
            <Title level={4} className="!mb-4 !text-gray-800">
                Order Summary
            </Title>

            <div className="space-y-3">
                <div className="flex justify-between items-center">
                    <Text className="text-gray-600">Items ({totalItems})</Text>
                    <Text className="font-medium">{formattedTotalPrice}</Text>
                </div>

                <div className="flex justify-between items-center">
                    <Text className="text-gray-600">Shipping</Text>
                    <Text className="text-green-600 font-medium">Free</Text>
                </div>
            </div>

            <Divider className="my-4" />

            <div className="flex justify-between items-center mb-6">
                <Text className="text-lg font-semibold text-gray-800">Total</Text>
                <Statistic 
                    value={totalPrice} 
                    precision={0}
                    prefix=""
                    suffix="₫"
                    valueStyle={{ 
                        fontSize: '1.5rem', 
                        fontWeight: 'bold', 
                        color: '#4f46e5' 
                    }}
                    formatter={(value) => new Intl.NumberFormat('vi-VN').format(value)}
                />
            </div>

            <Button
                type="primary"
                size="large"
                block
                icon={<ShoppingOutlined />}
                onClick={handleCheckout}
                loading={isLoading}
                disabled={totalItems === 0}
                className="h-12 bg-indigo-600 hover:!bg-indigo-700 border-none font-medium rounded-lg shadow-none"
            >
                Proceed to Checkout
            </Button>

            <div className="mt-6 space-y-3">
                <div className="flex items-center gap-3 text-gray-500">
                    <SafetyOutlined className="text-green-500 text-lg" />
                    <Text className="text-sm">Secure checkout with SSL encryption</Text>
                </div>
                <div className="flex items-center gap-3 text-gray-500">
                    <TruckOutlined className="text-indigo-500 text-lg" />
                    <Text className="text-sm">Free shipping on all orders</Text>
                </div>
            </div>
        </Card>
    );
});

CartSummary.displayName = 'CartSummary';

export default CartSummary;
