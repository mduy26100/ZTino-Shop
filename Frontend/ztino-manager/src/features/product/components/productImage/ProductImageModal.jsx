import React, { useMemo, useState } from 'react';
import { Modal, Spin, Empty, Tag, Image, Typography, Upload, message, Button, Divider } from 'antd';
import { InboxOutlined, CloudUploadOutlined, DeleteOutlined, EyeOutlined } from '@ant-design/icons';
import { useGetProductImages } from '../../hooks/productImages/useGetProductImages';
import { useCreateProductImages } from '../../hooks/productImages/useCreateProductImages';

const { Text } = Typography;
const { Dragger } = Upload;

const ProductImageModal = ({ open, onCancel, variantId, onSuccess }) => {
    const [messageApi, contextHolder] = message.useMessage();
    
    const [fileList, setFileList] = useState([]);

    const { data: images, isLoading, refetch } = useGetProductImages(variantId);
    const { create: createImages, isCreating: isUploading } = useCreateProductImages();

    const sortedImages = useMemo(() => {
        if (!images) return [];
        return [...images].sort((a, b) => {
            if (a.isMain === b.isMain) return 0;
            return a.isMain ? -1 : 1;
        });
    }, [images]);

    const handleBeforeUpload = (file) => {
        const isImage = file.type.startsWith('image/');
        if (!isImage) {
            messageApi.error(`${file.name} is not an image file`);
            return Upload.LIST_IGNORE;
        }
        file.preview = URL.createObjectURL(file);
        setFileList((prev) => [...prev, file]);
        return false;
    };

    const handleRemovePendingFile = (file) => {
        setFileList((prev) => prev.filter((item) => item.uid !== file.uid));
    };

    const handleBatchUpload = async () => {
        if (fileList.length === 0 || !variantId) return;

        try {
            const payload = {
                ProductVariantId: variantId,
                ImageFiles: fileList 
            };

            await createImages(payload);

            messageApi.success(`Uploaded ${fileList.length} images successfully`);
            setFileList([]); 
            refetch(); 
            onSuccess?.(); 
        } catch (err) {
            messageApi.error("Upload failed");
        }
    };

    const handleClose = () => {
        setFileList([]);
        onCancel();
    };

    const renderServerImages = () => {
        if (isLoading) {
            return <div className="flex h-40 items-center justify-center gap-2"><Spin /><Text type="secondary">Loading...</Text></div>;
        }
        if (!sortedImages?.length) {
            return <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="No uploaded images yet" />;
        }

        return (
            <div className="grid grid-cols-3 md:grid-cols-5 gap-3">
                <Image.PreviewGroup>
                    {sortedImages.map((img) => (
                        <div key={img.id} className={`group relative border rounded-lg overflow-hidden bg-white hover:shadow-md transition-all ${img.isMain ? 'border-blue-400 ring-2 ring-blue-100' : 'border-gray-200'}`}>
                            {img.isMain && <div className="absolute top-1 left-1 z-10"><Tag color="blue" className="m-0 text-[10px] border-none">Main</Tag></div>}
                            <div className="aspect-square flex items-center justify-center overflow-hidden bg-gray-50">
                                <Image 
                                    src={img.imageUrl} 
                                    className="object-cover w-full h-full" 
                                    width="100%" 
                                    height="100%"
                                />
                            </div>
                        </div>
                    ))}
                </Image.PreviewGroup>
            </div>
        );
    };

    const renderPendingFiles = () => {
        if (fileList.length === 0) return null;

        return (
            <div className="mt-4 animate-fade-in">
                <div className="flex justify-between items-center mb-2">
                    <Text strong className="text-orange-600">Pending Upload ({fileList.length})</Text>
                    <Button size="small" danger type="text" onClick={() => setFileList([])}>Clear All</Button>
                </div>
                
                <Image.PreviewGroup>
                    <div className="grid grid-cols-4 md:grid-cols-6 gap-3">
                        {fileList.map((file) => (
                            <div key={file.uid} className="group relative border border-orange-200 rounded-lg overflow-hidden bg-white shadow-sm hover:shadow-md transition-all">
                                <div className="aspect-square flex items-center justify-center overflow-hidden bg-gray-50">
                                    <Image
                                        src={file.preview}
                                        className="object-cover w-full h-full"
                                        width="100%"
                                        height="100%"
                                        preview={{
                                            mask: (
                                                <div className="flex items-center gap-1 text-white text-xs">
                                                    <EyeOutlined /> Preview
                                                </div>
                                            )
                                        }}
                                    />
                                </div>

                                <div className="absolute top-1 right-1 z-10 opacity-0 group-hover:opacity-100 transition-opacity">
                                    <Button 
                                        type="primary" 
                                        danger 
                                        shape="circle" 
                                        size="small" 
                                        icon={<DeleteOutlined />} 
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            handleRemovePendingFile(file);
                                        }}
                                        className="shadow-sm"
                                    />
                                </div>
                                
                                <div className="px-2 py-1 bg-orange-50 border-t border-orange-100 truncate text-[10px] text-orange-800 text-center">
                                    {file.name}
                                </div>
                            </div>
                        ))}
                    </div>
                </Image.PreviewGroup>
            </div>
        );
    };

    return (
        <Modal
            title={
                <div className="flex items-center gap-2">
                    <span className="text-lg font-semibold text-slate-700">Manage Variant Images</span>
                    {!isLoading && sortedImages?.length > 0 && <Tag className="rounded-full bg-gray-100 border-none">{sortedImages.length}</Tag>}
                </div>
            }
            open={open}
            onCancel={handleClose}
            width={800}
            destroyOnHidden
            centered
            maskClosable={!isUploading}
            styles={{
                mask: { backdropFilter: 'blur(4px)' },
                content: { padding: '24px', borderRadius: '16px' }
            }}
            footer={[
                <Button key="close" onClick={handleClose} disabled={isUploading}>Close</Button>,
                <Button 
                    key="upload" 
                    type="primary" 
                    icon={<CloudUploadOutlined />} 
                    onClick={handleBatchUpload} 
                    loading={isUploading}
                    disabled={fileList.length === 0} 
                    className="bg-indigo-600 hover:!bg-indigo-700 border-none"
                >
                    {isUploading ? 'Uploading...' : `Upload ${fileList.length > 0 ? `(${fileList.length})` : ''}`}
                </Button>
            ]}
        >
            {contextHolder}
            
            <div className="space-y-6">
                <Dragger
                    beforeUpload={handleBeforeUpload}
                    showUploadList={false}
                    multiple={true}
                    accept="image/*"
                    disabled={isUploading}
                    fileList={fileList}
                    className="!bg-slate-50 !border-slate-200 hover:!border-indigo-400 !rounded-xl overflow-hidden"
                >
                    <p className="ant-upload-drag-icon mb-2"><InboxOutlined className="text-indigo-500 text-3xl" /></p>
                    <p className="ant-upload-text text-sm">Click or drag images here</p>
                </Dragger>

                {renderPendingFiles()}

                {fileList.length > 0 && <Divider dashed />}

                <div>
                    <Text strong className="text-slate-600 mb-2 block">Current Images</Text>
                    <div className="min-h-[150px] bg-gray-50 rounded-lg p-4 border border-gray-100">
                        {renderServerImages()}
                    </div>
                </div>
            </div>
        </Modal>
    );
};

export default React.memo(ProductImageModal);