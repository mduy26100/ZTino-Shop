import React, { memo } from 'react';
import { Typography, Button, Tooltip, Tag } from 'antd';
import { ShoppingCartOutlined, HeartOutlined, EyeOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

const { Text } = Typography;

const ProductCard = ({ product }) => {
    const navigate = useNavigate();

    if (!product) return null;

    const formattedPrice = new Intl.NumberFormat('vi-VN', { 
        style: 'currency', 
        currency: 'VND' 
    }).format(product.basePrice);

    const handleCardClick = () => {
        navigate(`/product/${product.slug}`);
    };

    const handleAddToCart = (e) => {
        e.stopPropagation();
    };

    const handleWishlist = (e) => {
        e.stopPropagation();
    };

    return (
        <div 
            className="group relative bg-white rounded-none overflow-hidden hover:shadow-2xl transition-all duration-500 cursor-pointer h-full flex flex-col"
            onClick={handleCardClick}
        >
            <div className="relative aspect-[4/5] overflow-hidden bg-gray-50">
                <img 
                    src={product.mainImageUrl || "https://via.placeholder.com/300x400?text=No+Image"} 
                    alt={product.name}
                    className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-105"
                    loading="lazy"
                />

                <div className="absolute top-2 left-2 flex flex-col gap-1">
                    {!product.isActive && <Tag color="default" className="text-[10px] uppercase tracking-wider border-none bg-black text-white rounded-sm px-2">Sold Out</Tag>}
                    {product.id > 10 && <Tag className="text-[10px] uppercase tracking-wider border-none bg-white/90 backdrop-blur text-slate-800 rounded-sm px-2">New</Tag>}
                </div>

                <div className="absolute bottom-4 left-0 right-0 flex justify-center gap-3 opacity-0 translate-y-4 group-hover:opacity-100 group-hover:translate-y-0 transition-all duration-300">
                    <Tooltip title="Add to Cart">
                        <Button 
                            shape="circle" 
                            icon={<ShoppingCartOutlined />} 
                            className="bg-white border-none text-slate-800 hover:bg-black hover:text-white shadow-lg"
                            onClick={handleAddToCart}
                        />
                    </Tooltip>
                    <Tooltip title="Wishlist">
                        <Button 
                            shape="circle" 
                            icon={<HeartOutlined />} 
                            className="bg-white border-none text-slate-800 hover:bg-rose-500 hover:text-white shadow-lg"
                            onClick={handleWishlist}
                        />
                    </Tooltip>
                </div>
            </div>

            <div className="pt-3 pb-1 flex flex-col flex-grow text-center">
                <Text className="text-[10px] text-gray-400 uppercase tracking-widest mb-1">
                    {product.categoryName || "Collection"}
                </Text>

                <h3 
                    className="text-sm font-medium text-slate-800 mb-1 line-clamp-1 group-hover:text-indigo-600 transition-colors px-2"
                    title={product.name}
                >
                    {product.name}
                </h3>

                <Text className="text-sm font-semibold text-slate-900 mt-auto">
                    {formattedPrice}
                </Text>
            </div>
        </div>
    );
};

export default memo(ProductCard);