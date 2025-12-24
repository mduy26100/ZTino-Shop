import React, { useEffect } from 'react';
import { Modal, Form, Input, Button } from 'antd';

const UpsertColorModal = ({ 
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
            onSubmit(values, form);
        } catch (error) {
            console.error('Validate Failed:', error);
        }
    };

    return (
        <Modal
            title={isEdit ? "Update Color" : "Create New Color"}
            open={open}
            onCancel={onCancel}
            width={450}
            footer={[
                <Button key="back" onClick={onCancel}>
                    Cancel
                </Button>,
                <Button 
                    key="submit" 
                    type="primary" 
                    loading={confirmLoading} 
                    onClick={handleSubmit}
                    className="bg-indigo-600 hover:!bg-indigo-700 border-none rounded-lg"
                >
                    {isEdit ? "Update" : "Create"}
                </Button>,
            ]}
            destroyOnHidden
            centered
            styles={{ 
                mask: { backdropFilter: 'blur(4px)' },
                content: { padding: '24px', borderRadius: '16px' }
            }}
        >
            <Form
                form={form}
                layout="vertical"
                className="pt-4"
            >
                <Form.Item
                    name="name"
                    label={<span className="font-medium text-gray-700">Color Name</span>}
                    rules={[{ required: true, message: 'Please enter color name' }]}
                >
                    <Input 
                        placeholder="e.g. Red, Blue,.." 
                        className="rounded-lg py-2" 
                    />
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default React.memo(UpsertColorModal);