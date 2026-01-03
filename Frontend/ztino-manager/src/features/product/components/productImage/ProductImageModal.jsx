import React, { useMemo, useState, useRef } from 'react';
import { Modal, Spin, Empty, Tag, Image, Typography, Upload, message, Button, Divider, Tooltip } from 'antd';
import { InboxOutlined, CloudUploadOutlined, DeleteOutlined, EyeOutlined, StarOutlined, StarFilled, SwapOutlined, UndoOutlined, SaveOutlined, ExclamationCircleFilled } from '@ant-design/icons';
import { useGetProductImages } from '../../hooks/productImages/useGetProductImages';
import { useCreateProductImages } from '../../hooks/productImages/useCreateProductImages';
import { useUpdateProductImage } from '../../hooks/productImages/useUpdateProductImage';
import { useDeleteProductImage } from '../../hooks/productImages/useDeleteProductImage';

const { Text } = Typography;
const { Dragger } = Upload;

const ProductImageModal = ({ open, onCancel, productColorId, onSuccess }) => {
    const [messageApi, contextHolder] = message.useMessage();
    const [modal, modalContextHolder] = Modal.useModal();
    
    const [fileList, setFileList] = useState([]);
    const [pendingReplacements, setPendingReplacements] = useState({});

    const hiddenInputRef = useRef(null);
    const currentReplacingId = useRef(null);

    const { data: images, isLoading, refetch } = useGetProductImages(productColorId);
    const { create: createImages, isCreating } = useCreateProductImages();
    const { update: updateImage, isUpdating } = useUpdateProductImage();
    const { remove: deleteImage, isDeleting } = useDeleteProductImage();

    const isProcessing = isCreating || isUpdating || isDeleting;

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
            messageApi.open({
                type: 'error',
                content: `${file.name} is not an image file`,
            });
            return Upload.LIST_IGNORE;
        }
        file.preview = URL.createObjectURL(file);
        setFileList((prev) => [...prev, file]);
        return false;
    };

    const handleRemovePendingFile = (file) => {
        setFileList((prev) => prev.filter((item) => item.uid !== file.uid));
    };

    const triggerReplace = (imageId) => {
        currentReplacingId.current = imageId;
        if (hiddenInputRef.current) {
            hiddenInputRef.current.value = ""; 
            hiddenInputRef.current.click();
        }
    };

    const handleReplaceFileSelected = (e) => {
        const file = e.target.files[0];
        if (!file) return;

        const isImage = file.type.startsWith('image/');
        if (!isImage) {
            messageApi.open({
                type: 'error',
                content: 'Please select an image file',
            });
            return;
        }

        file.preview = URL.createObjectURL(file);

        setPendingReplacements(prev => ({
            ...prev,
            [currentReplacingId.current]: file
        }));
    };

    const handleCancelReplace = (imageId) => {
        setPendingReplacements(prev => {
            const newState = { ...prev };
            delete newState[imageId];
            return newState;
        });
    };

    const handleSaveAll = async () => {
        if (!productColorId) return;
        const newFilesCount = fileList.length;
        const replacementCount = Object.keys(pendingReplacements).length;

        if (newFilesCount === 0 && replacementCount === 0) return;

        try {
            const promises = [];

            if (newFilesCount > 0) {
                const createPayload = {
                    ProductColorId: productColorId,
                    ImageFiles: fileList
                };
                promises.push(createImages(createPayload));
            }

            if (replacementCount > 0) {
                Object.entries(pendingReplacements).forEach(([id, file]) => {
                    const originalImg = images.find(img => img.id === parseInt(id));
                    const updatePayload = {
                        Id: parseInt(id),
                        ProductColorId: productColorId,
                        IsMain: originalImg?.isMain || false,
                        ImageFile: file
                    };
                    promises.push(updateImage(updatePayload));
                });
            }

            await Promise.all(promises);

            let successContent = 'Changes saved successfully';
            
            if (newFilesCount > 0 && replacementCount > 0) {
                successContent = `Successfully uploaded ${newFilesCount} new images and updated ${replacementCount} images`;
            } else if (newFilesCount > 0) {
                successContent = `Successfully uploaded ${newFilesCount} new images`;
            } else if (replacementCount > 0) {
                successContent = `Successfully updated ${replacementCount} images`;
            }

            messageApi.open({
                type: 'success',
                content: successContent,
            });
            
            setFileList([]);
            setPendingReplacements({});
            
            refetch();
            onSuccess?.();

        } catch (error) {
            messageApi.open({
                type: 'error',
                content: error?.error?.message || error?.message || 'Some operations failed. Please try again.',
            });
        }
    };

    const handleSetMainImage = async (img) => {
        if (img.isMain) return;
        
        if (pendingReplacements[img.id]) {
            messageApi.open({
                type: 'warning',
                content: 'Please save the replacement first before setting as main.',
            });
            return;
        }

        await updateImage({
            Id: img.id,
            ProductColorId: productColorId,
            IsMain: true,
        }, {
            onSuccess: () => {
                messageApi.open({
                    type: 'success',
                    content: 'Main image updated',
                });
                refetch();
                onSuccess?.();
            },
            onError: (error) => {
                messageApi.open({
                    type: 'error',
                    content: error?.error?.message || error?.message || 'Failed to set main image',
                });
            }
        });
    };

    const handleDeleteServerImage = (imageId) => {
        if (pendingReplacements[imageId]) {
            handleCancelReplace(imageId);
            return;
        }

        modal.confirm({
            title: 'Delete Image',
            icon: <ExclamationCircleFilled />,
            content: (
                <div className="pt-2">
                    <Text>Are you sure you want to delete this image?</Text>
                    <br />
                    <Text type="secondary" className="text-xs">
                        This action cannot be undone.
                    </Text>
                </div>
            ),
            okText: 'Delete',
            okType: 'danger',
            cancelText: 'Cancel',
            centered: true,
            maskClosable: true,
            onOk: async () => {
                await deleteImage(imageId, {
                    onSuccess: () => {
                        messageApi.open({
                            type: 'success',
                            content: 'Image deleted successfully',
                        });
                        refetch();
                        onSuccess?.();
                    },
                    onError: (error) => {
                        messageApi.open({
                            type: 'error',
                            content: error?.error?.message || error?.message || 'Delete failed',
                        });
                    }
                });
            },
        });
    };

    const handleClose = () => {
        setFileList([]);
        setPendingReplacements({});
        onCancel();
    };

    const renderServerImages = () => {
        if (isLoading) {
            return <div className="flex h-40 w-full items-center justify-center flex-col gap-3"><Spin /><Text type="secondary">Loading...</Text></div>;
        }
        if (!sortedImages?.length) {
            return <Empty image={Empty.PRESENTED_IMAGE_SIMPLE} description="No uploaded images yet" />;
        }

        return (
            <div className="grid grid-cols-3 md:grid-cols-5 gap-3">
                <Image.PreviewGroup>
                    {sortedImages.map((img) => {
                        const pendingFile = pendingReplacements[img.id];
                        
                        return (
                            <div key={img.id} className={`group relative border rounded-lg overflow-hidden bg-white hover:shadow-md transition-all ${img.isMain ? 'border-blue-400 ring-2 ring-blue-100' : 'border-gray-200'}`}>
                                
                                {img.isMain && (
                                    <div className="absolute top-1 left-1 z-20">
                                        <Tag color="blue" className="m-0 text-[10px] border-none shadow-sm">Main</Tag>
                                    </div>
                                )}

                                {pendingFile && (
                                    <div className="absolute top-1 right-1 z-20">
                                        <Tag color="orange" className="m-0 text-[10px] border-none shadow-sm">New File</Tag>
                                    </div>
                                )}

                                <div className="aspect-square flex items-center justify-center overflow-hidden bg-gray-50 relative">
                                    <Image 
                                        src={pendingFile ? pendingFile.preview : img.imageUrl} 
                                        className={`object-cover w-full h-full ${pendingFile ? 'opacity-90' : ''}`} 
                                        width="100%" 
                                        height="100%" 
                                    />
                                    
                                    <div className="absolute inset-0 bg-black/40 flex items-center justify-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity z-10">
                                        
                                        {pendingFile ? (
                                            <Tooltip title="Undo Replace">
                                                <Button 
                                                    shape="circle" 
                                                    icon={<UndoOutlined />} 
                                                    onClick={() => handleCancelReplace(img.id)}
                                                    className="bg-white/90 hover:!bg-white border-none hover:!text-orange-500"
                                                />
                                            </Tooltip>
                                        ) : (
                                            <>
                                                {!img.isMain && (
                                                    <Tooltip title="Set as Main">
                                                        <Button 
                                                            shape="circle" icon={<StarOutlined />} 
                                                            onClick={() => handleSetMainImage(img)}
                                                            className="bg-white/90 hover:!bg-white border-none hover:!text-yellow-500"
                                                        />
                                                    </Tooltip>
                                                )}

                                                <Tooltip title="Replace Image">
                                                    <Button 
                                                        shape="circle" icon={<SwapOutlined />} 
                                                        onClick={() => triggerReplace(img.id)}
                                                        className="bg-white/90 hover:!bg-white border-none hover:!text-blue-500"
                                                    />
                                                </Tooltip>
                                                
                                                <Tooltip title="Delete">
                                                    <Button 
                                                        shape="circle" danger icon={<DeleteOutlined />} 
                                                        onClick={() => handleDeleteServerImage(img.id)}
                                                        className="bg-white/90 hover:!bg-white border-none"
                                                    />
                                                </Tooltip>
                                            </>
                                        )}
                                    </div>
                                </div>

                                <div className="px-2 py-1 bg-white border-t border-gray-100 flex justify-between items-center text-[10px]">
                                    {img.isMain ? (
                                        <span className="text-blue-600 font-medium flex items-center gap-1"><StarFilled /> Primary</span>
                                    ) : (
                                        <span className="text-gray-400">ID: {img.id}</span>
                                    )}
                                </div>
                            </div>
                        );
                    })}
                </Image.PreviewGroup>
            </div>
        );
    };

    const renderPendingFiles = () => {
        if (fileList.length === 0) return null;
        return (
            <div className="mt-4 animate-fade-in">
                <div className="flex justify-between items-center mb-2">
                    <Text strong className="text-orange-600">New Images ({fileList.length})</Text>
                    <Button size="small" danger type="text" onClick={() => setFileList([])}>Clear All</Button>
                </div>
                <div className="grid grid-cols-4 md:grid-cols-6 gap-3">
                    {fileList.map((file) => (
                        <div key={file.uid} className="group relative border border-orange-200 rounded-lg overflow-hidden bg-white shadow-sm hover:shadow-md transition-all">
                            <div className="aspect-square flex items-center justify-center overflow-hidden bg-gray-50">
                                <Image src={file.preview} className="object-cover w-full h-full opacity-80" width="100%" height="100%" preview={{ mask: <div className="flex items-center gap-1 text-white text-xs"><EyeOutlined /> Preview</div> }} />
                            </div>
                            <div className="absolute top-1 right-1 z-10 opacity-0 group-hover:opacity-100 transition-opacity">
                                <Button type="primary" danger shape="circle" size="small" icon={<DeleteOutlined />} onClick={(e) => { e.stopPropagation(); handleRemovePendingFile(file); }} className="shadow-sm" />
                            </div>
                            <div className="px-2 py-1 bg-orange-50 border-t border-orange-100 truncate text-[10px] text-orange-800 text-center">{file.name}</div>
                        </div>
                    ))}
                </div>
            </div>
        );
    };

    const hasChanges = fileList.length > 0 || Object.keys(pendingReplacements).length > 0;

    return (
        <Modal
            title={<div className="flex items-center gap-2"><span className="text-lg font-semibold text-slate-700">Manage Variant Images</span>{!isLoading && sortedImages?.length > 0 && <Tag className="rounded-full bg-gray-100 border-none">{sortedImages.length}</Tag>}</div>}
            open={open} onCancel={handleClose} width={800} destroyOnHidden centered maskClosable={!isProcessing}
            styles={{ mask: { backdropFilter: 'blur(4px)' }, content: { padding: '24px', borderRadius: '16px' } }}
            footer={[
                <Button key="close" onClick={handleClose} disabled={isProcessing}>Close</Button>,
                <Button 
                    key="save" 
                    type="primary" 
                    icon={<SaveOutlined />} 
                    onClick={handleSaveAll} 
                    loading={isProcessing} 
                    disabled={!hasChanges} 
                    className="bg-indigo-600 hover:!bg-indigo-700 border-none"
                >
                    {isProcessing ? 'Saving...' : `Save Changes ${hasChanges ? `(${fileList.length + Object.keys(pendingReplacements).length})` : ''}`}
                </Button>
            ]}
        >
            {contextHolder}
            {modalContextHolder}
            
            <input 
                type="file" 
                ref={hiddenInputRef} 
                style={{ display: 'none' }} 
                accept="image/*"
                onChange={handleReplaceFileSelected}
            />

            <div className="space-y-6">
                <Dragger
                    beforeUpload={handleBeforeUpload} showUploadList={false} multiple={true} accept="image/*" disabled={isProcessing} fileList={fileList}
                    className="!bg-slate-50 !border-slate-200 hover:!border-indigo-400 !rounded-xl overflow-hidden"
                >
                    <p className="ant-upload-drag-icon mb-2"><InboxOutlined className="text-indigo-500 text-3xl" /></p>
                    <p className="ant-upload-text text-sm">Click or drag NEW images here</p>
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