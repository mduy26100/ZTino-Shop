import React, { useState, useCallback } from 'react';
import { Button, Typography, Space, message, Row, Col, Empty, Skeleton, Modal } from 'antd';
import { PlusIcon, ArrowPathIcon } from '@heroicons/react/24/outline';
import { ExclamationCircleFilled } from '@ant-design/icons';
import { 
    ColorCard, 
    useGetColors, 
    useCreateColor, 
    useUpdateColor, 
    useDeleteColor,
    UpsertColorModal 
} from '../../features/product';

const { Title, Text } = Typography;

const ColorPage = () => {
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modalContextHolder] = Modal.useModal();
    
    const { data = [], isLoading, refetch } = useGetColors();
    const { create, isCreating } = useCreateColor();
    const { update, isUpdating } = useUpdateColor();
    const { remove } = useDeleteColor();
    
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingRecord, setEditingRecord] = useState(null);

    const handleOpenCreate = useCallback(() => {
        setEditingRecord(null);
        setIsModalOpen(true);
    }, []);

    const handleOpenEdit = useCallback((record) => {
        setEditingRecord(record);
        setIsModalOpen(true);
    }, []);

    const handleCloseModal = useCallback(() => {
        setIsModalOpen(false);
        setEditingRecord(null);
    }, []);

    const handleDelete = useCallback((record) => {
        modal.confirm({
            title: 'Delete Color',
            icon: <ExclamationCircleFilled />,
            content: (
                <div className="pt-2">
                    <Text>Are you sure you want to delete color </Text>
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
                            content: `Deleted color: ${record.name}`,
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

    const handleSubmit = useCallback(async (values) => {
        const isEdit = !!editingRecord;
        const action = isEdit ? update : create;
        const payload = isEdit ? { ...values, id: editingRecord.id } : values;

        await action(payload, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: `Color ${isEdit ? 'updated' : 'created'} successfully`,
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
    }, [create, update, editingRecord, messageApi, handleCloseModal, refetch]);

    const renderContent = () => {
        if (isLoading) {
            return (
                <Row gutter={[16, 16]}>
                    {[...Array(12)].map((_, index) => (
                        <Col key={index} xs={24} sm={12} md={8} lg={6} xl={4}>
                            <div className="p-4 border border-gray-100 rounded-2xl bg-white space-y-4">
                                <Skeleton.Input active size="large" className="!w-16 !h-16 !rounded-full mx-auto block" />
                                <Skeleton active paragraph={{ rows: 1 }} title={{ width: '100%' }} />
                            </div>
                        </Col>
                    ))}
                </Row>
            );
        }

        if (!data || data.length === 0) {
            return (
                <div className="py-20 bg-white rounded-3xl border border-dashed border-gray-200 flex justify-center flex-col items-center gap-4">
                    <Empty description="No colors found" />
                    <Button type="primary" onClick={handleOpenCreate}>
                        Create First Color
                    </Button>
                </div>
            );
        }

        return (
            <Row gutter={[16, 16]}>
                {data.map((item) => (
                    <Col key={item.id} xs={24} sm={12} md={8} lg={6} xl={4}>
                        <ColorCard 
                            item={item} 
                            onEdit={handleOpenEdit} 
                            onDelete={handleDelete} 
                        />
                    </Col>
                ))}
            </Row>
        );
    };

    return (
        <div className="animate-fade-in space-y-6">
            {contextHolder}
            {modalContextHolder}
            
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div>
                    <Title level={2} className="!mb-1">Colors</Title>
                    <Text type="secondary">Manage product color variations</Text>
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
                            Add Color
                        </Button>
                    </Space>
                </div>
            </div>

            <div className="min-h-[400px]">
                {renderContent()}
            </div>

            <UpsertColorModal 
                open={isModalOpen}
                onCancel={handleCloseModal}
                onSubmit={handleSubmit}
                confirmLoading={editingRecord ? isUpdating : isCreating}
                initialValues={editingRecord}
            />
        </div>
    );
};

export default ColorPage;