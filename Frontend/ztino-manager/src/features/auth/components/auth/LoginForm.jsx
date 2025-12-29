import React, { useState, useEffect } from 'react';
import { Form, Input, Button, Checkbox, Tooltip } from 'antd';
import { UserOutlined, LockOutlined, CheckCircleFilled } from '@ant-design/icons';
import PropTypes from 'prop-types';

const LoginForm = ({ onFinish, isLoading }) => {
    const [form] = Form.useForm();
    const [isEmailValid, setIsEmailValid] = useState(false);

    useEffect(() => {
        const savedEmail = localStorage.getItem('remembered_email');
        if (savedEmail) {
            form.setFieldsValue({ email: savedEmail, remember: true });
            setIsEmailValid(true); 
        }
    }, [form]);

    const handleEmailChange = (e) => {
        const value = e.target.value;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (emailRegex.test(value)) {
            setIsEmailValid(true);
        } else {
            setIsEmailValid(false);
        }
    };

    const handleSubmit = (values) => {
        if (values.remember) {
            localStorage.setItem('remembered_email', values.email);
        } else {
            localStorage.removeItem('remembered_email');
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
                label="Email Address"
                name="email"
                rules={[
                    { required: true, message: 'Please input your Email!' },
                    { type: 'email', message: 'Invalid email format!' }
                ]}
                className="mb-5"
            >
                <Input 
                    onChange={handleEmailChange}
                    prefix={<UserOutlined className="text-gray-400" />} 
                    suffix={
                        isEmailValid && (
                            <Tooltip title="Email format is valid">
                                <CheckCircleFilled className="text-green-500 animate-fade-in" />
                            </Tooltip>
                        )
                    }
                    placeholder="Enter your email" 
                    className="rounded-lg py-2.5" 
                />
            </Form.Item>

            <Form.Item
                label="Password"
                name="password"
                rules={[{ required: true, message: 'Please input your Password!' }]}
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