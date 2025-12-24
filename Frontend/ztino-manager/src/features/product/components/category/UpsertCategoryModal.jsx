import React, { useEffect, useMemo } from 'react';
import { Modal, Form, Input, Checkbox, Select, Button } from 'antd';

const UpsertCategoryModal = ({ 
    open, 
    onCancel, 
    onSubmit, 
    confirmLoading, 
    categoryList = [],
    initialValues = null 
}) => {
    const [form] = Form.useForm();
    const isEdit = !!initialValues;

    const shouldShowParentSelect = !isEdit || (isEdit && initialValues?.parentId !== null);

    const parentOptions = useMemo(() => {
        return categoryList
            .filter(category => category.parentId === null && category.id !== initialValues?.id)
            .map(category => ({
                label: category.name,
                value: category.id
            }));
    }, [categoryList, initialValues]);

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
            title={isEdit ? "Update Category" : "Create New Category"}
            open={open}
            onCancel={onCancel}
            width={500}
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
            className="top-4 md:top-0"
            styles={{ 
                mask: { backdropFilter: 'blur(4px)' },
                content: { padding: '24px', borderRadius: '16px' }
            }}
        >
            <Form
                form={form}
                layout="vertical"
                initialValues={{ 
                    isActive: true,
                    parentId: null 
                }}
                className="pt-2"
            >
                <Form.Item
                    name="name"
                    label={<span className="font-medium text-gray-700">Category Name</span>}
                    rules={[{ required: true, message: 'Please enter category name' }]}
                >
                    <Input placeholder="e.g. Men's Clothing" className="rounded-lg py-2" />
                </Form.Item>

                <Form.Item
                    name="slug"
                    label={<span className="font-medium text-gray-700">Slug</span>}
                    rules={[
                        { required: true, message: 'Please enter slug' },
                        { 
                            pattern: /^[a-z0-9-]+$/, 
                            message: 'Slug can only contain lowercase letters, numbers, and hyphens.' 
                        }
                    ]}
                >
                    <Input placeholder="e.g. mens-clothing" className="rounded-lg py-2" />
                </Form.Item>

                {shouldShowParentSelect && (
                    <Form.Item 
                        name="parentId" 
                        label={<span className="font-medium text-gray-700">Parent Category</span>}
                    >
                        <Select
                            placeholder="Select parent category (Root)"
                            allowClear
                            options={parentOptions}
                            className="rounded-lg h-10"
                            showSearch
                            optionFilterProp="label"
                            popupMatchSelectWidth={false}
                        />
                    </Form.Item>
                )}

                <Form.Item name="isActive" valuePropName="checked" className="mb-0">
                    <Checkbox className="text-gray-600 font-medium">Active</Checkbox>
                </Form.Item>
            </Form>
        </Modal>
    );
};

export default React.memo(UpsertCategoryModal);