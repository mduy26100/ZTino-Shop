import React, { memo } from 'react';
import { Empty, Button } from 'antd';
import { ShoppingOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

const OrderEmptyState = memo(() => {
    const navigate = useNavigate();

    const handleContinueShopping = () => {
        navigate('/products');
    };

    return (
        <div className="flex flex-col items-center justify-center py-16">
            <Empty
                image={<ShoppingOutlined className="text-6xl text-gray-300" />}
                imageStyle={{ height: 80 }}
                description={
                    <div className="text-center">
                        <p className="text-gray-500 text-lg mb-1">
                            You have no orders yet
                        </p>
                        <p className="text-gray-400 text-sm">
                            Explore our products and place your first order!
                        </p>
                    </div>
                }
            >
                <Button 
                    type="primary" 
                    icon={<ShoppingOutlined />}
                    onClick={handleContinueShopping}
                    className="mt-4"
                >
                    Continue Shopping
                </Button>
            </Empty>
        </div>
    );
});

OrderEmptyState.displayName = 'OrderEmptyState';

export default OrderEmptyState;