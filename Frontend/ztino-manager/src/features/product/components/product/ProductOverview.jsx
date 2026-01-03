import React, { memo, useMemo } from 'react';
import { Card, Image, Typography, Tag, Descriptions, Table, Button, Space, Tooltip, Spin } from 'antd';
import { PhotoIcon, PlusIcon, PencilIcon, TrashIcon } from '@heroicons/react/24/outline';

const { Text, Title } = Typography;

const ProductOverview = ({ product, productColors, isLoadingProductColors }) => {

    const colorColumns = useMemo(() => [
        {
            title: 'ID',
            dataIndex: 'id',
            key: 'id',
            width: 80,
            align: 'center',
            render: (id) => <Text className="font-mono text-gray-500">#{id}</Text>
        },
        {
            title: 'Color Preview',
            dataIndex: ['color', 'name'],
            key: 'colorPreview',
            width: 120,
            align: 'center',
            render: (colorName) => (
                <div className="flex items-center justify-center">
                    <div 
                        className="w-10 h-10 rounded-lg shadow-md border-2 border-white ring-1 ring-gray-200 transition-transform hover:scale-110"
                        style={{ backgroundColor: colorName }}
                        title={colorName}
                    />
                </div>
            )
        },
        {
            title: 'Color Code',
            dataIndex: ['color', 'name'],
            key: 'colorCode',
            render: (colorName) => (
                <Text copyable code className="font-mono text-sm">
                    {colorName}
                </Text>
            )
        },
        {
            title: 'Color ID',
            dataIndex: ['color', 'id'],
            key: 'colorId',
            width: 100,
            align: 'center',
            render: (id) => <Tag color="blue">ID: {id}</Tag>
        },
        {
            title: 'Actions',
            key: 'actions',
            width: 150,
            align: 'center',
            render: (_, record) => (
                <Space size="small">
                    <Tooltip title="Edit Color">
                        <Button
                            type="text"
                            size="small"
                            icon={<PencilIcon className="w-4 h-4 text-blue-500" />}
                            className="hover:bg-blue-50 rounded-lg"
                            onClick={() => console.log('Edit color:', record)}
                        />
                    </Tooltip>
                    <Tooltip title="Delete Color">
                        <Button
                            type="text"
                            size="small"
                            danger
                            icon={<TrashIcon className="w-4 h-4" />}
                            className="hover:bg-red-50 rounded-lg"
                            onClick={() => console.log('Delete color:', record)}
                        />
                    </Tooltip>
                </Space>
            )
        }
    ], []);

    if (!product) return null;

    return (
        <div className="space-y-6 animate-fade-in">
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                <div className="lg:col-span-1">
                    <Card bordered={false} className="shadow-sm rounded-xl overflow-hidden h-full">
                        <div className="aspect-square bg-slate-50 flex items-center justify-center relative group rounded-lg border border-gray-100 overflow-hidden">
                            {product.mainImageUrl ? (
                                <Image
                                    src={product.mainImageUrl}
                                    alt={product.name}
                                    className="object-cover w-full h-full"
                                    fallback="https://via.placeholder.com/400?text=No+Image"
                                />
                            ) : (
                                <div className="flex flex-col items-center text-gray-400">
                                    <PhotoIcon className="w-16 h-16 mb-2 opacity-50" />
                                    <Text type="secondary">No Main Image</Text>
                                </div>
                            )}
                        </div>
                        <div className="mt-4 text-center">
                            <Tag color="blue">Main Product Image</Tag>
                        </div>
                    </Card>
                </div>

                <div className="lg:col-span-2">
                    <Card bordered={false} className="shadow-sm rounded-xl h-full">
                        <Descriptions title="Basic Information" bordered column={{ xxl: 2, xl: 2, lg: 2, md: 1, sm: 1, xs: 1 }}>
                            <Descriptions.Item label="Product Name" span={2}>
                                <Text strong className="text-lg">{product.name}</Text>
                            </Descriptions.Item>
                            <Descriptions.Item label="Category">
                                {product.category ? <Tag color="purple">{product.category.name}</Tag> : 'N/A'}
                            </Descriptions.Item>
                            <Descriptions.Item label="Base Price">
                                <Text className="font-mono text-emerald-600 font-semibold">
                                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(product.basePrice)}
                                </Text>
                            </Descriptions.Item>
                            <Descriptions.Item label="Slug">
                                <Text copyable code>{product.slug}</Text>
                            </Descriptions.Item>
                            <Descriptions.Item label="Status">
                                <Tag color={product.isActive ? 'success' : 'error'}>
                                    {product.isActive ? 'Active' : 'Inactive'}
                                </Tag>
                            </Descriptions.Item>
                            <Descriptions.Item label="Created At">
                                {(product.createdAt || product.CreatedAt) ? (
                                    <Text type="secondary" className="text-xs">
                                        {new Date(product.createdAt || product.CreatedAt).toLocaleDateString('vi-VN')}
                                    </Text>
                                ) : <Text type="secondary">N/A</Text>}
                            </Descriptions.Item>
                            <Descriptions.Item label="Updated At">
                                {(product.updatedAt || product.UpdatedAt) ? (
                                    <Text type="secondary" className="text-xs">
                                        {new Date(product.updatedAt || product.UpdatedAt).toLocaleDateString('vi-VN')}
                                    </Text>
                                ) : <Text type="secondary">N/A</Text>}
                            </Descriptions.Item>
                            <Descriptions.Item label="Description" span={2}>
                                <div 
                                    className="prose prose-sm max-w-none text-slate-600 max-h-40 overflow-y-auto"
                                    dangerouslySetInnerHTML={{ __html: product.description || 'No description' }}
                                />
                            </Descriptions.Item>
                        </Descriptions>
                    </Card>
                </div>
            </div>

            <Card 
                bordered={false} 
                className="shadow-sm rounded-xl"
                title={
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-2">
                            <div className="w-1 h-6 bg-gradient-to-b from-indigo-500 to-purple-500 rounded-full"></div>
                            <Title level={5} className="!mb-0">
                                Product Colors
                            </Title>
                            <Tag color="blue" className="ml-2">
                                {productColors?.length || 0} colors
                            </Tag>
                        </div>
                        <Button
                            type="primary"
                            icon={<PlusIcon className="w-4 h-4 stroke-2" />}
                            className="h-9 px-4 rounded-lg bg-gradient-to-r from-indigo-500 to-purple-500 border-none shadow-md hover:shadow-lg transition-all hover:scale-105"
                            onClick={() => console.log('Add new color')}
                        >
                            Add Color
                        </Button>
                    </div>
                }
            >
                <Spin spinning={isLoadingProductColors}>
                    <Table
                        columns={colorColumns}
                        dataSource={productColors || []}
                        rowKey="id"
                        pagination={false}
                        size="middle"
                        className="product-colors-table"
                        locale={{
                            emptyText: (
                                <div className="py-8 text-center">
                                    <div className="w-16 h-16 mx-auto mb-4 bg-gray-100 rounded-full flex items-center justify-center">
                                        <PhotoIcon className="w-8 h-8 text-gray-400" />
                                    </div>
                                    <Text type="secondary">No colors available for this product</Text>
                                </div>
                            )
                        }}
                    />
                </Spin>
            </Card>
        </div>
    );
};

export default memo(ProductOverview);