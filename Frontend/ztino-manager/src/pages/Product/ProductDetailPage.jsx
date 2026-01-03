import React, { useMemo, useState, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
    Button, Typography, Spin, Breadcrumb, 
    Tabs, Empty, Card, Alert, message, Modal 
} from 'antd';
import { 
    ArrowLeftIcon, 
    CubeIcon, 
    SwatchIcon,
    PlusIcon
} from '@heroicons/react/24/outline';
import { ExclamationCircleFilled } from '@ant-design/icons';

import { 
    UpsertProductVariantModal, 
    useCreateProductVariant, 
    useUpdateProductVariant,
    useDeleteProductVariant,
    useGetColors, 
    useGetProductDetailById, 
    useGetSizes, 
    VariantTable,
    ProductImageModal,
    ProductOverview
} from '../../features/product';

const { Title, Text } = Typography;

const ProductDetailPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modalContextHolder] = Modal.useModal();

    const { data: product, isLoading, error, refetch } = useGetProductDetailById(id, {
        onError: (err) => messageApi.open({
            type: 'error',
            content: err?.message || 'Failed to load product details'
        })
    });

    const { data: colors, isLoading: isLoadingColors } = useGetColors();
    const { data: sizes, isLoading: isLoadingSizes } = useGetSizes();
    
    const { create: createVariant, isCreating } = useCreateProductVariant();
    const { update: updateVariant, isUpdating } = useUpdateProductVariant();
    const { remove: removeVariant } = useDeleteProductVariant();

    const [isCreateVariantOpen, setIsCreateVariantOpen] = useState(false);
    const [editingVariant, setEditingVariant] = useState(null);

    const [imageModalState, setImageModalState] = useState({
        open: false,
        variantId: null
    });

    const breadcrumbItems = useMemo(() => [
        { title: <a onClick={() => navigate('/dashboard')}>Dashboard</a> },
        { title: <a onClick={() => navigate('/products')}>Products</a> },
        { title: product?.name || 'Loading...' },
    ], [navigate, product]);

    const handleBack = useCallback(() => navigate('/products'), [navigate]);

    const handleOpenCreateVariant = useCallback(() => {
        setEditingVariant(null);
        setIsCreateVariantOpen(true);
    }, []);

    const handleOpenEditVariant = useCallback((variant) => {
        setEditingVariant(variant);
        setIsCreateVariantOpen(true);
    }, []);

    const handleCloseCreateVariant = useCallback(() => {
        setIsCreateVariantOpen(false);
        setEditingVariant(null);
    }, []);

    const handleDeleteVariant = useCallback((variantId) => {
        modal.confirm({
            title: 'Delete Variant',
            icon: <ExclamationCircleFilled />,
            content: (
                <div className="pt-2">
                    <Text>Are you sure you want to delete this variant?</Text>
                    <br />
                    <Text type="secondary" className="text-xs">
                        This action cannot be undone.
                    </Text>
                </div>
            ),
            okText: 'Delete',
            okType: 'danger',
            cancelText: 'Cancel',
            centered: true,
            maskClosable: true,
            onOk: async () => {
                await removeVariant(variantId, {
                    onSuccess: () => {
                        messageApi.open({
                            type: 'success',
                            content: 'Variant deleted successfully',
                        });
                        refetch();
                    },
                    onError: (error) => {
                        messageApi.open({
                            type: 'error',
                            content: error?.error?.message || error?.message || 'Delete failed',
                        });
                    }
                });
            },
        });
    }, [modal, removeVariant, messageApi, refetch]);

    const handleSubmitVariant = useCallback(async (values) => {
        if (!id) return;

        const isEdit = !!editingVariant;
        const action = isEdit ? updateVariant : createVariant;

        const payload = {
            id: isEdit ? editingVariant.id : undefined,
            productId: parseInt(id),
            colorId: values.colorId,
            sizeId: values.sizeId,
            stockQuantity: values.stockQuantity,
            price: values.price,
            isActive: values.isActive
        };

        await action(payload, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: `Variant ${isEdit ? 'updated' : 'created'} successfully`,
                });
                handleCloseCreateVariant();
                refetch();
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.error?.message || error?.message || 'Operation failed',
                });
            }
        });
    }, [id, editingVariant, createVariant, updateVariant, handleCloseCreateVariant, refetch, messageApi]);

    const handleManageImages = useCallback((variant) => {
        setImageModalState({
            open: true,
            variantId: variant.id
        });
    }, []);

    const handleCloseImageModal = useCallback(() => {
        setImageModalState(prev => ({ ...prev, open: false }));
    }, []);

    const tabItems = useMemo(() => {
        if (!product) return [];

        return [
            {
                key: 'overview',
                label: (
                    <span className="flex items-center gap-2">
                        <CubeIcon className="w-4 h-4" />
                        Overview
                    </span>
                ),
                children: <ProductOverview product={product} />,
            },
            {
                key: 'variants',
                label: (
                    <span className="flex items-center gap-2">
                        <SwatchIcon className="w-4 h-4" />
                        Variants ({product.productColors?.reduce((total, pc) => total + (pc.variants?.length || 0), 0) || 0})
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
                            <VariantTable 
                                productColors={product.productColors} 
                                productId={product.id} 
                                onEdit={handleOpenEditVariant}
                                onDelete={handleDeleteVariant}
                                onManageImages={handleManageImages}
                            />
                        </Card>
                    </div>
                ),
            },
        ];
    }, [product, handleOpenCreateVariant, handleOpenEditVariant, handleDeleteVariant, handleManageImages]);

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
                <Empty description={error?.error?.message || "Product not found"} />
                <Button onClick={handleBack}>Back to List</Button>
            </div>
        );
    }

    return (
        <div className="space-y-4 pb-10">
            {contextHolder}
            {modalContextHolder}
            
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
                confirmLoading={isCreating || isUpdating}
                initialValues={editingVariant}
                colors={colors}
                sizes={sizes}
                isLoadingColors={isLoadingColors}
                isLoadingSizes={isLoadingSizes}
            />

            <ProductImageModal 
                open={imageModalState.open}
                variantId={imageModalState.variantId}
                onCancel={handleCloseImageModal}
                onSuccess={refetch}
            />
        </div>
    );
};

export default ProductDetailPage;