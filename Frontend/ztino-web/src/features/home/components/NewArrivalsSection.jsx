import React, { useMemo, memo } from 'react';
import { Button, Typography } from 'antd';
import ProductCard from './ProductCard';

const { Title, Text } = Typography;

const NewArrivalsSection = ({ products, onViewAll, onProductClick }) => {
    
    const renderedProducts = useMemo(() => {
        return products.map((product) => (
            <ProductCard 
                key={product.id} 
                product={product} 
                onClick={onProductClick}
            />
        ));
    }, [products, onProductClick]);

    return (
        <section className="py-16 px-6 md:px-12 max-w-[1600px] mx-auto">
            <div className="text-center mb-16">
                <Text className="text-indigo-600 text-xs font-bold uppercase tracking-widest mb-3 block">Fresh Drops</Text>
                <Title level={2} className="!text-3xl md:!text-5xl !font-serif text-slate-900 !m-0 font-normal">New Arrivals</Title>
            </div>

            <div className="grid grid-cols-2 lg:grid-cols-4 gap-x-8 gap-y-16">
                {renderedProducts}
            </div>
            
            <div className="text-center mt-20">
                <Button 
                    size="large" 
                    className="h-12 px-12 rounded-full border border-slate-200 text-slate-800 hover:!border-slate-800 hover:!text-slate-900 hover:shadow-md transition-all duration-300 font-medium text-sm" 
                    onClick={onViewAll}
                >
                    View All Products
                </Button>
            </div>
        </section>
    );
};

export default memo(NewArrivalsSection);