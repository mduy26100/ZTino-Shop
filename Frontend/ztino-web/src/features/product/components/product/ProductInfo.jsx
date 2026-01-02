import React, { useState, useMemo, useEffect, memo } from 'react';
import { Typography, Button, Divider, InputNumber, Tag, message, Tooltip } from 'antd';
import { ShoppingCartOutlined, HeartOutlined, SyncOutlined } from '@ant-design/icons';

const { Title, Text } = Typography;

const ProductInfo = ({ product, selectedColorId, setSelectedColorId }) => {
    const [selectedSizeId, setSelectedSizeId] = useState(null);
    const [quantity, setQuantity] = useState(1);

    useEffect(() => {
        setSelectedSizeId(null);
        setQuantity(1);
    }, [selectedColorId]);

    const activeGroup = useMemo(() => {
        if (!selectedColorId || !product?.variantGroups) return null;
        return product.variantGroups.find(g => g.colorId === selectedColorId);
    }, [product, selectedColorId]);

    const activeOption = useMemo(() => {
        if (!activeGroup || !selectedSizeId) return null;
        return activeGroup.options.find(o => o.sizeId === selectedSizeId && o.isActive);
    }, [activeGroup, selectedSizeId]);

    const currentPrice = activeOption ? activeOption.price : product.basePrice;
    const currentStock = activeOption ? activeOption.stockQuantity : 0;
    
    const handleAddToCart = () => {
        if (!selectedColorId) {
            message.warning("Please select a color");
            return;
        }
        if (!selectedSizeId) {
            message.warning("Please select a size");
            return;
        }
        if (!activeOption) {
            message.error("This combination is currently unavailable");
            return;
        }
        
        console.log("Add to Cart:", { 
            productId: product.id,
            variantId: activeOption.variantId, 
            quantity 
        });
        message.success("Added to cart successfully!");
    };

    return (
        <div className="flex flex-col h-full sticky top-24">
            <div className="mb-4">
                <Text className="text-gray-400 uppercase tracking-widest text-xs font-bold">
                    {product.category?.name || "Collection"}
                </Text>
                <Title level={1} className="!text-3xl !font-serif !mt-1 !mb-2 text-slate-900">
                    {product.name}
                </Title>
                <div className="flex items-center gap-4">
                    <Title level={3} className="!m-0 !text-indigo-600">
                        {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(currentPrice)}
                    </Title>
                    {activeOption && (
                        <Tag color={currentStock > 0 ? "success" : "error"} className="border-none px-2 py-0.5">
                            {currentStock > 0 ? `In Stock: ${currentStock}` : "Out of Stock"}
                        </Tag>
                    )}
                </div>
            </div>

            <Divider className="my-6" />

            <div className="space-y-6">
                <div>
                    <div className="flex items-center justify-between mb-2">
                        <Text className="font-semibold text-slate-700">
                            Color: <span className="font-normal text-gray-500">
                                {activeGroup?.colorName || "Select a color"}
                            </span>
                        </Text>
                        {selectedColorId && (
                            <Tooltip title="Reset filters">
                                <Button 
                                    type="text" 
                                    size="small" 
                                    icon={<SyncOutlined />} 
                                    onClick={() => setSelectedColorId(null)}
                                    className="text-gray-400 hover:text-slate-800"
                                >
                                    Reset
                                </Button>
                            </Tooltip>
                        )}
                    </div>
                    <div className="flex gap-3 flex-wrap">
                        {product.variantGroups?.map(group => (
                            <button
                                key={group.colorId}
                                onClick={() => setSelectedColorId(group.colorId)}
                                className={`
                                    w-10 h-10 rounded-full border-2 flex items-center justify-center transition-all duration-200
                                    ${selectedColorId === group.colorId 
                                        ? 'border-indigo-600 ring-2 ring-indigo-100 scale-110' 
                                        : 'border-gray-200 hover:border-gray-400'}
                                `}
                                title={group.colorName}
                            >
                                <div 
                                    className="w-8 h-8 rounded-full shadow-sm border border-black/5" 
                                    style={{ backgroundColor: group.colorName.startsWith('#') ? group.colorName : group.colorName.toLowerCase() }} 
                                ></div>
                            </button>
                        ))}
                    </div>
                </div>

                <div>
                    <Text className="block mb-2 font-semibold text-slate-700">
                        Size {selectedColorId && !selectedSizeId && <span className="text-red-500 text-xs font-normal ml-2">* Please select</span>}
                    </Text>
                    <div className="flex flex-wrap gap-3">
                        {activeGroup ? (
                            activeGroup.options
                                .sort((a, b) => a.sizeId - b.sizeId)
                                .map(option => {
                                    const isSelected = selectedSizeId === option.sizeId;
                                    const isDisabled = !option.isActive;

                                    return (
                                        <button
                                            key={option.sizeId}
                                            onClick={() => !isDisabled && setSelectedSizeId(option.sizeId)}
                                            disabled={isDisabled}
                                            className={`
                                                min-w-[48px] h-10 px-3 rounded border text-sm font-medium transition-all
                                                ${isSelected
                                                    ? 'bg-black text-white border-black shadow-md' 
                                                    : isDisabled
                                                        ? 'bg-gray-100 text-gray-300 border-gray-100 cursor-not-allowed opacity-60' 
                                                        : 'bg-white text-slate-700 border-gray-200 hover:border-black hover:shadow-sm'}
                                            `}
                                        >
                                            {option.sizeName}
                                        </button>
                                    );
                                })
                        ) : (
                            <div className="flex gap-3">
                                {[1, 2, 3, 4].map(i => (
                                    <button 
                                        key={i} 
                                        disabled 
                                        className="min-w-[48px] h-10 px-3 rounded border bg-gray-50 border-gray-100 text-gray-300 cursor-not-allowed"
                                    >
                                        --
                                    </button>
                                ))}
                            </div>
                        )}
                    </div>
                    {!selectedColorId && (
                        <Text className="text-xs text-gray-400 mt-1 block">Please select a color to view available sizes</Text>
                    )}
                </div>

                <div>
                    <Text className="block mb-2 font-semibold text-slate-700">Quantity</Text>
                    <InputNumber 
                        min={1} 
                        max={currentStock || 100} 
                        value={quantity} 
                        onChange={setQuantity}
                        disabled={!activeOption}
                        className="w-32 rounded-lg"
                        size="large"
                    />
                </div>
            </div>

            <Divider className="my-8" />

            <div className="flex gap-4">
                <Button 
                    type="primary" 
                    size="large" 
                    icon={<ShoppingCartOutlined />} 
                    className="flex-1 h-14 bg-black hover:!bg-slate-800 rounded-lg text-base font-semibold tracking-wide disabled:bg-gray-200 disabled:text-gray-400 disabled:border-gray-200"
                    onClick={handleAddToCart}
                    disabled={!activeOption || currentStock === 0}
                >
                    {currentStock === 0 && activeOption ? "Out of Stock" : "Add to Cart"}
                </Button>
                <Button 
                    size="large" 
                    icon={<HeartOutlined />} 
                    className="h-14 w-14 flex items-center justify-center rounded-lg border-gray-200 text-slate-600 hover:text-rose-500 hover:border-rose-200 transition-colors"
                />
            </div>

            <div className="mt-8">
                <Text className="block mb-3 font-semibold text-slate-700 text-lg border-b pb-2">Description</Text>
                <div 
                    className="prose prose-slate max-w-none text-gray-600 leading-relaxed"
                    dangerouslySetInnerHTML={{ __html: product.description }} 
                />
            </div>
        </div>
    );
};

export default memo(ProductInfo);