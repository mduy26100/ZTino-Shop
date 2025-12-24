import React from 'react';
import { Card, Typography, Row, Col, message } from 'antd';
import { SafetyCertificateFilled } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import { LoginForm, useLogin } from '../../features/auth';

const { Title, Text } = Typography;

const LoginPage = () => {
    const navigate = useNavigate();
    const { login, isLoading } = useLogin();
    const [messageApi, contextHolder] = message.useMessage();

    const handleLoginSubmit = async (values) => {
        try {            
            await login(values);
            
            navigate('/dashboard', { 
                replace: true, 
                state: { showSuccessLogin: true } 
            });
            
        } catch (err) {            
            const apiErrorMessage = err.Error?.Message || err.message || 'Invalid email or password.';
            
            messageApi.open({
                type: 'error',
                content: apiErrorMessage,
            });
        }
    };

    return (
        <div style={{ backgroundColor: '#f0f2f5', minHeight: '100vh' }}>
            {contextHolder}

            <Row justify="center" align="middle" style={{ minHeight: '100vh' }}>
                <Col xs={22} sm={16} md={12} lg={8} xl={6}>
                    <Card 
                        bordered={false}
                        className="shadow-lg rounded-xl overflow-hidden" 
                        bodyStyle={{ padding: '40px' }}
                    >
                        <div className="flex flex-col items-center mb-8">
                            <div className="flex items-center justify-center w-16 h-16 bg-blue-50 rounded-full mb-4 text-blue-600">
                                <SafetyCertificateFilled style={{ fontSize: '32px' }} />
                            </div>
                            <Title level={3} className="!mb-1 !text-gray-700">Welcome back, Manager</Title>
                            <Text type="secondary">Access your Z-Tino dashboard</Text>
                        </div>

                        <LoginForm 
                            onFinish={handleLoginSubmit} 
                            isLoading={isLoading}
                        />
                    </Card>
                </Col>
            </Row>
        </div>
    );
};

export default LoginPage;