import React from 'react';
import { Table, Tag, Typography, Image, Space, Empty, theme } from 'antd';
import { EyeOutlined } from '@ant-design/icons';

const { Text } = Typography;

const ColorCell = ({ color }) => {
    const { token } = theme.useToken();
    if (!color) return <Text type="secondary">N/A</Text>;

    return (
        <div className="flex items-center gap-2">
            <div 
                className="w-5 h-5 rounded-full shadow-sm ring-1 ring-gray-100 flex-shrink-0"
                style={{ 
                    backgroundColor: color.hex || color.name?.toLowerCase(),
                    border: `1px solid ${token.colorBorderSecondary}`
                }}
                title={color.hex}
            />
            <Text>{color.name}</Text>
        </div>
    );
};

const VariantImages = ({ images = [] }) => {
    if (!images || images.length === 0) return <Text type="secondary" className="text-xs">No images</Text>;

    return (
        <Image.PreviewGroup>
            <Space size="small" wrap>
                {[...images]
                    .sort((a, b) => (a.displayOrder || 0) - (b.displayOrder || 0))
                    .map((img) => (
                        <div key={img.id} className="relative group">
                            <Image
                                width={40}
                                height={40}
                                src={img.imageUrl}
                                className="object-cover rounded border border-gray-100"
                                fallback="https://via.placeholder.com/40"
                                preview={{ mask: <EyeOutlined className="text-sm" /> }}
                            />
                            {img.isMain && (
                                <div className="absolute -top-1 -right-1 z-10">
                                    <Tag color="gold" className="m-0 text-[8px] px-1 py-0 leading-tight border-none shadow-sm">Main</Tag>
                                </div>
                            )}
                        </div>
                    ))}
            </Space>
        </Image.PreviewGroup>
    );
};

const VariantTable = ({ variants = [], productId }) => {
    
    const dataSource = variants.map(v => ({
        key: v.id,
        ...v,
        colorName: v.color?.name,
        sizeName: v.size?.name
    }));

    const columns = [
        {
            title: 'Color',
            dataIndex: 'color',
            key: 'color',
            width: 150,
            render: (color) => <ColorCell color={color} />
        },
        {
            title: 'Size',
            dataIndex: 'size',
            key: 'size',
            width: 100,
            align: 'center',
            render: (size) => size ? <Tag className="min-w-[40px] text-center">{size.name}</Tag> : 'N/A'
        },
        {
            title: 'Price',
            dataIndex: 'price',
            key: 'price',
            width: 150,
            align: 'right',
            render: (price) => (
                <Text strong className="text-emerald-600 font-mono">
                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price ?? 0)}
                </Text>
            )
        },
        {
            title: 'Stock',
            dataIndex: 'stockQuantity',
            key: 'stock',
            width: 100,
            align: 'center',
            render: (stock) => (
                <Tag color={stock > 0 ? "success" : "error"} className="min-w-[50px] text-center font-medium">
                    {stock ?? 0}
                </Tag>
            )
        },
        {
            title: 'Images',
            dataIndex: 'images',
            key: 'images',
            render: (images) => <VariantImages images={images} />
        },
        // Placeholder Action Column for Future Update/Delete
        /*
        {
            title: 'Action',
            key: 'action',
            width: 100,
            align: 'center',
            render: (_, record) => (
                <Button type="text" icon={<EllipsisHorizontalIcon className="w-5 h-5" />} />
            )
        }
        */
    ];

    return (
        <Table
            columns={columns}
            dataSource={dataSource}
            pagination={false}
            rowKey="id"
            locale={{ 
                emptyText: <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="No variants found" /> 
            }}
            scroll={{ x: 800 }}
        />
    );
};

export default React.memo(VariantTable);