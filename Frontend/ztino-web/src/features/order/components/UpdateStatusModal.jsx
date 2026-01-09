import React, { memo, useMemo, useCallback } from 'react';
import { Modal, Form, Input, Typography, Tag } from 'antd';
import { ExclamationCircleOutlined, EditOutlined } from '@ant-design/icons';
import { ORDER_STATUS, STATUS_COLOR } from '../constants';

const { Text } = Typography;
const { TextArea } = Input;

const UpdateStatusModal = memo(({ 
    open, 
    onCancel, 
    onConfirm, 
    currentStatus, 
    newStatus, 
    isLoading 
}) => {
    const [form] = Form.useForm();

    const isCancelling = useMemo(() => {
        return newStatus === ORDER_STATUS.CANCELLED;
    }, [newStatus]);

    const isReturning = useMemo(() => {
        return newStatus === ORDER_STATUS.RETURNED;
    }, [newStatus]);

    const requiresReason = useMemo(() => {
        return isCancelling || isReturning;
    }, [isCancelling, isReturning]);

    const modalTitle = useMemo(() => {
        if (isCancelling) {
            return 'Cancel Order';
        }
        if (isReturning) {
            return 'Return Order';
        }
        return 'Update Order Status';
    }, [isCancelling, isReturning]);

    const confirmMessage = useMemo(() => {
        const statusMessages = {
            Confirmed: 'Are you sure you want to confirm this order?',
            Shipping: 'Are you sure you want to mark this order as shipping?',
            Delivered: 'Are you sure you want to mark this order as delivered?',
            Cancelled: 'Are you sure you want to cancel this order? This action cannot be undone.',
            Returned: 'Are you sure you want to mark this order as returned?'
        };
        return statusMessages[newStatus] || `Are you sure you want to change status to ${newStatus}?`;
    }, [newStatus]);

    const handleOk = useCallback(() => {
        form.validateFields()
            .then((values) => {
                const payload = {};
                if (isCancelling) {
                    payload.cancelReason = values.reason;
                } else if (isReturning) {
                    payload.note = values.reason;
                } else {
                    payload.note = values.reason || undefined;
                }
                
                onConfirm(payload);
                form.resetFields();
            })
            .catch(() => {
            });
    }, [form, isCancelling, isReturning, onConfirm]);

    const handleCancel = useCallback(() => {
        form.resetFields();
        onCancel();
    }, [form, onCancel]);

    return (
        <Modal
            title={
                <div className="flex items-center gap-2">
                    {isCancelling ? (
                        <ExclamationCircleOutlined className="text-red-500" />
                    ) : (
                        <EditOutlined className="text-blue-500" />
                    )}
                    <span>{modalTitle}</span>
                </div>
            }
            open={open}
            onOk={handleOk}
            onCancel={handleCancel}
            confirmLoading={isLoading}
            okText={isCancelling ? 'Cancel Order' : isReturning ? 'Return Order' : 'Confirm'}
            cancelText="Close"
            okButtonProps={{
                className:
                    'bg-indigo-600 hover:!bg-indigo-700 border-none rounded-lg',
                danger: false
            }}
            destroyOnClose
        >
            <div className="py-4">
                <div className="flex items-center gap-2 mb-4 p-3 bg-gray-50 rounded-lg">
                    <Text className="text-gray-500">Status change:</Text>
                    <Tag color={STATUS_COLOR[currentStatus]}>{currentStatus}</Tag>
                    <span className="text-gray-400">→</span>
                    <Tag color={STATUS_COLOR[newStatus]}>{newStatus}</Tag>
                </div>

                <Text className="block mb-4 text-gray-600">
                    {confirmMessage}
                </Text>

                <Form form={form} layout="vertical">
                    <Form.Item
                        name="reason"
                        label={isCancelling ? 'Cancel Reason' : isReturning ? 'Return Reason' : 'Note (Optional)'}
                        rules={[
                            {
                                required: requiresReason,
                                message: isCancelling 
                                    ? 'Please enter the cancel reason' 
                                    : 'Please enter the return reason'
                            },
                            {
                                max: 500,
                                message: 'Maximum 500 characters allowed'
                            }
                        ]}
                    >
                        <TextArea
                            rows={3}
                            placeholder={
                                isCancelling 
                                    ? 'Enter the reason for cancellation...' 
                                    : isReturning
                                        ? 'Enter the reason for return...'
                                        : 'Enter optional note...'
                            }
                            maxLength={500}
                            showCount
                        />
                    </Form.Item>
                </Form>
            </div>
        </Modal>
    );
});

UpdateStatusModal.displayName = 'UpdateStatusModal';

export default UpdateStatusModal;
