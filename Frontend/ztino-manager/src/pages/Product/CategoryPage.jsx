import React from 'react';
import { Button, Card, Typography, Space, message } from 'antd';
import { PlusIcon, ArrowPathIcon } from '@heroicons/react/24/outline';
import { CategoryTable, useGetCategories } from '../../features/product';

const { Title, Text } = Typography;

const CategoryPage = () => {
    const { data, isLoading, refetch } = useGetCategories();

    const handleEdit = (record) => {
        console.log("Open Edit Modal for:", record);
    };

    const handleDelete = (record) => {
        console.log("Call Delete API for:", record.id);
        message.info(`Deleting ${record.name}...`);
    };

    const handleAdd = () => {
        console.log("Open Add Modal");
    };

    return (
        <div className="animate-fade-in space-y-6">
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
                <div>
                    <Title level={2} className="!mb-1">Categories</Title>
                    <Text type="secondary">Organize and manage your product hierarchy</Text>
                </div>
                <Space>
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
                        onClick={handleAdd}
                    >
                        Add Category
                    </Button>
                </Space>
            </div>

            <Card className="shadow-sm border-gray-100 rounded-2xl overflow-hidden">
                <CategoryTable
                    data={data} 
                    loading={isLoading} 
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                />
            </Card>
        </div>
    );
};

export default CategoryPage;