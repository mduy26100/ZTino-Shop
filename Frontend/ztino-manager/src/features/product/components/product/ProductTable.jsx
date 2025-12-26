import React, { useMemo } from 'react';
import { Table, Space, Button, Tag, Typography, Tooltip, Image } from 'antd';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';
import { EyeOutlined } from '@ant-design/icons';

const { Text } = Typography;

const ProductTable = ({ 
    dataSource, 
    isLoading, 
    onEdit, 
    onDelete 
}) => {
    
    const columns = useMemo(() => [
        {
            title: 'Product Info',
            dataIndex: 'name',
            key: 'name',
            width: 300,
            render: (text, record) => (
                <div className="flex items-center gap-3">
                    <div className="flex-shrink-0 border border-gray-100 rounded-lg overflow-hidden w-12 h-12">
                        <Image
                            src={record.mainImageUrl}
                            alt={text}
                            width={48}
                            height={48}
                            className="object-cover"
                            fallback="https://via.placeholder.com/48x48?text=No+Img"
                            preview={{ mask: <EyeOutlined /> }}
                        />
                    </div>
                    <div className="flex flex-col">
                        <Text strong className="text-sm line-clamp-1" title={text}>
                            {text}
                        </Text>
                        <Text type="secondary" className="text-xs">
                            Slug: {record.slug}
                        </Text>
                    </div>
                </div>
            ),
        },
        {
            title: 'Category ID',
            dataIndex: 'categoryId',
            key: 'categoryId',
            width: 120,
            align: 'center',
            render: (id) => <Tag color="blue">#{id}</Tag>,
        },
        {
            title: 'Price',
            dataIndex: 'basePrice',
            key: 'basePrice',
            width: 150,
            align: 'right',
            render: (price) => (
                <Text strong className="text-indigo-600">
                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)}
                </Text>
            ),
        },
        {
            title: 'Status',
            dataIndex: 'isActive',
            key: 'isActive',
            width: 120,
            align: 'center',
            render: (isActive) => (
                <Tag color={isActive ? 'success' : 'error'} className="m-0 min-w-[80px] text-center">
                    {isActive ? 'Active' : 'Inactive'}
                </Tag>
            ),
        },
        {
            title: 'Created At',
            dataIndex: 'createdAt',
            key: 'createdAt',
            width: 150,
            render: (date) => (
                <Text type="secondary" className="text-xs">
                    {new Date(date).toLocaleDateString('vi-VN')}
                </Text>
            ),
        },
        {
            title: 'Actions',
            key: 'action',
            width: 120,
            align: 'right',
            render: (_, record) => (
                <Space size="small">
                    <Tooltip title="Edit">
                        <Button
                            type="text"
                            size="small"
                            icon={<PencilSquareIcon className="w-4 h-4 text-indigo-600" />}
                            className="flex items-center justify-center hover:bg-indigo-50"
                            onClick={() => onEdit?.(record)}
                        />
                    </Tooltip>
                    <Tooltip title="Delete">
                        <Button
                            type="text"
                            size="small"
                            danger
                            icon={<TrashIcon className="w-4 h-4" />}
                            className="flex items-center justify-center hover:bg-rose-50"
                            onClick={() => onDelete?.(record)}
                        />
                    </Tooltip>
                </Space>
            ),
        },
    ], [onEdit, onDelete]);

    return (
        <Table
            rowKey="id"
            columns={columns}
            dataSource={dataSource}
            loading={isLoading}
            pagination={{
                pageSize: 10,
                showSizeChanger: true,
                showTotal: (total) => `Total ${total} products`,
            }}
            scroll={{ x: 1000 }}
            className="border border-gray-100 rounded-lg overflow-hidden shadow-sm bg-white"
        />
    );
};

export default React.memo(ProductTable);