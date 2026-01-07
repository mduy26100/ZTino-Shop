import React, { memo, useMemo } from 'react';
import { Card, Typography, Descriptions, Tag } from 'antd';
import { 
    UserOutlined, 
    PhoneOutlined, 
    EnvironmentOutlined, 
    CreditCardOutlined,
    FileTextOutlined 
} from '@ant-design/icons';

const { Title, Text } = Typography;

const PAYMENT_METHOD_LABELS = {
    COD: 'Cash on Delivery',
    BankTransfer: 'Bank Transfer',
    CreditCard: 'Credit Card',
    EWallet: 'E-Wallet'
};

const OrderDetailInfo = memo(({ order }) => {
    const { customerName, customerPhone, shippingAddress, paymentMethod, note } = order || {};

    const paymentMethodLabel = useMemo(() => {
        return PAYMENT_METHOD_LABELS[paymentMethod] || paymentMethod;
    }, [paymentMethod]);

    const infoItems = useMemo(() => [
        {
            key: 'customer',
            label: (
                <span className="flex items-center gap-2 text-gray-500 font-medium">
                    <UserOutlined className="text-indigo-500" />
                    Customer
                </span>
            ),
            children: <Text strong className="text-gray-800 text-base">{customerName}</Text>
        },
        {
            key: 'phone',
            label: (
                <span className="flex items-center gap-2 text-gray-500 font-medium">
                    <PhoneOutlined className="text-indigo-500" />
                    Phone
                </span>
            ),
            children: <Text className="text-gray-800 font-medium">{customerPhone}</Text>
        },
        {
            key: 'address',
            label: (
                <span className="flex items-center gap-2 text-gray-500 font-medium">
                    <EnvironmentOutlined className="text-indigo-500" />
                    Shipping Address
                </span>
            ),
            children: <Text className="text-gray-700 leading-relaxed block">{shippingAddress}</Text>,
            span: 2
        },
        {
            key: 'payment',
            label: (
                <span className="flex items-center gap-2 text-gray-500 font-medium">
                    <CreditCardOutlined className="text-indigo-500" />
                    Payment Method
                </span>
            ),
            children: <Tag color="blue" className="px-3 py-1 rounded-full m-0">{paymentMethodLabel}</Tag>
        },
        ...(note ? [{
            key: 'note',
            label: (
                <span className="flex items-center gap-2 text-gray-500 font-medium">
                    <FileTextOutlined className="text-indigo-500" />
                    Note
                </span>
            ),
            children: (
                <div className="bg-gray-50 p-3 rounded-lg border border-gray-100 text-gray-600 italic">
                    {note}
                </div>
            ),
            span: 2
        }] : [])
    ], [customerName, customerPhone, shippingAddress, paymentMethodLabel, note]);

    return (
        <Card className="shadow-sm rounded-xl mb-6 border border-gray-100">
            <Title level={5} className="!mb-6 text-gray-800 border-b border-gray-100 pb-4">
                Customer Information
            </Title>
            <Descriptions 
                layout="vertical"
                column={{ xs: 1, sm: 1, md: 2 }}
                items={infoItems}
                labelStyle={{ paddingBottom: 4 }}
                contentStyle={{ paddingBottom: 24 }}
                colon={false}
            />
        </Card>
    );
});

OrderDetailInfo.displayName = 'OrderDetailInfo';

export default OrderDetailInfo;