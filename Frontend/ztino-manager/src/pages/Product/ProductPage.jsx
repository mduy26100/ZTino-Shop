import React, { useState, useCallback } from 'react';
import { Button, Typography, message, Modal } from 'antd';
import { 
    PlusIcon, 
    ArrowPathIcon
} from '@heroicons/react/24/outline';
import { ExclamationCircleFilled } from "@ant-design/icons";

import { 
    ProductTable, 
    useGetProducts,
    useCreateProduct,
    useUpdateProduct,
    useDeleteProduct,
    useGetCategories,
    UpsertProductModal,
} from '../../features/product';
import { useNavigate } from 'react-router-dom';

const { Title, Text } = Typography;

const ProductPage = () => {
    const navigate = useNavigate();

    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modalContextHolder] = Modal.useModal();

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingRecord, setEditingRecord] = useState(null);

    const { data, isLoading, refetch } = useGetProducts({
        onError: (err) => messageApi.error(err?.message || 'Failed to fetch products')
    });

    const handleViewDetail = useCallback((record) => {
        navigate(`/products/${record.id}`);
    }, [navigate]);

    const { 
        data: categoriesData, 
        isLoading: isLoadingCategories 
    } = useGetCategories(); 

    const { create, isCreating } = useCreateProduct();
    const { update, isUpdating } = useUpdateProduct();
    const { remove } = useDeleteProduct();

    const handleOpenCreate = useCallback(() => {
        setEditingRecord(null);
        setIsModalOpen(true);
    }, []);

    const handleCloseModal = useCallback(() => {
        setIsModalOpen(false);
        setEditingRecord(null);
    }, []);

    const handleSubmit = useCallback(async (values) => {
        const isEdit = !!editingRecord;

        const formValues = {
            CategoryId: values.CategoryId,
            Name: values.Name,
            Slug: values.Slug,
            BasePrice: values.BasePrice,
            Description: values.Description || '',
            IsActive: values.IsActive,
            MainImageUrl: values.MainImageUrl?.[0]?.originFileObj || null 
        };

        const action = isEdit ? update : create;
        const payload = isEdit ? { ...formValues, Id: editingRecord.id } : formValues;

        await action(payload, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: `Product ${isEdit ? 'updated' : 'created'} successfully`,
                });
                handleCloseModal();
                refetch();
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.Error?.Message || error?.message || 'Operation failed',
                });
            }
        });

    }, [create, update, editingRecord, handleCloseModal, refetch, messageApi]);

    const handleRefresh = useCallback(() => {
        refetch({
            onSuccess: () => messageApi.success('Product list refreshed'),
        });
    }, [refetch, messageApi]);

    const handleEdit = useCallback((record) => {
        setEditingRecord({
            ...record,
            CategoryId: record.categoryId,
            Name: record.name,
            Slug: record.slug,
            BasePrice: record.basePrice,
            Description: record.description,
            IsActive: record.isActive,
            mainImageUrl: record.mainImageUrl 
        });
        setIsModalOpen(true);
    }, []);

    const handleDelete = useCallback((record) => {
        modal.confirm({
            title: 'Delete Product',
            icon: <ExclamationCircleFilled className="text-red-500" />,
            content: (
                <div className="pt-2">
                    <Text>Are you sure you want to delete product </Text>
                    <Text strong>"{record.name}"</Text>?
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
                await remove(record.id, {
                    onSuccess: () => {
                        messageApi.open({
                            type: 'success',
                            content: `Deleted product: ${record.name}`,
                        });
                        refetch();
                    },
                    onError: (error) => {
                        messageApi.open({
                            type: 'error',
                            content: error?.Error?.Message || error?.message || 'Delete failed',
                        });
                    }
                });
            },
        });
    }, [modal, remove, messageApi, refetch]);

    return (
        <div className="animate-fade-in space-y-6">
            {contextHolder}
            {modalContextHolder}

            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <Title level={2} className="!mb-1 text-slate-800">Products</Title>
                    <Text type="secondary" className="text-slate-500">
                        Manage your product catalog and prices
                    </Text>
                </div>

                <div className="flex items-center gap-3">
                    <Button
                        icon={<ArrowPathIcon className={`w-4 h-4 ${isLoading ? 'animate-spin' : ''}`} />}
                        onClick={handleRefresh}
                        className="flex items-center rounded-xl h-10 border-gray-200 text-slate-600 hover:text-indigo-600 hover:border-indigo-100"
                    >
                        Refresh
                    </Button>
                    <Button
                        type="primary"
                        icon={<PlusIcon className="w-5 h-5 stroke-2" />}
                        className="bg-indigo-600 hover:!bg-indigo-700 border-none h-10 rounded-xl flex items-center shadow-lg shadow-indigo-100 px-6"
                        onClick={handleOpenCreate}
                    >
                        Add Product
                    </Button>
                </div>
            </div>

            <div className="min-h-[400px]">
                <ProductTable 
                    dataSource={data}
                    isLoading={isLoading}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                    onViewDetail={handleViewDetail}
                />
            </div>

            <UpsertProductModal 
                open={isModalOpen}
                onCancel={handleCloseModal}
                onSubmit={handleSubmit}
                confirmLoading={isCreating || isUpdating}
                initialValues={editingRecord}
                categories={categoriesData}
                isLoadingCategories={isLoadingCategories}
            />
        </div>
    );
};

export default ProductPage;