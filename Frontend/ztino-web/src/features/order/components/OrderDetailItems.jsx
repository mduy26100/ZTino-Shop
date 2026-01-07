import React, { memo, useMemo } from 'react';
import { Card, Table, Typography, Image, Tag } from 'antd';

const { Title, Text } = Typography;

const formatCurrency = (amount) => {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
};

const OrderDetailItems = memo(({ items = [] }) => {
    const columns = useMemo(() => [
        {
            title: 'Product',
            key: 'product',
            render: (_, record) => {
                const { productName, sku, thumbnailUrl, colorName, sizeName } = record;
                return (
                    <div className="flex items-center gap-3">
                        <Image
                            src={thumbnailUrl}
                            alt={productName}
                            width={60}
                            height={60}
                            className="rounded-lg object-cover"
                            preview={false}
                            fallback="https://via.placeholder.com/60x60?text=No+Image"
                        />
                        <div className="flex flex-col gap-1 min-w-0">
                            <Text strong className="text-gray-800 truncate" title={productName}>
                                {productName}
                            </Text>
                            <Text className="text-gray-400 text-xs font-mono">
                                {sku}
                            </Text>
                            <div className="flex items-center gap-2 flex-wrap">
                                <div className="flex items-center gap-1">
                                    <div 
                                        className="w-3 h-3 rounded-full border border-gray-300"
                                        style={{ backgroundColor: colorName }}
                                        title={colorName}
                                    />
                                    <Text className="text-gray-500 text-xs">{colorName}</Text>
                                </div>
                                <Tag className="m-0 text-xs">{sizeName}</Tag>
                            </div>
                        </div>
                    </div>
                );
            }
        },
        {
            title: 'Unit Price',
            dataIndex: 'unitPrice',
            key: 'unitPrice',
            width: 120,
            align: 'right',
            render: (unitPrice) => (
                <Text className="text-gray-600">
                    {formatCurrency(unitPrice)}
                </Text>
            )
        },
        {
            title: 'Quantity',
            dataIndex: 'quantity',
            key: 'quantity',
            width: 80,
            align: 'center',
            render: (quantity) => (
                <Text strong className="text-gray-800">
                    {quantity}
                </Text>
            )
        },
        {
            title: 'Total',
            dataIndex: 'totalLineAmount',
            key: 'totalLineAmount',
            width: 130,
            align: 'right',
            render: (totalLineAmount) => (
                <Text className="text-indigo-600 font-semibold">
                    {formatCurrency(totalLineAmount)}
                </Text>
            )
        }
    ], []);

    const dataSource = useMemo(() => {
        return items.map((item, index) => ({
            ...item,
            key: item.sku || index
        }));
    }, [items]);

    return (
        <Card className="shadow-sm rounded-xl mb-6">
            <Title level={5} className="!mb-4 text-gray-800">
                Order Items ({items.length})
            </Title>
            <Table
                columns={columns}
                dataSource={dataSource}
                pagination={false}
                scroll={{ x: 500 }}
                size="small"
                rowClassName="hover:bg-gray-50"
            />
        </Card>
    );
});

OrderDetailItems.displayName = 'OrderDetailItems';

export default OrderDetailItems;
