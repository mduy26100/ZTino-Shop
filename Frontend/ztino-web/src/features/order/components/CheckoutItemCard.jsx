import React, { memo, useMemo } from 'react';
import { Card, Image, Typography, Tag } from 'antd';

const { Text, Title } = Typography;

const CheckoutItemCard = memo(({ item }) => {
    const {
        productName,
        mainImageUrl,
        sizeName,
        colorName,
        quantity,
        unitPrice,
        itemTotal
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

    return (
        <Card 
            className="border border-gray-200 rounded-xl hover:shadow-sm transition-shadow"
            styles={{ body: { padding: '16px' } }}
        >
            <div className="flex gap-4">
                <div className="flex-shrink-0">
                    <Image
                        src={mainImageUrl}
                        alt={productName}
                        width={80}
                        height={80}
                        className="rounded-lg object-cover"
                        preview={false}
                        fallback="https://via.placeholder.com/80x80?text=No+Image"
                    />
                </div>

                <div className="flex-1 min-w-0">
                    <Title 
                        level={5} 
                        className="!mb-1 !text-gray-800 truncate"
                        title={productName}
                    >
                        {productName}
                    </Title>

                    <div className="flex flex-wrap items-center gap-2 mb-2">
                        <div className="flex items-center gap-1">
                            <Text className="text-gray-500 text-xs">Color:</Text>
                            <div 
                                className="w-4 h-4 rounded-full border border-gray-300"
                                style={{ backgroundColor: colorName }}
                                title={colorName}
                            />
                        </div>
                        <div className="flex items-center gap-1">
                            <Text className="text-gray-500 text-xs">Size:</Text>
                            <Tag className="m-0 text-xs">{sizeName}</Tag>
                        </div>
                        <div className="flex items-center gap-1">
                            <Text className="text-gray-500 text-xs">Qty:</Text>
                            <Text className="font-medium text-xs">{quantity}</Text>
                        </div>
                    </div>

                    <div className="flex items-center justify-between">
                        <Text className="text-gray-500 text-sm">
                            {formattedUnitPrice} × {quantity}
                        </Text>
                        <Text className="text-indigo-600 font-semibold">
                            {formattedItemTotal}
                        </Text>
                    </div>
                </div>
            </div>
        </Card>
    );
});

CheckoutItemCard.displayName = 'CheckoutItemCard';

export default CheckoutItemCard;
