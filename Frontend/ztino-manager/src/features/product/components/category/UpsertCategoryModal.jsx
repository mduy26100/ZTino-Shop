import React, { useEffect, useMemo, useState } from 'react';
import { Modal, Form, Input, Checkbox, Select, Button, Upload, message, Image } from 'antd';
import { PlusOutlined } from '@ant-design/icons';

const getBase64 = (file) =>
    new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = (error) => reject(error);
    });

const normFile = (e) => {
    if (Array.isArray(e)) return e;
    return e?.fileList;
};

const beforeUpload = (file) => {
    const isImage = file.type.startsWith('image/');
    if (!isImage) {
        message.error('You can only upload image files!');
        return Upload.LIST_IGNORE;
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
        message.error('Image must be smaller than 2MB!');
        return Upload.LIST_IGNORE;
    }
    return false;
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
    const isEdit = !!initialValues;

    const [previewOpen, setPreviewOpen] = useState(false);
    const [previewImage, setPreviewImage] = useState('');

    const parentIdValue = Form.useWatch('parentId', form);
    const isRootCategory = !parentIdValue;
    const shouldShowParentSelect = !isEdit || (isEdit && initialValues?.parentId !== null);

    const parentOptions = useMemo(() => {
        return categoryList
            .filter(cat => cat.parentId === null && cat.id !== initialValues?.id)
            .map(cat => ({ label: cat.name, value: cat.id }));
    }, [categoryList, initialValues]);

    useEffect(() => {
        if (open) {
            if (initialValues) {
                let fileList = [];
                if (initialValues.imageUrl) {
                    fileList = [{
                        uid: '-1',
                        name: 'current-image',
                        status: 'done',
                        url: initialValues.imageUrl,
                    }];
                }

                form.setFieldsValue({
                    ...initialValues,
                    parentId: initialValues.parentId || null,
                    image: fileList 
                });
            } else {
                form.resetFields();
                form.setFieldsValue({ isActive: true, parentId: null });
            }
        }
    }, [open, initialValues, form]);

    const handlePreview = async (file) => {
        if (!file.url && !file.preview) {
            file.preview = await getBase64(file.originFileObj);
        }
        setPreviewImage(file.url || file.preview);
        setPreviewOpen(true);
    };

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            
            let imageToSend = null;
            if (isRootCategory && values.image && values.image.length > 0) {
                if (values.image[0].originFileObj) {
                    imageToSend = values.image[0].originFileObj;
                }
            }

            const payload = {
                id: initialValues?.id,
                name: values.name,
                slug: values.slug,
                isActive: values.isActive,
                parentId: values.parentId || null,
                image: imageToSend 
            };

            onSubmit(payload, form);
        } catch (error) {
            console.error('Validate Failed:', error);
        }
    };

    return (
        <>
            <Modal
                title={isEdit ? "Update Category" : "Create New Category"}
                open={open}
                onCancel={onCancel}
                width={500}
                footer={[
                    <Button key="back" onClick={onCancel}>Cancel</Button>,
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
                maskClosable={true}
                afterClose={() => {
                    form.resetFields();
                    setPreviewOpen(false);
                    setPreviewImage('');
                }}
                styles={{ 
                    mask: { backdropFilter: 'blur(4px)' },
                    content: { padding: '24px', borderRadius: '16px' }
                }}
                zIndex={1000} 
            >
                <Form
                    form={form}
                    layout="vertical"
                    initialValues={{ isActive: true, parentId: null }}
                    className="pt-2"
                >
                    <Form.Item
                        name="name"
                        label="Category Name"
                        rules={[{ required: true, message: 'Please enter category name' }]}
                    >
                        <Input placeholder="e.g. Men's Clothing" className="rounded-lg py-2" />
                    </Form.Item>

                    <Form.Item
                        name="slug"
                        label="Slug"
                        rules={[{ required: true, message: 'Please enter slug' }]}
                    >
                        <Input placeholder="e.g. mens-clothing" className="rounded-lg py-2" />
                    </Form.Item>

                    {shouldShowParentSelect && (
                        <Form.Item 
                            name="parentId" 
                            label="Parent Category"
                            tooltip="Select a parent to make this a sub-category"
                        >
                            <Select
                                placeholder="Select parent category (Root)"
                                allowClear
                                options={parentOptions}
                                className="rounded-lg h-10"
                                showSearch
                                optionFilterProp="label"
                            />
                        </Form.Item>
                    )}

                    {isRootCategory && (
                        <Form.Item
                            name="image"
                            label="Cover Image"
                            valuePropName="fileList"
                            getValueFromEvent={normFile}
                            tooltip="Only root categories can have a cover image"
                        >
                            <Upload 
                                listType="picture-card"
                                maxCount={1}
                                beforeUpload={beforeUpload}
                                onPreview={handlePreview}
                                showUploadList={{ showPreviewIcon: true, showRemoveIcon: true }}
                                accept="image/*"
                            >
                                <div className="flex flex-col items-center justify-center text-gray-500">
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

            <Image
                width={200}
                style={{ display: 'none' }}
                src={previewImage}
                preview={{
                    visible: previewOpen,
                    scaleStep: 0.5,
                    src: previewImage,
                    onVisibleChange: (value) => {
                        setPreviewOpen(value);
                    },
                    zIndex: 2000, 
                }}
            />
        </>
    );
};

export default React.memo(UpsertCategoryModal);