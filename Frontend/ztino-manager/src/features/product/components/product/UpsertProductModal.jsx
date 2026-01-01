import React, { useEffect, useMemo, useRef, useState } from 'react';
import { 
    Modal, Form, Input, Button, InputNumber, 
    TreeSelect, Upload, Row, Col, Switch, message, Image 
} from 'antd';
import { PlusOutlined, EyeOutlined } from '@ant-design/icons';
import JoditEditor from 'jodit-react';

const normFile = (e) => {
    if (Array.isArray(e)) return e;
    return e?.fileList;
};

const generateSlug = (text) => {
    return text
        ?.toString()
        .toLowerCase()
        .normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '')
        .replace(/\s+/g, '-')
        .replace(/[^\w-]+/g, '')
        .replace(/--+/g, '-') || '';
};

const beforeUpload = (file) => {
    const validTypes = ['image/jpeg', 'image/png', 'image/webp'];
    const isValidType = validTypes.includes(file.type);
    
    if (!isValidType) {
        message.error('You can only upload JPG/PNG/WEBP files!');
        return Upload.LIST_IGNORE;
    }

    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
        message.error('Image must be smaller than 2MB!');
        return Upload.LIST_IGNORE;
    }

    return false;
};

const getBase64 = (file) =>
    new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = (error) => reject(error);
    });

