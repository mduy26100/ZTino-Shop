import React, { memo, useMemo, useCallback } from 'react';
import { Card, Image, Typography, InputNumber, Button, Tag, Tooltip } from 'antd';
import { DeleteOutlined, ExclamationCircleOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';

const { Text, Title } = Typography;

const CartItemCard = memo(({ 
    item, 
    onQuantityChange, 
    onRemove, 
    isUpdating = false 
}) => {
    const {
        cartItemId,
        productId,
        productName,
        mainImageUrl,
        sizeName,
        colorName,
        quantity,
        unitPrice,
        itemTotal,
        stockQuantity,
        isAvailable
    } = item;

    const formattedUnitPrice = useMemo(() => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(unitPrice);
    }, [unitPrice]);

    const formattedItemTotal = useMemo(() => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(itemTotal);
    }, [itemTotal]);

    const handleQuantityChange = useCallback((value) => {
        if (onQuantityChange && value !== quantity) {
            onQuantityChange(cartItemId, value);
        }
    }, [cartItemId, quantity, onQuantityChange]);

    const handleRemove = useCallback(() => {
        if (onRemove) {
            onRemove(cartItemId);
        }
    }, [cartItemId, onRemove]);

    const isOutOfStock = !isAvailable || stockQuantity === 0;
    const isLowStock = stockQuantity > 0 && stockQuantity <= 5;

    return (
        <Card 
            className={`mb-4 border rounded-xl transition-all ${
                isOutOfStock ? 'border-red-200 bg-red-50' : 'border-gray-200 hover:shadow-md'
            }`}
            styles={{ body: { padding: '16px' } }}
        >
            <div className="flex gap-4">
                <div className="flex-shrink-0">
                    <Link to={`/product/${productId}`}>
                        <Image
                            src={mainImageUrl}
                            alt={productName}
                            width={100}
                            height={100}
                            className="rounded-lg object-cover"
                            preview={false}
                            fallback="https://via.placeholder.com/100x100?text=No+Image"
                        />
                    </Link>
                </div>

                <div className="flex-1 min-w-0">
                    <div className="flex flex-col sm:flex-row sm:justify-between sm:items-start gap-2">
                        <div className="flex-1 min-w-0">
                            <Link to={`/product/${productId}`}>
                                <Title 
                                    level={5} 
                                    className="!mb-1 !text-gray-800 hover:!text-indigo-600 truncate transition-colors"
                                    title={productName}
                                >
                                    {productName}
                                </Title>
                            </Link>

                            <div className="flex flex-wrap items-center gap-2 mb-2">
                                <div className="flex items-center gap-1">
                                    <Text className="text-gray-500 text-sm">Color:</Text>
                                    <div 
                                        className="w-5 h-5 rounded-full border border-gray-300"
                                        style={{ backgroundColor: colorName }}
                                        title={colorName}
                                    />
                                </div>
                                <div className="flex items-center gap-1">
                                    <Text className="text-gray-500 text-sm">Size:</Text>
                                    <Tag className="m-0">{sizeName}</Tag>
                                </div>
                            </div>

                            <Text className="text-indigo-600 font-semibold">
                                {formattedUnitPrice}
                            </Text>
                        </div>

                        <Tooltip title="Remove item">
                            <Button
                                type="text"
                                danger
                                icon={<DeleteOutlined />}
                                onClick={handleRemove}
                                disabled={isUpdating}
                                className="hidden sm:flex items-center justify-center"
                            />
                        </Tooltip>
                    </div>

                    <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between mt-3 gap-3">
                        <div className="flex items-center gap-3">
                            <Text className="text-gray-500 text-sm">Quantity:</Text>
                            <InputNumber
                                min={1}
                                max={stockQuantity}
                                value={quantity}
                                onChange={handleQuantityChange}
                                disabled={isOutOfStock || isUpdating}
                                size="middle"
                                className="w-20"
                            />
                            {isLowStock && (
                                <Tag color="orange" className="flex items-center gap-1">
                                    <ExclamationCircleOutlined />
                                    Only {stockQuantity} left
                                </Tag>
                            )}
                            {isOutOfStock && (
                                <Tag color="red">Out of Stock</Tag>
                            )}
                        </div>

                        <div className="flex items-center justify-between sm:justify-end gap-4">
                            <Text className="text-lg font-bold text-gray-800">
                                {formattedItemTotal}
                            </Text>

                            <Button
                                type="text"
                                danger
                                icon={<DeleteOutlined />}
                                onClick={handleRemove}
                                disabled={isUpdating}
                                className="sm:hidden"
                            >
                                Remove
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </Card>
    );
});

CartItemCard.displayName = 'CartItemCard';

export default CartItemCard;
