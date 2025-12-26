import React, { useEffect } from 'react';
import { Modal, Form, Input, Button, ColorPicker, Row, Col } from 'antd';

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

    const handleColorChange = (color) => {
        form.setFieldValue('name', color.toHexString());
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
                initialValues={{ name: '#1677ff' }}
            >
                <Form.Item label={<span className="font-medium text-gray-700">Color Name or Hex</span>}>
                    <Row gutter={8}>
                        <Col flex="auto">
                            <Form.Item
                                name="name"
                                noStyle
                                rules={[{ required: true, message: 'Please enter color name or pick a color' }]}
                            >
                                <Input 
                                    placeholder="e.g. Red, Blue, #1677ff" 
                                    className="rounded-lg py-2" 
                                />
                            </Form.Item>
                        </Col>
                        <Col flex="none">
                            <Form.Item
                                name="name"
                                noStyle
                                getValueFromEvent={(color) => color.toHexString()}
                            >
                                <ColorPicker onChange={handleColorChange} />
                            </Form.Item>
                        </Col>
                    </Row>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default React.memo(UpsertColorModal);