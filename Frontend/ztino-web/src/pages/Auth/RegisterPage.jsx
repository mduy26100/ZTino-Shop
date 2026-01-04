import React, { memo, useMemo } from 'react';
import { Card, Typography } from 'antd';
import { Link } from 'react-router-dom';
import RegisterForm from '../../features/auth/components/auth/RegisterForm';

const { Title, Text } = Typography;

const RegisterPage = memo(() => {
    const brandingContent = useMemo(() => (
        <div className="text-center mb-8">
            <Link to="/" className="inline-block mb-6">
                <div className="flex items-center justify-center gap-2">
                    <div className="w-10 h-10 bg-indigo-600 rounded-lg flex items-center justify-center">
                        <span className="text-white font-bold text-xl">Z</span>
                    </div>
                    <span className="text-2xl font-bold text-slate-800">ZTino</span>
                </div>
            </Link>
            <Title level={3} className="!mb-2 !text-gray-800">
                Create Your Account
            </Title>
            <Text className="text-gray-500">
                Join ZTino and start shopping today
            </Text>
        </div>
    ), []);

    return (
        <div className="min-h-screen bg-gradient-to-br from-slate-50 to-indigo-50 flex items-center justify-center py-12 px-4">
            <div className="w-full max-w-lg">
                <Card
                    className="shadow-xl border-0 rounded-2xl"
                    styles={{
                        body: { padding: '40px 32px' }
                    }}
                >
                    {brandingContent}
                    <RegisterForm />
                </Card>

                <div className="mt-8 text-center text-gray-500 text-sm">
                    <Text>
                        By creating an account, you agree to our{' '}
                        <Link to="/terms" className="text-indigo-600 hover:text-indigo-800">
                            Terms of Service
                        </Link>
                        {' '}and{' '}
                        <Link to="/privacy" className="text-indigo-600 hover:text-indigo-800">
                            Privacy Policy
                        </Link>
                    </Text>
                </div>
            </div>
        </div>
    );
});

RegisterPage.displayName = 'RegisterPage';

export default RegisterPage;
