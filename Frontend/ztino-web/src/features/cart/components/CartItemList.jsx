import React, { memo, useMemo, useCallback } from 'react';
import { Empty, Skeleton, Typography, Button, Checkbox } from 'antd';
import { ShoppingOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import CartItemCard from './CartItemCard';

const { Title } = Typography;

const CartItemList = memo(({ 
    items = [], 
    isLoading = false, 
    onQuantityChange, 
    onRemove,
    isUpdating = false,
    selectedItems = [],
    onSelectedItemsChange 
}) => {
    const navigate = useNavigate();

    const sortedItems = useMemo(() => {
        if (!items || items.length === 0) return [];
        return [...items].sort((a, b) => {
            if (a.isAvailable === b.isAvailable) return 0;
            return a.isAvailable ? -1 : 1;
        });
    }, [items]);

    // Get available items for "select all" logic
    const availableItems = useMemo(() => {
        return items.filter(item => item.isAvailable && item.stockQuantity > 0);
    }, [items]);

    const isAllSelected = useMemo(() => {
        if (availableItems.length === 0) return false;
        return availableItems.every(item => selectedItems.includes(item.cartItemId));
    }, [availableItems, selectedItems]);

    const isIndeterminate = useMemo(() => {
        if (availableItems.length === 0) return false;
        const selectedCount = availableItems.filter(item => selectedItems.includes(item.cartItemId)).length;
        return selectedCount > 0 && selectedCount < availableItems.length;
    }, [availableItems, selectedItems]);

    const handleSelectAll = useCallback((e) => {
        if (onSelectedItemsChange) {
            if (e.target.checked) {
                const allAvailableIds = availableItems.map(item => item.cartItemId);
                onSelectedItemsChange(allAvailableIds);
            } else {
                onSelectedItemsChange([]);
            }
        }
    }, [availableItems, onSelectedItemsChange]);

    const handleItemSelectChange = useCallback((cartItemId, isSelected) => {
        if (onSelectedItemsChange) {
            if (isSelected) {
                onSelectedItemsChange([...selectedItems, cartItemId]);
            } else {
                onSelectedItemsChange(selectedItems.filter(id => id !== cartItemId));
            }
        }
    }, [selectedItems, onSelectedItemsChange]);

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
                <div className="flex items-center gap-4">
                    <Checkbox
                        checked={isAllSelected}
                        indeterminate={isIndeterminate}
                        onChange={handleSelectAll}
                        disabled={availableItems.length === 0}
                    >
                        <span className="text-gray-600 font-medium">Select All</span>
                    </Checkbox>
                    <Title level={4} className="!mb-0 !text-gray-800">
                        Shopping Cart ({items.length} {items.length === 1 ? 'item' : 'items'})
                    </Title>
                </div>
                {selectedItems.length > 0 && (
                    <span className="text-indigo-600 font-medium">
                        {selectedItems.length} item{selectedItems.length > 1 ? 's' : ''} selected
                    </span>
                )}
            </div>

            <div className="flex flex-col gap-4">
                {sortedItems.map(item => (
                    <CartItemCard
                        key={item.cartItemId}
                        item={item}
                        onQuantityChange={onQuantityChange}
                        onRemove={onRemove}
                        isUpdating={isUpdating}
                        isSelected={selectedItems.includes(item.cartItemId)}
                        onSelectChange={handleItemSelectChange}
                    />
                ))}
            </div>
        </div>
    );
});

CartItemList.displayName = 'CartItemList';

export default CartItemList;
