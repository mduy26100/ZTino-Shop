import React, { useMemo } from 'react';
import { Modal, Descriptions, Tag, Table, Image, Typography, Divider, Space, Empty, theme } from 'antd';
import { EyeOutlined } from '@ant-design/icons';

const { Text } = Typography;

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
                                preview={{
                                    mask: <EyeOutlined className="text-sm" />,
                                }}
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

const ColorCell = ({ color }) => {
    const { token } = theme.useToken();
    
    if (!color) return <Text type="secondary">N/A</Text>;

    const colorStyle = {
        backgroundColor: color.hex || color.name?.toLowerCase(),
        border: `1px solid ${token.colorBorderSecondary}`,
    };

    return (
        <div className="flex items-center gap-2">
            <div 
                className="w-5 h-5 rounded-full shadow-sm ring-1 ring-gray-100 flex-shrink-0"
                style={colorStyle}
                title={color.hex || color.name}
            />
            <Text>{color.name}</Text>
        </div>
    );
};

const ProductDetailModal = ({ open, onCancel, product, loading }) => {
    
    const variantsDataSource = useMemo(() => {
        if (!product?.variants) return [];
        return product.variants.map(v => ({
            key: v.id,
            color: v.color,
            size: v.size?.name || 'N/A',
            price: v.price ?? 0,
            stock: v.stockQuantity ?? 0,
            images: v.images || []
        }));
    }, [product]);

    const variantColumns = [
        {
            title: 'Color',
            dataIndex: 'color',
            key: 'color',
            width: 140,
            render: (color) => <ColorCell color={color} />
        },
        {
            title: 'Size',
            dataIndex: 'size',
            key: 'size',
            width: 80,
            align: 'center',
            render: (text) => <Tag className="min-w-[40px] text-center">{text}</Tag>
        },
        {
            title: 'Price',
            dataIndex: 'price',
            key: 'price',
            width: 120,
            align: 'right',
            render: (price) => (
                <Text strong className="text-emerald-600 font-mono">
                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(price)}
                </Text>
            )
        },
        {
            title: 'Stock',
            dataIndex: 'stock',
            key: 'stock',
            width: 80,
            align: 'center',
            render: (stock) => (
                <Tag color={stock > 0 ? "success" : "error"} className="min-w-[40px] text-center">
                    {stock}
                </Tag>
            )
        },
        {
            title: 'Variant Images',
            dataIndex: 'images',
            key: 'images',
            render: (images) => <VariantImages images={images} />
        }
    ];

    if (!product && !loading) return null;

    return (
        <Modal
            title={
                <div className="flex items-center gap-2">
                    <span className="text-lg font-semibold text-slate-800">Product Details</span>
                    {product?.id && <Tag color="blue" className="rounded-full px-2">#{product.id}</Tag>}
                </div>
            }
            open={open}
            onCancel={onCancel}
            footer={null}
            width={900}
            centered
            destroyOnHidden
            loading={loading}
            styles={{ 
                content: { borderRadius: '16px', overflow: 'hidden' },
                body: { padding: 0 }
            }}
        >
            <div className="p-6 max-h-[80vh] overflow-y-auto">
                <div className="flex flex-col md:flex-row gap-6 mb-6">
                    <div className="w-full md:w-1/3 flex-shrink-0">
                        <div className="aspect-square rounded-xl overflow-hidden border border-gray-100 bg-slate-50 flex items-center justify-center relative shadow-inner">
                            {product?.mainImageUrl ? (
                                <Image
                                    src={product.mainImageUrl}
                                    alt={product.name}
                                    className="object-cover w-full h-full"
                                    fallback="https://via.placeholder.com/300?text=No+Image"
                                />
                            ) : (
                                <Empty description="No Main Image" image={Empty.PRESENTED_IMAGE_SIMPLE} />
                            )}
                        </div>
                    </div>

                    <div className="w-full md:w-2/3">
                        <Descriptions 
                            bordered 
                            column={1} 
                            size="small" 
                            labelStyle={{ width: '120px', fontWeight: 600, backgroundColor: '#f8fafc', color: '#475569' }}
                            contentStyle={{ backgroundColor: '#fff' }}
                        >
                            <Descriptions.Item label="Name">
                                <Text strong className="text-lg text-slate-800">{product?.name || 'Unknown Product'}</Text>
                            </Descriptions.Item>
                            <Descriptions.Item label="Category">
                                {product?.category ? (
                                    <Tag color="purple" className="m-0">{product.category.name}</Tag>
                                ) : <Text type="secondary">Uncategorized</Text>}
                            </Descriptions.Item>
                            <Descriptions.Item label="Base Price">
                                <Text className="text-lg font-mono text-slate-700 font-medium">
                                    {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(product?.basePrice ?? 0)}
                                </Text>
                            </Descriptions.Item>
                            <Descriptions.Item label="Slug">
                                <Text copyable className="text-xs text-gray-500 font-mono">{product?.slug}</Text>
                            </Descriptions.Item>
                            <Descriptions.Item label="Description">
                                <div 
                                    className="prose prose-sm max-h-32 overflow-y-auto text-slate-600"
                                    dangerouslySetInnerHTML={{ __html: product?.description || '<span class="text-gray-400 italic">No description available</span>' }} 
                                />
                            </Descriptions.Item>
                        </Descriptions>
                    </div>
                </div>

                <Divider orientation="left" className="!text-slate-600 !border-slate-200 !text-sm !font-semibold !mt-0">
                    Product Variants ({variantsDataSource.length})
                </Divider>

                <Table 
                    columns={variantColumns}
                    dataSource={variantsDataSource}
                    pagination={false}
                    size="middle"
                    bordered
                    locale={{ emptyText: <Empty description="No variants available" image={Empty.PRESENTED_IMAGE_SIMPLE} /> }}
                    className="shadow-sm border border-gray-100 rounded-lg overflow-hidden"
                    scroll={{ x: 600 }}
                />
            </div>
        </Modal>
    );
};

export default React.memo(ProductDetailModal);