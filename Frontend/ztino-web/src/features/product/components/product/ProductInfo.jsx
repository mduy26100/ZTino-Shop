import React, { useState, useMemo, useEffect, useCallback, memo } from 'react';
import { Typography, Button, Divider, InputNumber, Tag, message, Tooltip } from 'antd';
import { ShoppingCartOutlined, HeartOutlined, SyncOutlined } from '@ant-design/icons';

const { Title, Text } = Typography;

const ProductInfo = ({ 
    product, 
    selectedProductColorId, 
    setSelectedProductColorId,
    onResetSelection,
    onAddToCart,
    isAddingToCart
}) => {
    const [messageApi, contextHolder] = message.useMessage();
    const [selectedSizeId, setSelectedSizeId] = useState(null);
    const [quantity, setQuantity] = useState(1);

    useEffect(() => {
        setSelectedSizeId(null);
        setQuantity(1);
    }, [selectedProductColorId]);

    const activeProductColor = useMemo(() => {
        if (!selectedProductColorId || !product?.productColors) return null;
        return product.productColors.find(pc => pc.id === selectedProductColorId);
    }, [product?.productColors, selectedProductColorId]);

    const getColorDisplay = useCallback((colorName) => {
        if (!colorName) return '';
        return colorName.startsWith('#') ? colorName : colorName;
    }, []);

    const availableSizes = useMemo(() => {
        if (!activeProductColor?.variants) return [];
        return activeProductColor.variants
            .filter(v => v.isActive)
            .sort((a, b) => (a.size?.id || 0) - (b.size?.id || 0));
    }, [activeProductColor]);

    const activeVariant = useMemo(() => {
        if (!activeProductColor?.variants || !selectedSizeId) return null;
        return activeProductColor.variants.find(
            v => v.size?.id === selectedSizeId && v.isActive
        );
    }, [activeProductColor, selectedSizeId]);

    const currentPrice = activeVariant?.price ?? product?.basePrice ?? 0;
    const currentStock = activeVariant?.stockQuantity ?? 0;

    const isAddToCartEnabled = useMemo(() => {
        return !!(
            selectedProductColorId && 
            selectedSizeId && 
            activeVariant && 
            currentStock > 0 && 
            quantity > 0
        );
    }, [selectedProductColorId, selectedSizeId, activeVariant, currentStock, quantity]);

    const handleColorSelect = useCallback((productColorId) => {
        setSelectedProductColorId(productColorId);
    }, [setSelectedProductColorId]);

    const handleSizeSelect = useCallback((sizeId) => {
        setSelectedSizeId(sizeId);
    }, []);

    const handleReset = useCallback(() => {
        onResetSelection?.();
        setSelectedSizeId(null);
        setQuantity(1);
    }, [onResetSelection]);

    const handleAddToCart = useCallback(() => {
        if (!isAddToCartEnabled) {
            if (!selectedProductColorId) {
                messageApi.open({
                    type: 'warning',
                    content: 'Please select a color',
                });
                return;
            }
            if (!selectedSizeId) {
                messageApi.open({
                    type: 'warning',
                    content: 'Please select a size',
                });
                return;
            }
            return;
        }

        onAddToCart?.({
            productVariantId: activeVariant?.id,
            quantity
        });
    }, [isAddToCartEnabled, selectedProductColorId, selectedSizeId, activeVariant, quantity, onAddToCart, messageApi]);

    const selectedColorName = useMemo(() => {
        if (!activeProductColor) return null;
        return activeProductColor.color?.name || null;
    }, [activeProductColor]);

    return (
        <div className="flex flex-col h-full sticky top-24">
            {contextHolder}
            <div className="mb-4">
                <Text className="text-gray-400 uppercase tracking-widest text-xs font-bold">
                    {product?.category?.name || "Collection"}
                </Text>
                <Title level={1} className="!text-3xl !font-serif !mt-1 !mb-2 text-slate-900">
                    {product?.name}
                </Title>
                <div className="flex items-center gap-4">
                    <Title level={3} className="!m-0 !text-indigo-600">
                        {new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(currentPrice)}
                    </Title>
                    {activeVariant && (
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
                                {selectedColorName 
                                    ? (selectedColorName.startsWith('#') ? selectedColorName : selectedColorName)
                                    : "Select a color"}
                            </span>
                        </Text>
                        {selectedProductColorId && (
                            <Tooltip title="Reset selection">
                                <Button 
                                    type="text"
                                    size="small"
                                    icon={<SyncOutlined />}
                                    onClick={handleReset}
                                    className="
                                        bg-gray-100 
                                        hover:bg-gray-200 
                                        text-gray-500
                                        hover:text-slate-800
                                        rounded-md
                                        px-2
                                        transition
                                    "
                                >
                                    Reset
                                </Button>
                            </Tooltip>

                        )}
                    </div>
                    <div className="flex gap-3 flex-wrap">
                        {product?.productColors?.map(productColor => {
                            const colorValue = getColorDisplay(productColor.color?.name);
                            const isSelected = selectedProductColorId === productColor.id;
                            const hasVariants = productColor.variants?.length > 0;
                            
                            return (
                                <Tooltip 
                                    key={productColor.id} 
                                    title={!hasVariants ? "No variants available" : colorValue}
                                >
                                    <button
                                        onClick={() => handleColorSelect(productColor.id)}
                                        disabled={!hasVariants}
                                        className={`
                                            w-10 h-10 rounded-full border-2 flex items-center justify-center transition-all duration-200
                                            ${isSelected 
                                                ? 'border-indigo-600 ring-2 ring-indigo-100 scale-110' 
                                                : hasVariants
                                                    ? 'border-gray-200 hover:border-gray-400'
                                                    : 'border-gray-100 opacity-40 cursor-not-allowed'}
                                        `}
                                    >
                                        <div 
                                            className="w-8 h-8 rounded-full shadow-sm border border-black/5" 
                                            style={{ 
                                                backgroundColor: colorValue.startsWith('#') 
                                                    ? colorValue 
                                                    : colorValue.toLowerCase() 
                                            }} 
                                        />
                                    </button>
                                </Tooltip>
                            );
                        })}
                    </div>
                </div>

                <div>
                    <Text className="block mb-2 font-semibold text-slate-700">
                        Size 
                        {selectedProductColorId && !selectedSizeId && (
                            <span className="text-red-500 text-xs font-normal ml-2">* Please select</span>
                        )}
                    </Text>
                    <div className="flex flex-wrap gap-3">
                        {selectedProductColorId ? (
                            availableSizes.length > 0 ? (
                                availableSizes.map(variant => {
                                    const isSelected = selectedSizeId === variant.size?.id;
                                    const isOutOfStock = variant.stockQuantity === 0;

                                    return (
                                        <Tooltip 
                                            key={variant.id} 
                                            title={isOutOfStock ? "Out of stock" : `Stock: ${variant.stockQuantity}`}
                                        >
                                            <button
                                                onClick={() => !isOutOfStock && handleSizeSelect(variant.size?.id)}
                                                disabled={isOutOfStock}
                                                className={`
                                                    min-w-[48px] h-10 px-3 rounded border text-sm font-medium transition-all
                                                    ${isSelected
                                                        ? 'bg-black text-white border-black shadow-md' 
                                                        : isOutOfStock
                                                            ? 'bg-gray-100 text-gray-300 border-gray-100 cursor-not-allowed opacity-60 line-through' 
                                                            : 'bg-white text-slate-700 border-gray-200 hover:border-black hover:shadow-sm'}
                                                `}
                                            >
                                                {variant.size?.name || '--'}
                                            </button>
                                        </Tooltip>
                                    );
                                })
                            ) : (
                                <Text className="text-gray-400 text-sm">No sizes available</Text>
                            )
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
                    {!selectedProductColorId && (
                        <Text className="text-xs text-gray-400 mt-1 block">
                            Please select a color to view available sizes
                        </Text>
                    )}
                </div>

                <div>
                    <Text className="block mb-2 font-semibold text-slate-700">Quantity</Text>
                    <InputNumber 
                        min={1} 
                        max={currentStock || 1} 
                        value={quantity} 
                        onChange={(val) => setQuantity(val || 1)}
                        disabled={!activeVariant || currentStock === 0}
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
                    className={`
                        flex-1 h-14 rounded-lg text-base font-semibold tracking-wide
                        ${isAddToCartEnabled 
                            ? 'bg-black hover:!bg-slate-800' 
                            : 'bg-gray-200 text-gray-400 border-gray-200 cursor-not-allowed'}
                    `}
                    onClick={handleAddToCart}
                    disabled={!isAddToCartEnabled || isAddingToCart}
                    loading={isAddingToCart}
                >
                    {!selectedProductColorId ? "Select a Color" 
                        : !selectedSizeId ? "Select a Size" 
                        : currentStock === 0 ? "Out of Stock" 
                        : "Add to Cart"}
                </Button>
                <Button 
                    size="large" 
                    icon={<HeartOutlined />} 
                    className="h-14 w-14 flex items-center justify-center rounded-lg border-gray-200 text-slate-600 hover:text-rose-500 hover:border-rose-200 transition-colors"
                />
            </div>

            <div className="mt-8">
                <Text className="block mb-3 font-semibold text-slate-700 text-lg border-b pb-2">
                    Description
                </Text>
                <div 
                    className="prose prose-slate max-w-none text-gray-600 leading-relaxed"
                    dangerouslySetInnerHTML={{ __html: product?.description || '' }} 
                />
            </div>
        </div>
    );
};

export default memo(ProductInfo);