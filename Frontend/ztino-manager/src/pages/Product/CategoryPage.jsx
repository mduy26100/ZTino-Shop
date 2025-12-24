import React, { useState } from 'react';
import { Button, Card, Typography, Space, message } from 'antd';
import { PlusIcon, ArrowPathIcon } from '@heroicons/react/24/outline';
import { 
    UpsertCategoryModal,
    CategoryTable, 
    useCreateCategory, 
    useUpdateCategory,
    useGetCategories 
} from '../../features/product';

const { Title, Text } = Typography;

const CategoryPage = () => {
    const [messageApi, contextHolder] = message.useMessage();
    const { data, isLoading, refetch } = useGetCategories();
    
    const { create, isLoading: isCreating } = useCreateCategory();
    const { update, isUpdating } = useUpdateCategory();
    
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingRecord, setEditingRecord] = useState(null);

    const handleOpenCreate = () => {
        setEditingRecord(null);
        setIsModalOpen(true);
    };

    const handleOpenEdit = (record) => {
        setEditingRecord(record);
        setIsModalOpen(true);
    };

    const handleCloseModal = () => {
        setIsModalOpen(false);
        setEditingRecord(null);
    };

    const handleSubmit = async (values) => {
        const isEdit = !!editingRecord;
        const action = isEdit ? update : create;
        const payload = isEdit ? { ...values, id: editingRecord.id } : values;

        await action(payload, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: `Category ${isEdit ? 'updated' : 'created'} successfully`,
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
    };

    return (
        <div className="animate-fade-in space-y-6">
            {contextHolder}
            
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                <div>
                    <Title level={2} className="!mb-1">Categories</Title>
                    <Text type="secondary">Organize and manage your product hierarchy</Text>
                </div>
                <div className="flex justify-end">
                    <Space size="small">
                        <Button 
                            icon={<ArrowPathIcon className={`w-4 h-4 ${isLoading ? 'animate-spin' : ''}`} />} 
                            onClick={() => refetch()}
                            className="flex items-center rounded-xl h-10"
                        >
                            Refresh
                        </Button>
                        <Button 
                            type="primary" 
                            icon={<PlusIcon className="w-4 h-4" />} 
                            className="bg-indigo-600 hover:!bg-indigo-700 border-none h-10 rounded-xl flex items-center shadow-lg shadow-indigo-100"
                            onClick={handleOpenCreate}
                        >
                            Add Category
                        </Button>
                    </Space>
                </div>
            </div>

            <Card className="shadow-sm border-gray-100 rounded-2xl overflow-hidden">
                <CategoryTable 
                    data={data} 
                    loading={isLoading} 
                    onEdit={handleOpenEdit}
                    onDelete={(record) => console.log('Delete logic here')}
                />
            </Card>

            <UpsertCategoryModal
                open={isModalOpen}
                onCancel={handleCloseModal}
                onSubmit={handleSubmit}
                confirmLoading={editingRecord ? isUpdating : isCreating}
                categoryList={data}
                initialValues={editingRecord}
            />
        </div>
    );
};

export default CategoryPage;