const UpsertProductModal = ({
    open,
    onCancel,
    onSubmit,
    confirmLoading,
    initialValues = null,
    categories = [],      
    isLoadingCategories   
}) => {
    const [form] = Form.useForm();
    const isEdit = !!initialValues;
    const editor = useRef(null);

    const [previewOpen, setPreviewOpen] = useState(false);
    const [previewImage, setPreviewImage] = useState('');

    const treeData = useMemo(() => {
        const transform = (nodes) => {
            return nodes.map(node => {
                const isRoot = node.parentId === null;

                return {
                    value: node.id,
                    title: node.name,
                    disabled: isRoot,
                    selectable: !isRoot,
                    children: node.children && node.children.length > 0 
                        ? transform(node.children) 
                        : [],
                    className: isRoot ? 'font-semibold text-slate-700' : '' 
                };
            });
        };
        
        const rawList = Array.isArray(categories) ? categories : (categories?.Data || []);
        return transform(rawList);
    }, [categories]);

    const editorConfig = useMemo(() => ({
        readonly: false,
        placeholder: 'Enter product description...',
        height: 300,
        menubar: false,
        toolbarAdaptive: false,
        buttons: [
            'bold', 'italic', 'underline', 'strikethrough', '|',
            'font', 'fontsize', 'brush', '|',
            'ul', 'ol', 'align', '|',
            'link', 'table', 'image', '|',
            'undo', 'redo', 'hr', 'source'
        ],
        uploader: { insertImageAsBase64URI: true },
        showCharsCounter: false,
        showWordsCounter: false,
        showXPathInStatusbar: false
    }), []);

    useEffect(() => {
        if (open) {
            if (initialValues) {
                form.setFieldsValue({
                    ...initialValues,
                    MainImageUrl: initialValues.mainImageUrl 
                        ? [{ 
                            uid: '-1', 
                            name: 'current-image', 
                            status: 'done', 
                            url: initialValues.mainImageUrl,
                            thumbUrl: initialValues.mainImageUrl
                        }] 
                        : []
                });
            } else {
                form.resetFields();
                form.setFieldsValue({ 
                    IsActive: true, 
                    BasePrice: 0 
                });
            }
        }
    }, [open, initialValues, form]);

    const handleNameChange = (e) => {
        if (!isEdit) {
            const slug = generateSlug(e.target.value);
            form.setFieldValue('Slug', slug);
        }
    };

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
            onSubmit(values);
        } catch (error) {
            console.error('Validate Failed:', error);
        }
    };

    return (
        <>
            <Modal
                title={
                    <span className="text-xl font-semibold text-slate-700">
                        {isEdit ? "Update Product" : "Create New Product"}
                    </span>
                }
                open={open}
                onCancel={onCancel}
                width={900} 
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
                maskClosable={!confirmLoading}
                centered
                styles={{
                    mask: { backdropFilter: 'blur(4px)' },
                    content: { padding: '24px', borderRadius: '16px' }
                }}
                zIndex={1000}
            >
                <Form form={form} layout="vertical" className="pt-4">
                    <Row gutter={24}>
                        <Col xs={24} md={8}>
                            <Form.Item
                                name="MainImageUrl"
                                label="Product Image"
                                valuePropName="fileList"
                                getValueFromEvent={normFile}
                                rules={[{ required: !isEdit, message: 'Please upload an image' }]}
                                tooltip="Supports JPG, PNG, WEBP. Max 2MB."
                            >
                                <Upload
                                    listType="picture-card"
                                    maxCount={1}
                                    beforeUpload={beforeUpload} 
                                    onPreview={handlePreview}
                                    showUploadList={{ 
                                        showPreviewIcon: true,
                                        showRemoveIcon: true
                                    }}
                                    accept="image/png, image/jpeg, image/webp"
                                >
                                    <div className="flex flex-col items-center justify-center text-gray-500">
                                        <PlusOutlined className="text-lg mb-2" />
                                        <span className="text-xs">Upload</span>
                                    </div>
                                </Upload>
                            </Form.Item>

                            <Form.Item name="IsActive" label="Status" valuePropName="checked">
                                <Switch checkedChildren="Active" unCheckedChildren="Inactive" />
                            </Form.Item>
                        </Col>

                        <Col xs={24} md={16}>
                            <Form.Item
                                name="Name"
                                label="Product Name"
                                rules={[{ required: true, message: 'Please enter product name' }]}
                            >
                                <Input onChange={handleNameChange} placeholder="e.g., Slim Fit T-Shirt" />
                            </Form.Item>

                            <Form.Item
                                name="Slug"
                                label="Slug"
                                rules={[{ required: true, message: 'Slug is required' }]}
                                tooltip="URL-friendly version of the name"
                            >
                                <Input placeholder="auto-generated-slug" />
                            </Form.Item>

                            <Row gutter={16}>
                                <Col span={12}>
                                    <Form.Item
                                        name="CategoryId"
                                        label="Category"
                                        rules={[{ required: true, message: 'Select a category' }]}
                                    >
                                        <TreeSelect
                                            style={{ width: '100%' }}
                                            dropdownStyle={{ maxHeight: 400, overflow: 'auto' }}
                                            treeData={treeData}
                                            placeholder="Select sub-category"
                                            treeDefaultExpandAll
                                            allowClear
                                            loading={isLoadingCategories}
                                            showSearch
                                            treeNodeFilterProp="title"
                                        />
                                    </Form.Item>
                                </Col>
                                <Col span={12}>
                                    <Form.Item
                                        name="BasePrice"
                                        label="Base Price (VND)"
                                        rules={[{ required: true, message: 'Enter price' }]}
                                    >
                                        <InputNumber
                                            className="w-full"
                                            formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                                            parser={(value) => value?.replace(/\$\s?|(,*)/g, '')}
                                            min={0}
                                            addonAfter="₫"
                                        />
                                    </Form.Item>
                                </Col>
                            </Row>

                            <Form.Item 
                                name="Description" 
                                label="Description"
                                rules={[{ required: true, message: 'Please enter description' }]}
                                trigger="onBlur"
                                validateTrigger="onBlur"
                            >
                               <JoditEditor
                                    ref={editor}
                                    config={editorConfig}
                                />
                            </Form.Item>
                        </Col>
                    </Row>
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
                    zIndex: 2000
                }}
            />
        </>
    );
};

export default React.memo(UpsertProductModal);