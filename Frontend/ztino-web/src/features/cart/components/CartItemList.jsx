import React, { memo, useMemo } from 'react';
import { Empty, Skeleton, Typography, Button } from 'antd';
import { ShoppingOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import CartItemCard from './CartItemCard';

const { Title } = Typography;

const CartItemList = memo(({ 
    items = [], 
    isLoading = false, 
    onQuantityChange, 
    onRemove,
    isUpdating = false 
}) => {
    const navigate = useNavigate();

    const sortedItems = useMemo(() => {
        if (!items || items.length === 0) return [];
        return [...items].sort((a, b) => {
            if (a.isAvailable === b.isAvailable) return 0;
            return a.isAvailable ? -1 : 1;
        });
    }, [items]);

    if (isLoading) {
        return (
            <div className="space-y-4">
                {[1, 2, 3].map(i => (
                    <Skeleton 
                        key={i} 
                        active 
                        avatar={{ shape: 'square', size: 100 }} 
                        paragraph={{ rows: 2 }} 
                    />
                ))}
            </div>
        );
    }

    if (!items || items.length === 0) {
        return (
            <div 
                className="flex items-center justify-center w-full"
                style={{ minHeight: 'calc(100vh - 300px)' }}
            >
                <div className="bg-white rounded-2xl py-12 sm:py-16 px-6 sm:px-12 text-center w-full max-w-md mx-auto shadow-sm border border-gray-100">
                    <Empty
                        image={Empty.PRESENTED_IMAGE_SIMPLE}
                        description={
                            <div className="space-y-2 mt-4">
                                <Title level={4} className="!mb-0 !text-gray-700">
                                    Your cart is empty
                                </Title>
                                <p className="text-gray-500 text-sm sm:text-base">
                                    Looks like you haven't added anything to your cart yet.
                                </p>
                            </div>
                        }
                    >
                        <Button 
                            type="primary" 
                            icon={<ShoppingOutlined />}
                            size="large"
                            onClick={() => navigate('/products')}
                            className="bg-indigo-600 hover:!bg-indigo-700 border-none font-medium mt-2"
                        >
                            Start Shopping
                        </Button>
                    </Empty>
                </div>
            </div>
        );
    }

    return (
        <div>
            <div className="flex items-center justify-between mb-4">
                <Title level={4} className="!mb-0 !text-gray-800">
                    Shopping Cart ({items.length} {items.length === 1 ? 'item' : 'items'})
                </Title>
            </div>

            <div className="space-y-0">
                {sortedItems.map(item => (
                    <CartItemCard
                        key={item.cartItemId}
                        item={item}
                        onQuantityChange={onQuantityChange}
                        onRemove={onRemove}
                        isUpdating={isUpdating}
                    />
                ))}
            </div>
        </div>
    );
});

CartItemList.displayName = 'CartItemList';

export default CartItemList;
