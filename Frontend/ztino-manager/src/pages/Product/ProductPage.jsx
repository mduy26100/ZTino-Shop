import React, { useState, useCallback } from 'react';
import { Button, Typography, message, Modal } from 'antd';
import { 
    PlusIcon, 
    ArrowPathIcon
} from '@heroicons/react/24/outline';
import { ExclamationCircleFilled } from "@ant-design/icons";

import { ProductTable, useGetProducts } from '../../features/product';

const { Title, Text } = Typography;

const ProductPage = () => {
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modalContextHolder] = Modal.useModal();

    const { data, isLoading, refetch } = useGetProducts();

    const handleRefresh = useCallback(() => {
        refetch({
            onSuccess: () => messageApi.success('Product list refreshed'),
        });
    }, [refetch, messageApi]);

    const handleCreate = useCallback(() => {
        messageApi.info('Create feature coming soon');
    }, [messageApi]);

    const handleEdit = useCallback((record) => {
        messageApi.info(`Editing product: ${record.name}`);
    }, [messageApi]);

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
            onOk: async () => {
                messageApi.success(`Deleted product: ${record.name}`);
                refetch();
            },
        });
    }, [modal, messageApi, refetch]);

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
                        onClick={handleCreate}
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
                />
            </div>
            
        </div>
    );
};

export default ProductPage;