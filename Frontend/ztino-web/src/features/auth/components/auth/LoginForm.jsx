import React, { memo, useCallback, useMemo } from 'react';
import { Form, Input, Button, Checkbox, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import { useLogin } from '../../hooks/auth/useLogin';

const LoginForm = memo(({ onSuccess }) => {
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const { login, isLoading, error } = useLogin();

    const handleSubmit = useCallback(async (values) => {
        try {
            await login(values);
            if (onSuccess) {
                onSuccess();
            } else {
                navigate('/');
            }
        } catch (err) {
        }
    }, [login, navigate, onSuccess]);

    const formRules = useMemo(() => ({
        identifier: [
            { required: true, message: 'Please enter your email or username' }
        ],
        password: [
            { required: true, message: 'Please enter your password' },
            { min: 6, message: 'Password must be at least 6 characters' }
        ]
    }), []);

    return (
        <div className="w-full">
            {error && (
                <Alert
                    message={error}
                    type="error"
                    showIcon
                    className="mb-6"
                />
            )}

            <Form
                form={form}
                name="login"
                onFinish={handleSubmit}
                layout="vertical"
                size="large"
                autoComplete="off"
            >
                <Form.Item
                    name="identifier"
                    label={<span className="text-gray-700 font-medium">Email or Username</span>}
                    rules={formRules.identifier}
                >
                    <Input
                        prefix={<UserOutlined className="text-gray-400" />}
                        placeholder="Enter your email or username"
                        className="h-12 rounded-lg"
                    />
                </Form.Item>

                <Form.Item
                    name="password"
                    label={<span className="text-gray-700 font-medium">Password</span>}
                    rules={formRules.password}
                >
                    <Input.Password
                        prefix={<LockOutlined className="text-gray-400" />}
                        placeholder="Enter your password"
                        className="h-12 rounded-lg"
                    />
                </Form.Item>

                <div className="flex items-center justify-between mb-6">
                    <Form.Item name="remember" valuePropName="checked" noStyle>
                        <Checkbox className="text-gray-600">
                            Remember me
                        </Checkbox>
                    </Form.Item>

                    <Link
                        to="/forgot-password"
                        className="text-indigo-600 hover:text-indigo-800 font-medium transition-colors"
                    >
                        Forgot password?
                    </Link>
                </div>

                <Form.Item className="mb-4">
                    <Button
                        type="primary"
                        htmlType="submit"
                        loading={isLoading}
                        className="w-full h-12 rounded-lg bg-indigo-600 font-medium hover:!bg-indigo-700 border-none shadow-none"
                    >
                        {isLoading ? 'Signing in...' : 'Sign In'}
                    </Button>
                </Form.Item>

                <div className="text-center text-gray-600">
                    Don't have an account?{' '}
                    <Link
                        to="/register"
                        className="text-indigo-600 hover:text-indigo-800 font-medium transition-colors"
                    >
                        Create Account
                    </Link>
                </div>
            </Form>
        </div>
    );
});

LoginForm.displayName = 'LoginForm';

export default LoginForm;
