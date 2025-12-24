import React from 'react';
import { Table, Tag, Space, Button, Tooltip, Typography } from 'antd';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/outline';

const { Text } = Typography;

const CategoryTable = ({ data, loading, onEdit, onDelete }) => {
    const columns = [
        {
            title: 'Category Name',
            dataIndex: 'name',
            key: 'name',
            width: '40%',
            render: (name) => <Text className="font-semibold text-slate-700">{name}</Text>,
        },
        {
            title: 'Slug',
            dataIndex: 'slug',
            key: 'slug',
            render: (slug) => <span className="text-xs font-mono text-slate-400">{slug}</span>,
        },
        {
            title: 'Status',
            dataIndex: 'isActive',
            key: 'isActive',
            align: 'center',
            render: (isActive) => (
                <Tag color={isActive ? 'success' : 'error'} className="rounded-full border-none px-3 capitalize">
                    {isActive ? 'active' : 'inactive'}
                </Tag>
            ),
        },
        {
            title: 'Actions',
            key: 'actions',
            align: 'right',
            render: (_, record) => (
                <Space size="small">
                    <Tooltip title="Edit">
                        <Button 
                            type="text" 
                            className="flex items-center justify-center hover:!bg-indigo-50 hover:!text-indigo-600"
                            icon={<PencilSquareIcon className="w-5 h-5" />} 
                            onClick={() => onEdit?.(record)}
                        />
                    </Tooltip>
                    <Tooltip title="Delete">
                        <Button 
                            type="text" 
                            danger
                            className="flex items-center justify-center hover:!bg-rose-50"
                            icon={<TrashIcon className="w-5 h-5" />} 
                            onClick={() => onDelete?.(record)}
                        />
                    </Tooltip>
                </Space>
            ),
        },
    ];

    return (
        <Table
            columns={columns}
            dataSource={data}
            loading={loading}
            rowKey="id"
            pagination={false}
            expandable={{
                childrenColumnName: "children",
                expandRowByClick: true,
                rowExpandable: (record) => record.parentId === null,
            }}
            className="category-table shadow-none"
        />
    );
};

export default CategoryTable;