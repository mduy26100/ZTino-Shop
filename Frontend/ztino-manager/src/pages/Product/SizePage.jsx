import React, { useState, useCallback } from 'react';
import { 
    Button, Typography, message, Row, Col, 
    Empty, Skeleton, Modal 
} from 'antd';
import { 
    PlusIcon, 
    ArrowPathIcon, 
    ExclamationCircleIcon 
} from '@heroicons/react/24/outline';

import { 
    SizeCard, 
    useGetSizes, 
    useCreateSize, 
    useUpdateSize, 
    useDeleteSize,
    UpsertSizeModal 
} from '../../features/product';

const { Title, Text } = Typography;

const SizePage = () => {
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modalContextHolder] = Modal.useModal();

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingRecord, setEditingRecord] = useState(null);

    const { data, isLoading, refetch } = useGetSizes({
        onError: (err) => {
            messageApi.open({
                type: 'error',
                content: err?.message || 'Failed to load sizes.',
            });
        }
    });

    const { create, isCreating } = useCreateSize();
    const { update, isUpdating } = useUpdateSize();
    const { remove } = useDeleteSize();

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

    const handleSubmit = useCallback(async (values) => {
        const isEdit = !!editingRecord;
        
        const action = isEdit ? update : create;
        const payload = isEdit ? { ...values, id: editingRecord.id } : values;

        await action(payload, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: `Size ${isEdit ? 'updated' : 'created'} successfully`,
                });
                handleCloseModal();
                refetch();
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.error?.message || error?.message || 'Operation failed',
                });
            }
        });
    }, [create, update, editingRecord, handleCloseModal, refetch, messageApi]);

    const handleDelete = useCallback((record) => {
        modal.confirm({
            title: 'Delete Size',
            icon: <ExclamationCircleIcon className="w-6 h-6 text-red-500 mr-2" />,
            content: (
                <div className="pt-2">
                    <Text>Are you sure you want to delete size </Text>
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
                await remove(record.id, {
                    onSuccess: () => {
                        messageApi.open({
                            type: 'success',
                            content: `Deleted size: ${record.name}`,
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
    }, [modal, remove, messageApi, refetch]);

    const handleRefresh = useCallback(() => {
        refetch({
            onSuccess: () => messageApi.success('Data refreshed'),
        });
    }, [refetch, messageApi]);

    const renderContent = () => {
        if (isLoading) {
            return (
                <Row gutter={[24, 24]}>
                    {[...Array(8)].map((_, index) => (
                        <Col key={index} xs={24} sm={12} md={8} lg={6} xl={4}>
                            <div className="p-6 border border-gray-100 rounded-2xl bg-white flex flex-col items-center gap-4">
                                <Skeleton.Input active size="large" className="!w-16 !h-16 !rounded-2xl" />
                                <Skeleton active paragraph={false} title={{ width: 60 }} />
                            </div>
                        </Col>
                    ))}
                </Row>
            );
        }

        if (!data || data.length === 0) {
            return (
                <div className="py-20 bg-white rounded-3xl border border-dashed border-gray-200 flex flex-col items-center justify-center gap-4 animate-fade-in">
                    <Empty 
                        image={Empty.PRESENTED_IMAGE_SIMPLE} 
                        description={<Text type="secondary">No sizes found</Text>} 
                    />
                    <Button type="primary" onClick={handleOpenCreate}>
                        Create First Size
                    </Button>
                </div>
            );
        }

        return (
            <Row gutter={[24, 24]}>
                {data.map((item) => (
                    <Col key={item.id} xs={24} sm={12} md={8} lg={6} xl={4}>
                        <SizeCard 
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
                    <Title level={2} className="!mb-1 text-slate-800">Sizes</Title>
                    <Text type="secondary" className="text-slate-500">
                        Manage product size variations (S, M, L, XL...)
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
                        Add Size
                    </Button>
                </div>
            </div>

            <div className="min-h-[400px]">
                {renderContent()}
            </div>

            <UpsertSizeModal 
                open={isModalOpen}
                onCancel={handleCloseModal}
                onSubmit={handleSubmit}
                confirmLoading={editingRecord ? isUpdating : isCreating}
                initialValues={editingRecord}
            />
        </div>
    );
};

export default SizePage;