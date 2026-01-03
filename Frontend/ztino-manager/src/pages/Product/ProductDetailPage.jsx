import React, { useMemo, useState, useCallback } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
    Button, Typography, Spin, Breadcrumb, 
    Tabs, Empty, Card, Alert, message, Modal, Select 
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
    useGetColorsByProductId,
    useCreateProductColor,
    useDeleteProductColor,
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
    const { data: productColors, isLoading: isLoadingProductColors, refetch: refetchProductColors } = useGetColorsByProductId(id);
    
    const { create: createVariant, isCreating } = useCreateProductVariant();
    const { update: updateVariant, isUpdating } = useUpdateProductVariant();
    const { remove: removeVariant } = useDeleteProductVariant();
    const { create: createProductColor, isCreating: isCreatingProductColor } = useCreateProductColor();
    const { remove: removeProductColor } = useDeleteProductColor();

    const [isCreateVariantOpen, setIsCreateVariantOpen] = useState(false);
    const [editingVariant, setEditingVariant] = useState(null);
    const [selectedColorId, setSelectedColorId] = useState(null);
    const [isAddColorModalOpen, setIsAddColorModalOpen] = useState(false);

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
            productColorId: values.productColorId,
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

    const handleOpenAddColorModal = useCallback(() => {
        setIsAddColorModalOpen(true);
    }, []);

    const handleCloseAddColorModal = useCallback(() => {
        setIsAddColorModalOpen(false);
    }, []);

    const handleAddProductColor = useCallback(async (colorId) => {
        if (!id || !colorId) return;

        await createProductColor({ productId: parseInt(id), colorId }, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: 'Color added to product successfully!',
                });
                handleCloseAddColorModal();
                refetchProductColors();
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.error?.message || error?.message || 'Failed to add color',
                });
            }
        });
    }, [id, createProductColor, messageApi, handleCloseAddColorModal, refetchProductColors]);

    const handleDeleteProductColor = useCallback((productColorId) => {
        modal.confirm({
            title: 'Delete Color',
            icon: <ExclamationCircleFilled />,
            content: (
                <div className="pt-2">
                    <Text>Are you sure you want to remove this color from the product?</Text>
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
                await removeProductColor(productColorId, {
                    onSuccess: () => {
                        messageApi.open({
                            type: 'success',
                            content: 'Color removed from product successfully',
                        });
                        refetchProductColors();
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
    }, [modal, removeProductColor, messageApi, refetchProductColors]);


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
                children: <ProductOverview 
                    product={product} 
                    productColors={productColors}
                    isLoadingProductColors={isLoadingProductColors}
                    colors={colors}
                    isLoadingColors={isLoadingColors}
                    onAddColor={handleOpenAddColorModal}
                    onDeleteColor={handleDeleteProductColor}
                />,
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
                productColors={productColors}
                sizes={sizes}
                isLoadingProductColors={isLoadingProductColors}
                isLoadingSizes={isLoadingSizes}
            />

            <ProductImageModal 
                open={imageModalState.open}
                variantId={imageModalState.variantId}
                onCancel={handleCloseImageModal}
                onSuccess={refetch}
            />

            <Modal
                title={
                    <div className="flex items-center gap-3">
                        <div className="w-10 h-10 rounded-xl bg-gradient-to-br from-indigo-500 to-purple-600 flex items-center justify-center shadow-lg">
                            <SwatchIcon className="w-5 h-5 text-white" />
                        </div>
                        <div>
                            <Text strong className="text-lg">Add Color to Product</Text>
                            <br />
                            <Text type="secondary" className="text-xs">Select a color to add to this product</Text>
                        </div>
                    </div>
                }
                open={isAddColorModalOpen}
                onCancel={() => {
                    handleCloseAddColorModal();
                    setSelectedColorId(null);
                }}
                onOk={() => handleAddProductColor(selectedColorId)}
                okText="Add Color"
                cancelText="Cancel"
                confirmLoading={isCreatingProductColor}
                okButtonProps={{ 
                    disabled: !selectedColorId,
                    className: 'bg-indigo-600 hover:!bg-indigo-700 border-none shadow-md'
                }}
                centered
                destroyOnClose
                className="add-color-modal"
            >
                <div className="py-4">
                    <Text className="block mb-2 text-gray-600">Choose Color</Text>
                    <Select
                        placeholder="Select a color..."
                        className="w-full"
                        size="large"
                        loading={isLoadingColors}
                        value={selectedColorId}
                        onChange={setSelectedColorId}
                        optionLabelProp="label"
                        showSearch
                        filterOption={(input, option) =>
                            option?.searchValue?.toLowerCase().includes(input.toLowerCase())
                        }
                    >
                        {colors?.map(color => (
                            <Select.Option 
                                key={color.id} 
                                value={color.id}
                                label={
                                    <div className="flex items-center gap-2">
                                        <div 
                                            className="w-5 h-5 rounded-md border border-gray-200"
                                            style={{ backgroundColor: color.name }}
                                        />
                                        <span>{color.name}</span>
                                    </div>
                                }
                                searchValue={color.name}
                            >
                                <div className="flex items-center gap-3 py-1">
                                    <div 
                                        className="w-8 h-8 rounded-lg shadow-sm border-2 border-white ring-1 ring-gray-200"
                                        style={{ backgroundColor: color.name }}
                                    />
                                    <div>
                                        <Text className="font-medium">{color.name}</Text>
                                        <br />
                                        <Text type="secondary" className="text-xs">ID: {color.id}</Text>
                                    </div>
                                </div>
                            </Select.Option>
                        ))}
                    </Select>

                    {selectedColorId && (
                        <div className="mt-4 p-4 bg-gradient-to-r from-gray-50 to-gray-100 rounded-xl border border-gray-200">
                            <Text type="secondary" className="text-xs uppercase tracking-wide">Preview</Text>
                            <div className="flex items-center gap-3 mt-2">
                                <div 
                                    className="w-12 h-12 rounded-xl shadow-lg border-2 border-white ring-2 ring-gray-200"
                                    style={{ backgroundColor: colors?.find(c => c.id === selectedColorId)?.name }}
                                />
                                <div>
                                    <Text strong className="text-base">
                                        {colors?.find(c => c.id === selectedColorId)?.name}
                                    </Text>
                                    <br />
                                    <Text type="secondary" className="text-xs">
                                        Will be added to: {product?.name}
                                    </Text>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
            </Modal>
        </div>
    );
};

export default ProductDetailPage;