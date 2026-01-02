import React, { useState, useEffect } from 'react';
import { Form, Input, Button, Checkbox, Tooltip } from 'antd';
import { UserOutlined, LockOutlined, CheckCircleFilled } from '@ant-design/icons';
import PropTypes from 'prop-types';

const LoginForm = ({ onFinish, isLoading }) => {
    const [form] = Form.useForm();
    const [isValidIdentifier, setIsValidIdentifier] = useState(false);

    useEffect(() => {
        const savedIdentifier = localStorage.getItem('remembered_identifier');
        if (savedIdentifier) {
            form.setFieldsValue({ identifier: savedIdentifier, remember: true });
            validateIdentifier(savedIdentifier);
        }
    }, [form]);

    const validateIdentifier = (value) => {
        if (!value) {
            setIsValidIdentifier(false);
            return;
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const usernameRegex = /^[a-zA-Z0-9_.]+$/;
        
        const isEmail = emailRegex.test(value);
        const isUsername = usernameRegex.test(value) && value.length >= 3 && value.length <= 50;

        setIsValidIdentifier(isEmail || isUsername);
    };

    const handleInputChange = (e) => {
        validateIdentifier(e.target.value);
    };

    const handleSubmit = (values) => {
        if (values.remember) {
            localStorage.setItem('remembered_identifier', values.identifier);
        } else {
            localStorage.removeItem('remembered_identifier');
        }
        onFinish(values);
    };

    return (
        <Form
            form={form}
            name="login_form"
            layout="vertical"
            onFinish={handleSubmit}
            autoComplete="off"
            requiredMark={false}
            size="large"
        >
            <Form.Item
                label="Email or Username"
                name="identifier"
                rules={[
                    { required: true, message: 'Please input your Email or Username!' },
                    { min: 3, message: 'Identifier must be at least 3 characters' }
                ]}
                className="mb-5"
            >
                <Input 
                    onChange={handleInputChange}
                    prefix={<UserOutlined className="text-gray-400" />} 
                    suffix={
                        isValidIdentifier && (
                            <Tooltip title="Valid format">
                                <CheckCircleFilled className="text-green-500 animate-fade-in" />
                            </Tooltip>
                        )
                    }
                    placeholder="Enter email or username" 
                    className="rounded-lg py-2.5" 
                />
            </Form.Item>

            <Form.Item
                label="Password"
                name="password"
                rules={[
                    { required: true, message: 'Please input your Password!' },
                    { min: 6, message: 'Password must be at least 6 characters' }
                ]}
                className="mb-5"
            >
                <Input.Password 
                    prefix={<LockOutlined className="text-gray-400" />} 
                    placeholder="Enter your password" 
                    className="rounded-lg py-2.5" 
                />
            </Form.Item>

            <div className="flex justify-between items-center mb-8">
                <Form.Item name="remember" valuePropName="checked" noStyle>
                    <Checkbox className="text-gray-500 font-medium">
                        Remember me
                    </Checkbox>
                </Form.Item>
                <a className="text-blue-600 hover:text-blue-700 font-semibold text-sm transition-all cursor-pointer">
                    Forgot password?
                </a>
            </div>

            <Form.Item className="mb-4">
                <Button 
                    type="primary" 
                    htmlType="submit" 
                    loading={isLoading} 
                    block 
                    className="h-12 rounded-lg font-bold text-base bg-blue-600 hover:!bg-blue-700 shadow-lg shadow-blue-200 border-none"
                >
                    Sign In
                </Button>
            </Form.Item>
        </Form>
    );
};

LoginForm.propTypes = {
    onFinish: PropTypes.func.isRequired,
    isLoading: PropTypes.bool,
};

export default LoginForm;