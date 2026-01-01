import React, { useMemo } from 'react';
import { Table, Tag, Space, Button, Tooltip, Typography, Image } from 'antd';
import { EyeOutlined } from '@ant-design/icons';
import { PencilSquareIcon, TrashIcon, PhotoIcon } from '@heroicons/react/24/outline';

const { Text } = Typography;

const CategoryTable = ({ data = [], loading, onEdit, onDelete }) => {
    
    const columns = useMemo(() => [
        {
            title: 'Image',
            dataIndex: 'imageUrl',
            key: 'imageUrl',
            width: 80,
            align: 'center',
            render: (imageUrl, record) => {
                const isRoot = record.parentId === null;
                
                if (!isRoot) return null;

                if (imageUrl) {
                    return (
                        <Image
                            src={imageUrl}
                            alt={record.name}
                            width={40}
                            height={40}
                            className="object-cover rounded-md border border-gray-200"
                            fallback="https://via.placeholder.com/40?text=No+Img"
                            preview={{ mask: <EyeOutlined className="text-white text-xs" /> }}
                        />
                    );
                }

                return (
                    <div className="w-10 h-10 rounded-md bg-gray-50 border border-gray-200 flex items-center justify-center mx-auto text-gray-300">
                        <PhotoIcon className="w-5 h-5" />
                    </div>
                );
            },
        },
        {
            title: 'Category Name',
            dataIndex: 'name',
            key: 'name',
            width: 250, 
            render: (name) => (
                <Text className="font-semibold text-slate-700 block truncate">
                    {name ?? <span className="italic text-gray-400">N/A</span>}
                </Text>
            ),
        },
        {
            title: 'Slug',
            dataIndex: 'slug',
            key: 'slug',
            width: 200,
            render: (slug) => (
                <span className="text-xs font-mono text-slate-400 block truncate">
                    {slug || '--'}
                </span>
            ),
        },
        {
            title: 'Status',
            dataIndex: 'isActive',
            key: 'isActive',
            align: 'center',
            width: 120,
            render: (isActive) => (
                <Tag 
                    color={isActive ? 'success' : 'error'} 
                    className="rounded-full border-none px-2.5 capitalize text-[11px] font-medium m-0"
                >
                    {isActive ? 'active' : 'inactive'}
                </Tag>
            ),
        },
        {
            title: 'Actions',
            key: 'actions',
            align: 'center',
            width: 100,
            render: (_, record) => {
                if (!record) return null;
                
                return (
                    <Space size={4}>
                        <Tooltip title="Edit">
                            <Button 
                                type="text" 
                                size="small"
                                className="flex items-center justify-center hover:!bg-indigo-50 hover:!text-indigo-600"
                                icon={<PencilSquareIcon className="w-4 h-4" />} 
                                onClick={() => onEdit?.(record)}
                            />
                        </Tooltip>
                        <Tooltip title="Delete">
                            <Button 
                                type="text" 
                                size="small"
                                danger
                                className="flex items-center justify-center hover:!bg-rose-50"
                                icon={<TrashIcon className="w-4 h-4" />} 
                                onClick={() => onDelete?.(record)}
                            />
                        </Tooltip>
                    </Space>
                );
            },
        },
    ], [onEdit, onDelete]);

    return (
        <Table
            columns={columns}
            dataSource={Array.isArray(data) ? data : []}
            loading={loading}
            rowKey={(record) => record?.id || Math.random()}
            pagination={false}
            expandable={{
                childrenColumnName: "children",
                expandRowByClick: true,
                rowExpandable: (record) => record?.parentId === null,
                indentSize: 15,
            }}
            scroll={{ x: 'max-content' }}
            className="category-table shadow-none overflow-hidden"
            size="middle"
            locale={{ emptyText: 'No categories found' }}
        />
    );
};

export default React.memo(CategoryTable);