import React, { useEffect } from 'react';
import { Modal, Form, Input, Button } from 'antd';

const UpsertSizeModal = ({ 
    open, 
    onCancel, 
    onSubmit, 
    confirmLoading, 
    initialValues = null 
}) => {
    const [form] = Form.useForm();
    const isEdit = !!initialValues;

    useEffect(() => {
        if (open) {
            if (initialValues) {
                form.setFieldsValue(initialValues);
            } else {
                form.resetFields();
            }
        }
    }, [open, initialValues, form]);

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            onSubmit(values);
        } catch (error) {
            console.error('Validate Failed:', error);
        }
    };

    return (
        <Modal
            title={
                <span className="text-xl font-semibold text-slate-700">
                    {isEdit ? "Update Size" : "Create New Size"}
                </span>
            }
            open={open}
            onCancel={onCancel}
            width={450}
            footer={[
                <Button key="back" onClick={onCancel} className="h-9 px-4 rounded-lg">
                    Cancel
                </Button>,
                <Button 
                    key="submit" 
                    type="primary" 
                    loading={confirmLoading} 
                    onClick={handleSubmit}
                    className="bg-indigo-600 hover:!bg-indigo-700 border-none h-9 px-6 rounded-lg shadow-sm"
                >
                    {isEdit ? "Update" : "Create"}
                </Button>,
            ]}
            destroyOnHidden
            centered
            maskClosable={!confirmLoading}
            styles={{ 
                mask: { backdropFilter: 'blur(4px)' },
                content: { padding: '24px', borderRadius: '16px' }
            }}
        >
            <Form
                form={form}
                layout="vertical"
                className="pt-4"
                initialValues={{ name: '' }}
            >
                <Form.Item
                    name="name"
                    label={<span className="font-medium text-gray-700">Size Name</span>}
                    rules={[
                        { required: true, message: 'Please enter size name (e.g., S, M, XL)' },
                        { max: 10, message: 'Size name is too long' }
                    ]}
                >
                    <Input 
                        placeholder="e.g. S, M, L, XL, 42, 43..." 
                        className="rounded-lg py-2" 
                        maxLength={10}
                        showCount
                    />
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default React.memo(UpsertSizeModal);