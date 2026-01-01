import React, { useEffect, useMemo } from 'react';
import { Modal, Form, Input, Checkbox, Select, Button, Upload } from 'antd';
import { PlusOutlined } from '@ant-design/icons';

const normFile = (e) => {
    if (Array.isArray(e)) {
        return e;
    }
    return e?.fileList;
};

const UpsertCategoryModal = ({ 
    open, 
    onCancel, 
    onSubmit, 
    confirmLoading, 
    categoryList = [],
    initialValues = null 
}) => {
    const [form] = Form.useForm();
    
    const parentIdValue = Form.useWatch('parentId', form);
    
    const isRootCategory = !parentIdValue;

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
            form.resetFields(); 
            if (initialValues) {
                form.setFieldsValue({
                    ...initialValues,
                    parentId: initialValues.parentId || null 
                });
            }
        }
    }, [open, initialValues, form]);

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            
            const payload = {
                name: values.name,
                slug: values.slug,
                isActive: values.isActive,
                
                parentId: values.parentId || null,

                image: isRootCategory && values.image?.[0]?.originFileObj 
                    ? values.image[0].originFileObj 
                    : null
            };

            onSubmit(payload, form);
        } catch (error) {
            console.error('Validate Failed:', error);
        }
    };

    const uploadProps = {
        beforeUpload: (file) => {
            const isImage = file.type.startsWith('image/');
            if (!isImage) {
                return Upload.LIST_IGNORE;
            }
            return false;
        },
        maxCount: 1,
        listType: "picture-card",
        showUploadList: { showPreviewIcon: false },
        accept: "image/*"
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
            maskClosable={false}
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
                        tooltip="Select a parent to make this a sub-category. Leave empty for Root category."
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

                {isRootCategory && (
                    <Form.Item
                        name="image"
                        label={<span className="font-medium text-gray-700">Cover Image</span>}
                        valuePropName="fileList"
                        getValueFromEvent={normFile}
                        tooltip="Only root categories can have a cover image"
                    >
                        <Upload {...uploadProps}>
                            <div>
                                <PlusOutlined />
                                <div style={{ marginTop: 8 }}>Upload</div>
                            </div>
                        </Upload>
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