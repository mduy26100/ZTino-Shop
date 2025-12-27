import React, { useMemo } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
    Button, Typography, Spin, Breadcrumb, 
    Tabs, Tag, Descriptions, Empty, Card, 
    Space, Alert 
} from 'antd';
import { 
    ArrowLeftIcon, 
    CubeIcon, 
    SwatchIcon 
} from '@heroicons/react/24/outline';
import { useGetProductDetailById, VariantTable } from '../../features/product';

const { Title, Text } = Typography;

const ProductDetailPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();

    const { data: product, isLoading, error } = useGetProductDetailById(id);

    const breadcrumbItems = useMemo(() => [
        { title: <a onClick={() => navigate('/dashboard')}>Dashboard</a> },
        { title: <a onClick={() => navigate('/products')}>Products</a> },
        { title: product?.name || 'Loading...' },
    ], [navigate, product]);

    const handleBack = () => navigate('/products');

    if (isLoading) {
        return (
            <div className="flex h-[80vh] items-center justify-center">
                <Spin size="large" tip="Loading product details..." />
            </div>
        );
    }

    if (error || !product) {
        return (
            <div className="flex flex-col items-center justify-center h-[60vh] gap-4">
                <Empty description={error?.Error?.Message || "Product not found"} />
                <Button onClick={handleBack}>Back to List</Button>
            </div>
        );
    }

    const tabItems = [
        {
            key: 'overview',
            label: (
                <span className="flex items-center gap-2">
                    <CubeIcon className="w-4 h-4" />
                    Overview
                </span>
            ),
            children: (
                <div className="space-y-6 animate-fade-in">
                    <Card bordered={false} className="shadow-sm rounded-xl">
                        <Descriptions title="Basic Information" bordered column={{ xxl: 2, xl: 2, lg: 2, md: 1, sm: 1, xs: 1 }}>
                            <Descriptions.Item label="Product Name">
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
                            <Descriptions.Item label="Description" span={2}>
                                <div 
                                    className="prose prose-sm max-w-none text-slate-600"
                                    dangerouslySetInnerHTML={{ __html: product.description || 'No description' }}
                                />
                            </Descriptions.Item>
                        </Descriptions>
                    </Card>
                </div>
            ),
        },
        {
            key: 'variants',
            label: (
                <span className="flex items-center gap-2">
                    <SwatchIcon className="w-4 h-4" />
                    Variants ({product.variants?.length || 0})
                </span>
            ),
            children: (
                <div className="animate-fade-in">
                    <Alert 
                        message="Product Variants Management" 
                        description="Manage size, color, pricing, and stock for each product variation."
                        type="info" 
                        showIcon 
                        className="mb-4 rounded-lg border-blue-100 bg-blue-50"
                    />
                    <Card bordered={false} className="shadow-sm rounded-xl !p-0" bodyStyle={{ padding: 0 }}>
                        <VariantTable variants={product.variants} productId={product.id} />
                    </Card>
                </div>
            ),
        },
    ];

    return (
        <div className="space-y-4 pb-10">
            <div className="flex flex-col gap-2">
                <Breadcrumb items={breadcrumbItems} />
                <div className="flex items-center justify-between mt-2">
                    <div className="flex items-center gap-3">
                        <Button 
                            icon={<ArrowLeftIcon className="w-4 h-4" />} 
                            onClick={handleBack}
                            className="border-none shadow-none bg-transparent hover:bg-slate-100"
                        />
                        <div>
                            <Title level={2} className="!mb-0 !text-2xl text-slate-800">
                                {product.name}
                            </Title>
                            <Text type="secondary" className="text-xs">ID: {product.id}</Text>
                        </div>
                    </div>
                    <Space>
                        {/* Placeholder buttons for future actions */}
                        {/* <Button>Edit Product</Button> */}
                    </Space>
                </div>
            </div>

            <Tabs 
                defaultActiveKey="variants" 
                items={tabItems} 
                type="card" 
                className="custom-tabs"
            />
        </div>
    );
};

export default ProductDetailPage;