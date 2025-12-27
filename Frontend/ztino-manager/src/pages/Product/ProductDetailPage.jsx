import React, { useMemo, useState, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
    Button, Typography, Spin, Breadcrumb, 
    Tabs, Tag, Descriptions, Empty, Card, 
    Space, Alert, message, Image 
} from 'antd';
import { 
    ArrowLeftIcon, 
    CubeIcon, 
    SwatchIcon,
    PlusIcon,
    PhotoIcon
} from '@heroicons/react/24/outline';

import { 
    UpsertProductVariantModal, 
    useCreateVariant, 
    useGetColors, 
    useGetProductDetailById, 
    useGetSizes, 
    VariantTable 
} from '../../features/product';

const { Title, Text } = Typography;

const ProductDetailPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [messageApi, contextHolder] = message.useMessage();

    const { data: product, isLoading, error, refetch } = useGetProductDetailById(id, {
        onError: (err) => messageApi.open({
            type: 'error',
            content: err?.message || 'Failed to load product details'
        })
    });

    const { data: colors, isLoading: isLoadingColors } = useGetColors();
    const { data: sizes, isLoading: isLoadingSizes } = useGetSizes();
    const { create: createVariant, isCreating: isCreatingVariant } = useCreateVariant();

    const [isCreateVariantOpen, setIsCreateVariantOpen] = useState(false);

    const breadcrumbItems = useMemo(() => [
        { title: <a onClick={() => navigate('/dashboard')}>Dashboard</a> },
        { title: <a onClick={() => navigate('/products')}>Products</a> },
        { title: product?.name || 'Loading...' },
    ], [navigate, product]);

    const handleBack = useCallback(() => navigate('/products'), [navigate]);

    const handleOpenCreateVariant = useCallback(() => setIsCreateVariantOpen(true), []);
    const handleCloseCreateVariant = useCallback(() => setIsCreateVariantOpen(false), []);

    const handleSubmitVariant = useCallback(async (values) => {
        if (!id) return;

        const payload = {
            productId: parseInt(id),
            colorId: values.colorId,
            sizeId: values.sizeId,
            stockQuantity: values.stockQuantity,
            price: values.price,
            isActive: values.isActive
        };

        await createVariant(payload, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: 'Variant created successfully',
                });
                handleCloseCreateVariant();
                refetch();
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.Error?.Message || error?.message || 'Failed to create variant',
                });
            }
        });
    }, [id, createVariant, handleCloseCreateVariant, refetch, messageApi]);

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
                {contextHolder}
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
                                <div className="mt-4">
                                    <Text type="secondary" className="text-xs block text-center">
                                        Main Product Image
                                    </Text>
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
                                        ) : (
                                            <Text type="secondary" className="text-xs italic">N/A</Text>
                                        )}
                                    </Descriptions.Item>
                                    <Descriptions.Item label="Updated At">
                                        {(product.updatedAt || product.UpdatedAt) ? (
                                            <Text type="secondary" className="text-xs">
                                                {new Date(product.updatedAt || product.UpdatedAt).toLocaleDateString('vi-VN')}
                                            </Text>
                                        ) : (
                                            <Text type="secondary" className="text-xs italic">N/A</Text>
                                        )}
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
                    <div className="flex justify-between items-center mb-4">
                        <Alert 
                            message="Product Variants Management" 
                            description="Manage size, color, pricing, and stock for each product variation."
                            type="info" 
                            showIcon 
                            className="flex-1 mr-4 rounded-lg border-blue-100 bg-blue-50"
                        />
                        <Button 
                            type="primary" 
                            icon={<PlusIcon className="w-4 h-4 stroke-2" />}
                            onClick={handleOpenCreateVariant}
                            className="h-10 px-6 rounded-lg bg-indigo-600 hover:!bg-indigo-700 border-none shadow-md shadow-indigo-100"
                        >
                            Add Variant
                        </Button>
                    </div>
                    
                    <Card bordered={false} className="shadow-sm rounded-xl !p-0" bodyStyle={{ padding: 0 }}>
                        <VariantTable variants={product.variants} productId={product.id} />
                    </Card>
                </div>
            ),
        },
    ];

    return (
        <div className="space-y-4 pb-10">
            {contextHolder}
            
            <div className="flex flex-col gap-2">
                <Breadcrumb items={breadcrumbItems} />
                <div className="flex items-center justify-between mt-2">
                    <div className="flex items-center gap-3">
                        <Button 
                            icon={<ArrowLeftIcon className="w-4 h-4" />} 
                            onClick={handleBack}
                            className="border-none shadow-none bg-transparent hover:bg-slate-100 rounded-full w-8 h-8 flex items-center justify-center p-0"
                        />
                        <div>
                            <Title level={2} className="!mb-0 !text-2xl text-slate-800">
                                {product.name}
                            </Title>
                            <Text type="secondary" className="text-xs">ID: {product.id}</Text>
                        </div>
                    </div>
                </div>
            </div>

            <Tabs 
                defaultActiveKey="variants" 
                items={tabItems} 
                type="card" 
                className="custom-tabs"
            />

            <UpsertProductVariantModal
                open={isCreateVariantOpen}
                onCancel={handleCloseCreateVariant}
                onSubmit={handleSubmitVariant}
                confirmLoading={isCreatingVariant}
                colors={colors}
                sizes={sizes}
                isLoadingColors={isLoadingColors}
                isLoadingSizes={isLoadingSizes}
            />
        </div>
    );
};

export default ProductDetailPage;