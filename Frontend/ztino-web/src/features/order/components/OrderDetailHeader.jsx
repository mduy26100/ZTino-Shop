import React, { memo, useMemo, useCallback, useState } from 'react';
import { Typography, Button, Tooltip, message, Card, Divider } from 'antd';
import { ArrowLeftOutlined, CopyOutlined, ReloadOutlined, CalendarOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import OrderStatusTag from './OrderStatusTag';
import OrderPaymentStatusTag from './OrderPaymentStatusTag';
import UpdateStatusButton from './UpdateStatusButton';
import UpdateStatusModal from './UpdateStatusModal';

const { Title, Text } = Typography;

const formatDate = (dateString) => {
    return dayjs(dateString).format('DD/MM/YYYY HH:mm');
};

const StatusItem = memo(({ label, children }) => (
    <div className="flex flex-col gap-1">
        <Text className="text-gray-400 text-xs uppercase tracking-wide font-medium">
            {label}
        </Text>
        {children}
    </div>
));

StatusItem.displayName = 'StatusItem';

const OrderDetailHeader = memo(({ order, onRefresh, onUpdateStatus, isUpdating }) => {
    const navigate = useNavigate();
    const { orderCode, status, paymentStatus, createdAt, id } = order || {};
    
    const [modalOpen, setModalOpen] = useState(false);
    const [selectedStatus, setSelectedStatus] = useState(null);

    const handleBack = useCallback(() => {
        navigate('/orders');
    }, [navigate]);

    const handleCopyOrderCode = useCallback(() => {
        if (orderCode) {
            navigator.clipboard.writeText(orderCode);
            message.success('Order code copied');
        }
    }, [orderCode]);

    const formattedDate = useMemo(() => {
        return createdAt ? formatDate(createdAt) : '';
    }, [createdAt]);

    const handleStatusSelect = useCallback((newStatus) => {
        setSelectedStatus(newStatus);
        setModalOpen(true);
    }, []);

    const handleModalCancel = useCallback(() => {
        setModalOpen(false);
        setSelectedStatus(null);
    }, []);

    const handleConfirmUpdate = useCallback((formData) => {
        if (!selectedStatus || !id) return;

        const payload = {
            orderId: id,
            newStatus: selectedStatus,
            ...formData
        };

        onUpdateStatus(payload, {
            onSuccess: () => {
                setModalOpen(false);
                setSelectedStatus(null);
            }
        });
    }, [selectedStatus, id, onUpdateStatus]);

    return (
        <>
            <Card className="shadow-sm rounded-xl mb-6">
                <div className="flex flex-col gap-4">
                    <div className="flex items-center justify-between">
                        <div className="flex items-center gap-3">
                            <Button 
                                type="text" 
                                icon={<ArrowLeftOutlined />}
                                onClick={handleBack}
                                className="p-0 text-gray-500 hover:text-indigo-600"
                            />
                            <div className="flex items-center gap-2 flex-wrap">
                                <Title level={4} className="!mb-0 font-mono text-indigo-600">
                                    {orderCode}
                                </Title>
                                <Tooltip title="Copy order code">
                                    <Button 
                                        type="text" 
                                        size="small"
                                        icon={<CopyOutlined />}
                                        onClick={handleCopyOrderCode}
                                        className="text-gray-400 hover:text-indigo-600"
                                    />
                                </Tooltip>
                            </div>
                        </div>
                        <div className="flex items-center gap-2">
                            <UpdateStatusButton 
                                currentStatus={status}
                                onStatusSelect={handleStatusSelect}
                                disabled={isUpdating}
                            />
                            <Button
                                icon={<ReloadOutlined />}
                                onClick={onRefresh}
                            >
                                Refresh
                            </Button>
                        </div>
                    </div>

                    <Divider className="!my-2" />

                    <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 lg:gap-8">
                        <StatusItem label="Order Status">
                            <OrderStatusTag status={status} />
                        </StatusItem>

                        <StatusItem label="Payment Status">
                            <OrderPaymentStatusTag status={paymentStatus} />
                        </StatusItem>

                        <StatusItem label="Order Date">
                            <div className="flex items-center gap-2">
                                <CalendarOutlined className="text-gray-400" />
                                <Text className="text-gray-700 font-medium">
                                    {formattedDate}
                                </Text>
                            </div>
                        </StatusItem>
                    </div>
                </div>
            </Card>

            <UpdateStatusModal
                open={modalOpen}
                onCancel={handleModalCancel}
                onConfirm={handleConfirmUpdate}
                currentStatus={status}
                newStatus={selectedStatus}
                isLoading={isUpdating}
            />
        </>
    );
});

OrderDetailHeader.displayName = 'OrderDetailHeader';

export default OrderDetailHeader;
