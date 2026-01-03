import React, { useEffect, useMemo } from 'react';
import { Modal, Form, Select, InputNumber, Switch, Row, Col, Typography, Button } from 'antd';

const { Text } = Typography;

const UpsertProductVariantModal = ({ 
    open, 
    onCancel, 
    onSubmit, 
    productColors = [], 
    sizes = [], 
    isLoadingProductColors, 
    isLoadingSizes, 
    confirmLoading,
    initialValues
}) => {
    const [form] = Form.useForm();
    const isEdit = !!initialValues;

    useEffect(() => {
        if (open) {
            if (initialValues) {
                form.setFieldsValue({
                    ...initialValues,
                    productColorId: initialValues.productColorId || initialValues.productColor?.id,
                    sizeId: initialValues.sizeId || initialValues.size?.id,
                });
            } else {
                form.resetFields();
                form.setFieldsValue({
                    isActive: true,
                    stockQuantity: 0,
                    price: 0
                });
            }
        }
    }, [open, initialValues, form]);

    const handleSubmit = async () => {
        try {
            const values = await form.validateFields();
            onSubmit?.(values); 
        } catch (error) {
            console.error('Validate Failed:', error);
        }
    };

    const productColorOptions = useMemo(() => {
        return productColors.map(pc => ({
            label: (
                <div className="flex items-center gap-2">
                    <div 
                        className="w-4 h-4 rounded-full border border-gray-200" 
                        style={{ backgroundColor: pc.color?.name }}
                    />
                    <span>{pc.color?.name}</span>
                </div>
            ),
            value: pc.id,
            raw: pc 
        }));
    }, [productColors]);

    const sizeOptions = useMemo(() => {
        return sizes.map(size => ({
            label: size.name,
            value: size.id
        }));
    }, [sizes]);

    return (
        <Modal
            title={
                <span className="text-xl font-semibold text-slate-700">
                    {isEdit ? "Update Product Variant" : "Create Product Variant"}
                </span>
            }
            open={open}
            onCancel={onCancel}
            width={600}
            destroyOnHidden
            centered
            maskClosable={!confirmLoading}
            footer={[
                <Button 
                    key="back" 
                    onClick={onCancel} 
                    className="h-9 px-4 rounded-lg"
                    disabled={confirmLoading}
                >
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
            styles={{
                mask: { backdropFilter: 'blur(4px)' },
                content: { padding: '24px', borderRadius: '16px' }
            }}
        >
            <Form
                form={form}
                layout="vertical"
                className="pt-4"
                initialValues={{ isActive: true, stockQuantity: 0, price: 0 }}
            >
                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            name="productColorId"
                            label={<span className="font-medium text-gray-700">Color</span>}
                            rules={[{ required: true, message: 'Please select a color' }]}
                        >
                            <Select 
                                placeholder="Select color"
                                loading={isLoadingProductColors}
                                options={productColorOptions}
                                showSearch
                                filterOption={(input, option) => 
                                    option.raw?.color?.name?.toLowerCase().includes(input.toLowerCase())
                                }
                                className="h-10"
                            />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item
                            name="sizeId"
                            label={<span className="font-medium text-gray-700">Size</span>}
                            rules={[{ required: true, message: 'Please select a size' }]}
                        >
                            <Select 
                                placeholder="Select size"
                                loading={isLoadingSizes}
                                options={sizeOptions}
                                showSearch
                                filterOption={(input, option) => 
                                    option.label.toLowerCase().includes(input.toLowerCase())
                                }
                                className="h-10"
                            />
                        </Form.Item>
                    </Col>
                </Row>

                <Row gutter={16}>
                    <Col span={12}>
                        <Form.Item
                            name="price"
                            label={<span className="font-medium text-gray-700">Price (VND)</span>}
                            rules={[
                                { required: true, message: 'Please enter price' },
                                { type: 'number', min: 0, message: 'Price cannot be negative' }
                            ]}
                        >
                            <InputNumber 
                                className="w-full h-10 pt-1"
                                formatter={(value) => `${value}`.replace(/\B(?=(\d{3})+(?!\d))/g, ',')}
                                parser={(value) => value?.replace(/\$\s?|(,*)/g, '')}
                                placeholder="0"
                                min={0}
                                addonAfter="₫"
                            />
                        </Form.Item>
                    </Col>
                    <Col span={12}>
                        <Form.Item
                            name="stockQuantity"
                            label={<span className="font-medium text-gray-700">Stock Quantity</span>}
                            rules={[
                                { required: true, message: 'Please enter stock quantity' },
                                { type: 'number', min: 0, message: 'Stock cannot be negative' }
                            ]}
                        >
                            <InputNumber 
                                className="w-full h-10 pt-1" 
                                placeholder="0" 
                                min={0} 
                                precision={0}
                            />
                        </Form.Item>
                    </Col>
                </Row>

                <div className="flex items-center justify-between p-4 bg-gray-50 rounded-xl border border-gray-100 mt-2 mb-2">
                    <div className="flex flex-col">
                        <Text strong className="text-slate-700">Active Status</Text>
                        <Text type="secondary" className="text-xs">
                            Allow this variant to be sold
                        </Text>
                    </div>
                    <Form.Item name="isActive" valuePropName="checked" noStyle>
                        <Switch />
                    </Form.Item>
                </div>
            </Form>
        </Modal>
    );
};

export default React.memo(UpsertProductVariantModal);