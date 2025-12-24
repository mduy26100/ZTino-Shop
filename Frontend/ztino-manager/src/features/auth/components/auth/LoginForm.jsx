import React from 'react';
import { Form, Input, Button, Checkbox } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import PropTypes from 'prop-types';

const LoginForm = ({ onFinish, isLoading }) => {
    return (
        <Form
            name="login_form"
            layout="vertical"
            onFinish={onFinish}
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
                    prefix={<UserOutlined className="text-gray-400" />} 
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
                    <Checkbox className="text-gray-500">Remember me</Checkbox>
                </Form.Item>
                <a className="text-blue-600 hover:text-blue-700 font-medium text-sm cursor-pointer">
                    Contact support
                </a>
            </div>

            <Form.Item className="mb-4">
                <Button 
                    type="primary" 
                    htmlType="submit" 
                    loading={isLoading} 
                    block 
                    className="h-12 rounded-lg font-semibold text-base bg-blue-600 hover:!bg-blue-700 border-none"
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