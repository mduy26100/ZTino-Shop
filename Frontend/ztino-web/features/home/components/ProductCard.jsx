import React, { useMemo, memo } from 'react';
import { Typography } from 'antd';
import { PlusOutlined } from '@ant-design/icons';

const { Text } = Typography;

const ProductCard = ({ product, onClick }) => {
    const formattedPrice = useMemo(() => {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(product.basePrice);
    }, [product.basePrice]);

    return (
        <div className="group cursor-pointer flex flex-col gap-3" onClick={() => onClick(product.id)}>
            <div className="relative overflow-hidden bg-[#f5f5f7] aspect-[3/4] rounded-2xl shadow-sm hover:shadow-md transition-shadow duration-300">
                <img 
                    src={product.mainImageUrl} 
                    alt={product.name} 
                    className="w-full h-full object-cover transition-transform duration-700 ease-out group-hover:scale-105 mix-blend-multiply" 
                />
                
                {product.tag && (
                    <span className="absolute top-3 left-3 bg-white/90 backdrop-blur-md text-slate-800 text-[10px] font-bold tracking-widest px-3 py-1 rounded-full shadow-sm">
                        {product.tag}
                    </span>
                )}

                <button className="absolute bottom-4 right-4 w-10 h-10 bg-white rounded-full shadow-lg flex items-center justify-center text-slate-800 translate-y-14 opacity-0 group-hover:translate-y-0 group-hover:opacity-100 transition-all duration-500 ease-out hover:bg-slate-900 hover:text-white">
                    <PlusOutlined />
                </button>
            </div>
            
            <div className="px-1">
                <div className="flex flex-col gap-1">
                    <h3 className="text-base font-medium text-slate-800 m-0 group-hover:text-indigo-600 transition-colors font-sans tracking-tight">
                        {product.name}
                    </h3>
                    <Text className="text-sm font-normal text-slate-500">{formattedPrice}</Text>
                </div>
            </div>
        </div>
    );
};

export default memo(ProductCard);