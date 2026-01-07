import React, { memo } from 'react';
import { Form, Input, Divider, Typography } from 'antd';
import { UserOutlined, PhoneOutlined, MailOutlined, HomeOutlined, EnvironmentOutlined } from '@ant-design/icons';

const { Title } = Typography;
const { TextArea } = Input;

const PHONE_REGEX = /^(0|\+84)[0-9]{9,10}$/;

const normalizePhone = (value) => value?.replace(/\s/g, '') || '';

const CheckoutForm = memo(({ form }) => {
    return (
        <div className="space-y-6">
            <div>
                <Title level={5} className="!mb-4 !text-gray-800 flex items-center gap-2">
                    <UserOutlined className="text-indigo-600" />
                    Customer Information
                </Title>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <Form.Item
                        name="customerName"
                        label="Full Name"
                        rules={[
                            { required: true, message: 'Customer name is required.' },
                            { max: 100, message: 'Customer name must not exceed 100 characters.' }
                        ]}
                    >
                        <Input 
                            prefix={<UserOutlined className="text-gray-400" />}
                            placeholder="Enter your full name"
                            maxLength={100}
                        />
                    </Form.Item>

                    <Form.Item
                        name="customerPhone"
                        label="Phone Number"
                        normalize={normalizePhone}
                        rules={[
                            { required: true, message: 'Customer phone is required.' },
                            { 
                                pattern: PHONE_REGEX, 
                                message: 'Invalid phone number format. Expected: 0xxxxxxxxx or +84xxxxxxxxx' 
                            }
                        ]}
                    >
                        <Input 
                            prefix={<PhoneOutlined className="text-gray-400" />}
                            placeholder="0912345678"
                            maxLength={12}
                        />
                    </Form.Item>
                </div>

                <Form.Item
                    name="customerEmail"
                    label="Email (Optional)"
                    rules={[
                        { type: 'email', message: 'Invalid email format.' }
                    ]}
                >
                    <Input 
                        prefix={<MailOutlined className="text-gray-400" />}
                        placeholder="your.email@example.com"
                    />
                </Form.Item>
            </div>

            <Divider />

            <div>
                <Title level={5} className="!mb-4 !text-gray-800 flex items-center gap-2">
                    <EnvironmentOutlined className="text-indigo-600" />
                    Shipping Address
                </Title>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <Form.Item
                        name="recipientName"
                        label="Recipient Name"
                        rules={[
                            { required: true, message: 'Recipient name is required.' },
                            { max: 100, message: 'Recipient name must not exceed 100 characters.' }
                        ]}
                    >
                        <Input 
                            prefix={<UserOutlined className="text-gray-400" />}
                            placeholder="Recipient's full name"
                            maxLength={100}
                        />
                    </Form.Item>

                    <Form.Item
                        name="shippingPhoneNumber"
                        label="Recipient Phone"
                        normalize={normalizePhone}
                        rules={[
                            { required: true, message: 'Recipient phone number is required.' },
                            { 
                                pattern: PHONE_REGEX, 
                                message: 'Invalid recipient phone number format.' 
                            }
                        ]}
                    >
                        <Input 
                            prefix={<PhoneOutlined className="text-gray-400" />}
                            placeholder="0912345678"
                            maxLength={12}
                        />
                    </Form.Item>
                </div>

                <Form.Item
                    name="street"
                    label="Street Address"
                    rules={[
                        { required: true, message: 'Street address is required.' },
                        { max: 200, message: 'Street address must not exceed 200 characters.' }
                    ]}
                >
                    <Input 
                        prefix={<HomeOutlined className="text-gray-400" />}
                        placeholder="House number, street name"
                        maxLength={200}
                    />
                </Form.Item>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                    <Form.Item
                        name="ward"
                        label="Ward"
                        rules={[
                            { required: true, message: 'Ward is required.' },
                            { max: 100, message: 'Ward must not exceed 100 characters.' }
                        ]}
                    >
                        <Input placeholder="Ward name" maxLength={100} />
                    </Form.Item>

                    <Form.Item
                        name="district"
                        label="District"
                        rules={[
                            { required: true, message: 'District is required.' },
                            { max: 100, message: 'District must not exceed 100 characters.' }
                        ]}
                    >
                        <Input placeholder="District name" maxLength={100} />
                    </Form.Item>

                    <Form.Item
                        name="city"
                        label="City"
                        rules={[
                            { required: true, message: 'City is required.' },
                            { max: 100, message: 'City must not exceed 100 characters.' }
                        ]}
                    >
                        <Input placeholder="City name" maxLength={100} />
                    </Form.Item>
                </div>
            </div>

            <Divider />

            <div>
                <Form.Item
                    name="note"
                    label="Order Notes (Optional)"
                    rules={[
                        { max: 500, message: 'Note must not exceed 500 characters.' }
                    ]}
                >
                    <TextArea 
                        placeholder="Special instructions for delivery, gift message, etc."
                        rows={3}
                        maxLength={500}
                        showCount
                    />
                </Form.Item>
            </div>
        </div>
    );
});

CheckoutForm.displayName = 'CheckoutForm';

export default CheckoutForm;
