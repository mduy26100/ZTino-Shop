import React, { useMemo } from 'react';
import { Modal, Spin, Empty, Tag, Image, Typography } from 'antd';
import { useGetProductImages } from '../../hooks/productImages/useGetProductImages';

const { Text } = Typography;

const ProductImageModal = ({ open, onCancel, variantId }) => {
    const { data: images, isLoading } = useGetProductImages(variantId);

    const sortedImages = useMemo(() => {
        if (!images) return [];
        
        return [...images].sort((a, b) => {
            if (a.isMain === b.isMain) {
                return 0; 
            }
            return a.isMain ? -1 : 1;
        });
    }, [images]);

    const renderContent = () => {
        if (isLoading) {
            return (
                <div className="flex h-64 w-full items-center justify-center flex-col gap-3">
                    <Spin size="large" />
                    <Text type="secondary">Loading images...</Text>
                </div>
            );
        }

        if (!sortedImages || sortedImages.length === 0) {
            return (
                <div className="flex h-64 w-full items-center justify-center">
                    <Empty 
                        image={Empty.PRESENTED_IMAGE_SIMPLE} 
                        description="No images found for this variant" 
                    />
                </div>
            );
        }

        return (
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4 p-1">
                <Image.PreviewGroup>
                    {sortedImages.map((img) => (
                        <div 
                            key={img.id} 
                            className={`group relative border rounded-lg overflow-hidden bg-white hover:shadow-md transition-all duration-300 ${
                                img.isMain ? 'border-blue-400 ring-2 ring-blue-100' : 'border-gray-200'
                            }`}
                        >
                            {img.isMain && (
                                <div className="absolute top-2 left-2 z-10">
                                    <Tag color="blue" className="m-0 font-bold shadow-sm text-[10px] px-2 uppercase tracking-wider border-none">
                                        Main Image
                                    </Tag>
                                </div>
                            )}

                            <div className="aspect-square bg-gray-50 flex items-center justify-center overflow-hidden">
                                <Image
                                    src={img.imageUrl}
                                    alt="Variant Image"
                                    className="object-cover w-full h-full transform group-hover:scale-105 transition-transform duration-500"
                                    fallback="https://via.placeholder.com/300?text=Error"
                                    width="100%"
                                    height="100%"
                                />
                            </div>
                            
                            <div className="p-2 bg-white border-t border-gray-100 flex justify-between items-center">
                                <Text type="secondary" className="text-[10px] block truncate">
                                    ID: {img.id}
                                </Text>
                                {img.isMain && <span className="text-[10px] text-blue-500">★ Primary</span>}
                            </div>
                        </div>
                    ))}
                </Image.PreviewGroup>
            </div>
        );
    };

    return (
        <Modal
            title={
                <div className="flex items-center gap-2">
                    <span className="text-lg font-semibold text-slate-700">Variant Images Gallery</span>
                    {!isLoading && sortedImages?.length > 0 && (
                        <Tag className="rounded-full bg-gray-100 border-none text-gray-500">
                            {sortedImages.length}
                        </Tag>
                    )}
                </div>
            }
            open={open}
            onCancel={onCancel}
            footer={null}
            width={800}
            destroyOnHidden
            centered
            styles={{
                mask: { backdropFilter: 'blur(4px)' },
                content: { padding: '24px', borderRadius: '16px', overflow: 'hidden' }
            }}
        >
            <div className="min-h-[200px]">
                {renderContent()}
            </div>
        </Modal>
    );
};

export default React.memo(ProductImageModal);