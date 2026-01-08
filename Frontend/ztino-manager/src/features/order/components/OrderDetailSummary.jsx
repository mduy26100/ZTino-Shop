import React, { memo, useMemo } from 'react';
import { Card, Typography, Divider } from 'antd';

const { Title, Text } = Typography;

const formatCurrency = (amount) => {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
};

const SummaryRow = memo(({ label, value, isTotal = false, isDiscount = false }) => (
    <div className={`flex justify-between items-center ${isTotal ? 'py-2' : 'py-1'}`}>
        <Text className={`${isTotal ? 'text-base font-semibold text-gray-800' : 'text-gray-600'}`}>
            {label}
        </Text>
        <Text 
            className={`
                ${isTotal ? 'text-lg font-bold text-indigo-600' : ''}
                ${isDiscount ? 'text-green-600' : ''}
                ${!isTotal && !isDiscount ? 'text-gray-800' : ''}
            `}
        >
            {isDiscount && value !== '₫0' ? '-' : ''}{value}
        </Text>
    </div>
));

SummaryRow.displayName = 'SummaryRow';

const OrderDetailSummary = memo(({ order }) => {
    const { subTotal, shippingFee, discountAmount, totalAmount } = order || {};

    const formattedValues = useMemo(() => ({
        subTotal: formatCurrency(subTotal || 0),
        shippingFee: formatCurrency(shippingFee || 0),
        discountAmount: formatCurrency(discountAmount || 0),
        totalAmount: formatCurrency(totalAmount || 0)
    }), [subTotal, shippingFee, discountAmount, totalAmount]);

    return (
        <Card className="shadow-sm rounded-xl mb-6">
            <Title level={5} className="!mb-4 text-gray-800">
                Order Summary
            </Title>
            <div className="space-y-1">
                <SummaryRow 
                    label="Subtotal" 
                    value={formattedValues.subTotal} 
                />
                <SummaryRow 
                    label="Shipping Fee" 
                    value={formattedValues.shippingFee} 
                />
                {discountAmount > 0 && (
                    <SummaryRow 
                        label="Discount" 
                        value={formattedValues.discountAmount}
                        isDiscount
                    />
                )}
                <Divider className="!my-3" />
                <SummaryRow 
                    label="Total" 
                    value={formattedValues.totalAmount}
                    isTotal
                />
            </div>
        </Card>
    );
});

OrderDetailSummary.displayName = 'OrderDetailSummary';

export default OrderDetailSummary;
