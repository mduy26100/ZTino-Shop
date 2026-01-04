import React, { memo, useCallback, useMemo, useState } from 'react';
import { Form, Input, Button, Alert, message } from 'antd';
import { UserOutlined, LockOutlined, PhoneOutlined, IdcardOutlined } from '@ant-design/icons';
import { Link, useNavigate } from 'react-router-dom';
import { useRegister } from '../../hooks/auth/useRegister';

const RegisterForm = memo(({ onSuccess }) => {
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const { register, isRegistering } = useRegister();
    const [error, setError] = useState(null);

    const handleSubmit = useCallback(async (values) => {
        setError(null);
        try {
            await register(values, {
                onSuccess: () => {
                    message.success('Account created successfully! Please login.');
                    if (onSuccess) {
                        onSuccess();
                    } else {
                        navigate('/login');
                    }
                },
                onError: (err) => {
                    const errorMsg = err?.response?.data?.errors 
                        ? Object.values(err.response.data.errors).flat().join(', ')
                        : err?.error?.message || err?.message || 'Registration failed. Please try again.';
                    setError(errorMsg);
                }
            });
        } catch (err) {
        }
    }, [register, navigate, onSuccess]);

    const formRules = useMemo(() => ({
        firstName: [
            { required: true, message: 'FirstName is required.' },
            { max: 50, message: 'First name must be at most 50 characters.' }
        ],
        lastName: [
            { required: true, message: 'LastName is required.' },
            { max: 50, message: 'Last name must be at most 50 characters.' }
        ],
        userName: [
            { required: true, message: 'UserName is required.' },
            { min: 3, max: 50, message: 'UserName must be between 3 and 50 characters.' },
            { pattern: /^[a-zA-Z0-9_.]+$/, message: 'UserName can only contain letters, numbers, underscores, or dots.' }
        ],
        phoneNumber: [
            { required: true, message: 'Phone number is required.' }
        ],
        password: [
            { required: true, message: 'Password is required.' },
            { min: 6, message: 'Password must be at least 6 characters.' }
        ],
        confirmPassword: [
            { required: true, message: 'Please confirm your password.' },
            ({ getFieldValue }) => ({
                validator(_, value) {
                    if (!value || getFieldValue('password') === value) {
                        return Promise.resolve();
                    }
                    return Promise.reject(new Error('Passwords do not match.'));
                }
            })
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
                    closable
                    onClose={() => setError(null)}
                />
            )}

            <Form
                form={form}
                name="register"
                onFinish={handleSubmit}
                layout="vertical"
                size="large"
                autoComplete="off"
            >
                <div className="grid grid-cols-2 gap-4">
                    <Form.Item
                        name="firstName"
                        label={<span className="text-gray-700 font-medium">First Name</span>}
                        rules={formRules.firstName}
                    >
                        <Input
                            prefix={<IdcardOutlined className="text-gray-400" />}
                            placeholder="First name"
                            className="h-12 rounded-lg"
                        />
                    </Form.Item>

                    <Form.Item
                        name="lastName"
                        label={<span className="text-gray-700 font-medium">Last Name</span>}
                        rules={formRules.lastName}
                    >
                        <Input
                            prefix={<IdcardOutlined className="text-gray-400" />}
                            placeholder="Last name"
                            className="h-12 rounded-lg"
                        />
                    </Form.Item>
                </div>

                <Form.Item
                    name="userName"
                    label={<span className="text-gray-700 font-medium">Username</span>}
                    rules={formRules.userName}
                >
                    <Input
                        prefix={<UserOutlined className="text-gray-400" />}
                        placeholder="Choose a username"
                        className="h-12 rounded-lg"
                    />
                </Form.Item>

                <Form.Item
                    name="phoneNumber"
                    label={<span className="text-gray-700 font-medium">Phone Number</span>}
                    rules={formRules.phoneNumber}
                >
                    <Input
                        prefix={<PhoneOutlined className="text-gray-400" />}
                        placeholder="Enter your phone number"
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
                        placeholder="Create a password"
                        className="h-12 rounded-lg"
                    />
                </Form.Item>

                <Form.Item
                    name="confirmPassword"
                    label={<span className="text-gray-700 font-medium">Confirm Password</span>}
                    rules={formRules.confirmPassword}
                    dependencies={['password']}
                >
                    <Input.Password
                        prefix={<LockOutlined className="text-gray-400" />}
                        placeholder="Confirm your password"
                        className="h-12 rounded-lg"
                    />
                </Form.Item>

                <Form.Item className="mb-4 mt-6">
                    <Button
                        type="primary"
                        htmlType="submit"
                        loading={isRegistering}
                        className="w-full h-12 rounded-lg bg-indigo-600 font-medium hover:!bg-indigo-700 border-none shadow-none"
                    >
                        {isRegistering ? 'Creating Account...' : 'Create Account'}
                    </Button>
                </Form.Item>

                <div className="text-center text-gray-600">
                    Already have an account?{' '}
                    <Link
                        to="/login"
                        className="text-indigo-600 hover:text-indigo-800 font-medium transition-colors"
                    >
                        Sign In
                    </Link>
                </div>
            </Form>
        </div>
    );
});

RegisterForm.displayName = 'RegisterForm';

export default RegisterForm;
