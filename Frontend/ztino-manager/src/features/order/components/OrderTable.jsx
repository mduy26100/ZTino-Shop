import React, { memo, useMemo, useCallback } from 'react';
import { Table, Typography, Button, Tooltip, message } from 'antd';
import { EyeOutlined, CopyOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import OrderStatusTag from './OrderStatusTag';
import OrderPaymentStatusTag from './OrderPaymentStatusTag';

const { Text } = Typography;

const formatCurrency = (amount) => {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
};

const formatDate = (dateString) => {
    return dayjs(dateString).format('DD/MM/YYYY HH:mm');
};

const OrderTable = memo(({ orders = [], loading = false }) => {
    const navigate = useNavigate();

    const handleCopyOrderCode = useCallback((orderCode) => {
        navigator.clipboard.writeText(orderCode);
        message.success('Order code copied');
    }, []);

    const handleViewDetails = useCallback((orderCode) => {
        navigate(`/orders/${orderCode}`);
    }, [navigate]);

    const columns = useMemo(() => [
        {
            title: 'Order Code',
            dataIndex: 'orderCode',
            key: 'orderCode',
            width: 200,
            render: (orderCode) => (
                <div className="flex items-center gap-2">
                    <Text 
                        className="font-mono text-indigo-600 font-medium"
                        copyable={false}
                    >
                        {orderCode}
                    </Text>
                    <Tooltip title="Copy">
                        <Button 
                            type="text" 
                            size="small"
                            icon={<CopyOutlined />}
                            onClick={() => handleCopyOrderCode(orderCode)}
                            className="text-gray-400 hover:text-indigo-600"
                        />
                    </Tooltip>
                </div>
            )
        },
        {
            title: 'Product',
            key: 'product',
            width: 200,
            render: (_, record) => {
                const { firstProductName, itemCount } = record;
                const additionalItems = itemCount - 1;

                return (
                    <div>
                        <Text className="text-gray-800" ellipsis={{ tooltip: firstProductName }}>
                            {firstProductName}
                        </Text>
                        {additionalItems > 0 && (
                            <Text className="text-gray-400 text-xs block">
                                +{additionalItems} more items
                            </Text>
                        )}
                    </div>
                );
            }
        },
        {
            title: 'Order Date',
            dataIndex: 'createdAt',
            key: 'createdAt',
            width: 150,
            render: (createdAt) => (
                <Text className="text-gray-600">
                    {formatDate(createdAt)}
                </Text>
            )
        },
        {
            title: 'Total',
            dataIndex: 'totalAmount',
            key: 'totalAmount',
            width: 130,
            render: (totalAmount) => (
                <Text className="text-indigo-600 font-semibold">
                    {formatCurrency(totalAmount)}
                </Text>
            )
        },
        {
            title: 'Status',
            dataIndex: 'status',
            key: 'status',
            width: 130,
            render: (status) => <OrderStatusTag status={status} />
        },
        {
            title: 'Payment',
            dataIndex: 'paymentStatus',
            key: 'paymentStatus',
            width: 140,
            render: (paymentStatus) => <OrderPaymentStatusTag status={paymentStatus} />
        },
        {
            title: 'Action',
            key: 'action',
            width: 100,
            render: (_, record) => (
                <Button
                    type="link"
                    icon={<EyeOutlined />}
                    onClick={() => handleViewDetails(record.orderCode)}
                    className="p-0"
                >
                    Details
                </Button>
            )
        }
    ], [handleCopyOrderCode, handleViewDetails]);

    const dataSource = useMemo(() => {
        return orders.map((order) => ({
            ...order,
            key: order.id
        }));
    }, [orders]);

    return (
        <Table
            columns={columns}
            dataSource={dataSource}
            loading={loading}
            pagination={{
                defaultPageSize: 10,
                showSizeChanger: true,
                pageSizeOptions: ['10', '20', '50'],
                showTotal: (total, range) => 
                    `${range[0]}-${range[1]} of ${total} orders`
            }}
            scroll={{ x: 1000 }}
            className="order-table"
            rowClassName="hover:bg-gray-50"
        />
    );
});

OrderTable.displayName = 'OrderTable';

export default OrderTable;
