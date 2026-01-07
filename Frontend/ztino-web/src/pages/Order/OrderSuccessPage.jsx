import React, { memo, useMemo } from 'react';
import { Typography, Button, Card, Tag, Divider } from 'antd';
import { 
    CheckCircleOutlined, 
    ShoppingOutlined, 
    HomeOutlined, 
    FileTextOutlined,
    CreditCardOutlined,
    ClockCircleOutlined,
    DollarOutlined
} from '@ant-design/icons';
import { Link, useLocation } from 'react-router-dom';

const { Title, Text, Paragraph } = Typography;

const getStatusColor = (status) => {
    const statusMap = {
        'Pending': 'orange',
        'Processing': 'blue',
        'Shipped': 'cyan',
        'Delivered': 'green',
        'Cancelled': 'red',
    };
    return statusMap[status] || 'default';
};

const getPaymentStatusColor = (status) => {
    const statusMap = {
        'Pending': 'orange',
        'Paid': 'green',
        'Failed': 'red',
        'Refunded': 'purple',
    };
    return statusMap[status] || 'default';
};

const OrderSuccessPage = memo(() => {
    const location = useLocation();
    
    const orderData = useMemo(() => location.state?.orderData || null, [location.state]);

    const formattedAmount = useMemo(() => {
        if (!orderData?.totalAmount) return null;
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND'
        }).format(orderData.totalAmount);
    }, [orderData?.totalAmount]);

    return (
        <div className="bg-gradient-to-br from-green-50 to-indigo-50 min-h-screen py-12 sm:py-16">
            <div className="max-w-2xl mx-auto px-4 sm:px-6 lg:px-8">
                <Card 
                    className="border-0 shadow-2xl rounded-3xl overflow-hidden"
                    styles={{ body: { padding: 0 } }}
                >
                    <div className="bg-gradient-to-r from-green-500 to-emerald-600 px-8 py-12 text-center">
                        <div className="inline-flex items-center justify-center w-20 h-20 bg-white rounded-full mb-6 shadow-lg">
                            <CheckCircleOutlined className="text-4xl text-green-500" />
                        </div>
                        <Title level={2} className="!text-white !mb-2">
                            Order Placed Successfully!
                        </Title>
                        <Text className="text-green-100 text-lg">
                            {orderData?.message || 'Thank you for your purchase'}
                        </Text>
                    </div>

                    <div className="px-8 py-8">
                        {orderData?.orderCode && (
                            <div className="bg-gray-50 rounded-xl p-6 mb-6 text-center">
                                <Text className="text-gray-500 text-sm block mb-1">Order Code</Text>
                                <Text className="text-2xl font-bold text-indigo-600 font-mono">
                                    {orderData.orderCode}
                                </Text>
                            </div>
                        )}

                        {orderData && (
                            <div className="grid grid-cols-2 gap-4 mb-6">
                                {formattedAmount && (
                                    <div className="bg-indigo-50 rounded-xl p-4 text-center">
                                        <DollarOutlined className="text-indigo-500 text-xl mb-2" />
                                        <Text className="text-gray-500 text-xs block mb-1">Total Amount</Text>
                                        <Text className="text-lg font-bold text-indigo-600">
                                            {formattedAmount}
                                        </Text>
                                    </div>
                                )}

                                {orderData.paymentMethod && (
                                    <div className="bg-blue-50 rounded-xl p-4 text-center">
                                        <CreditCardOutlined className="text-blue-500 text-xl mb-2" />
                                        <Text className="text-gray-500 text-xs block mb-1">Payment Method</Text>
                                        <Text className="text-lg font-bold text-blue-600">
                                            {orderData.paymentMethod}
                                        </Text>
                                    </div>
                                )}

                                {orderData.status && (
                                    <div className="bg-orange-50 rounded-xl p-4 text-center">
                                        <ClockCircleOutlined className="text-orange-500 text-xl mb-2" />
                                        <Text className="text-gray-500 text-xs block mb-1">Order Status</Text>
                                        <Tag color={getStatusColor(orderData.status)} className="mt-1">
                                            {orderData.status}
                                        </Tag>
                                    </div>
                                )}

                                {orderData.paymentStatus && (
                                    <div className="bg-green-50 rounded-xl p-4 text-center">
                                        <CheckCircleOutlined className="text-green-500 text-xl mb-2" />
                                        <Text className="text-gray-500 text-xs block mb-1">Payment Status</Text>
                                        <Tag color={getPaymentStatusColor(orderData.paymentStatus)} className="mt-1">
                                            {orderData.paymentStatus}
                                        </Tag>
                                    </div>
                                )}
                            </div>
                        )}

                        <Divider className="my-6" />

                        <Paragraph className="text-gray-600 text-center mb-8">
                            We've sent a confirmation email with your order details. 
                            You can track your order status in your account.
                        </Paragraph>

                        <div className="space-y-3">
                            <Link to="/" className="block">
                                <Button
                                    type="primary"
                                    size="large"
                                    block
                                    icon={<HomeOutlined />}
                                    className="h-12 bg-indigo-600 hover:!bg-indigo-700 border-none font-medium rounded-lg"
                                >
                                    Continue Shopping
                                </Button>
                            </Link>

                            <Link to="/orders" className="block">
                                <Button
                                    size="large"
                                    block
                                    icon={<FileTextOutlined />}
                                    className="h-12 font-medium rounded-lg border-indigo-200 text-indigo-600 hover:!border-indigo-400 hover:!text-indigo-700"
                                >
                                    View My Orders
                                </Button>
                            </Link>
                        </div>
                    </div>

                    <div className="bg-gray-50 px-8 py-6 border-t border-gray-100">
                        <div className="flex flex-col sm:flex-row items-center justify-center gap-4 text-gray-500 text-sm">
                            <span className="flex items-center gap-2">
                                <ShoppingOutlined className="text-indigo-500" />
                                Free shipping on all orders
                            </span>
                            <span className="hidden sm:block">•</span>
                            <span className="flex items-center gap-2">
                                📞 Need help? Contact support
                            </span>
                        </div>
                    </div>
                </Card>
            </div>
        </div>
    );
});

OrderSuccessPage.displayName = 'OrderSuccessPage';

export default OrderSuccessPage;
