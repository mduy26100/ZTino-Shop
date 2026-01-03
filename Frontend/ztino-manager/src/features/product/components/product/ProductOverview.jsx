import React, { memo } from 'react';
import { Card, Image, Typography, Tag, Descriptions } from 'antd';
import { PhotoIcon } from '@heroicons/react/24/outline';

const { Text } = Typography;

const ProductOverview = ({ product }) => {
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
        </div>
    );
};

export default memo(ProductOverview);