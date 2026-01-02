import React, { useRef, useMemo, memo } from 'react';
import { Typography } from 'antd';
import { ArrowRightOutlined, ArrowLeftOutlined } from '@ant-design/icons';

const { Text } = Typography;

const CategorySection = ({ categories, onCategoryClick }) => {
    const scrollContainerRef = useRef(null);

    const scroll = (direction) => {
        if (scrollContainerRef.current) {
            const { current } = scrollContainerRef;
            const scrollAmount = 350;
            current.scrollBy({ 
                left: direction === 'left' ? -scrollAmount : scrollAmount, 
                behavior: 'smooth' 
            });
        }
    };

    const renderedCategories = useMemo(() => {
        return categories.map((cat) => (
            <div 
                key={cat.id} 
                className="min-w-[280px] md:min-w-[320px] snap-start group cursor-pointer"
                onClick={() => onCategoryClick(cat.id)}
            >
                <div className="relative h-[420px] overflow-hidden rounded-2xl bg-gray-100 mb-4 shadow-sm group-hover:shadow-lg transition-all duration-500">
                    <img 
                        src={cat.image} 
                        alt={cat.name} 
                        className="w-full h-full object-cover transition-transform duration-1000 ease-out group-hover:scale-110"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-black/50 via-transparent to-transparent opacity-60 group-hover:opacity-80 transition-opacity duration-500"></div>
                    
                    <div className="absolute bottom-6 left-6 right-6">
                        <div className="w-8 h-8 rounded-full bg-white/20 backdrop-blur-md flex items-center justify-center opacity-0 translate-x-4 group-hover:opacity-100 group-hover:translate-x-0 transition-all duration-500 ml-auto">
                            <ArrowRightOutlined className="text-white text-xs"/>
                        </div>
                    </div>
                </div>
                
                <div className="mt-4 px-1">
                    <h3 className="text-lg font-medium text-slate-900 font-serif tracking-wide group-hover:text-indigo-600 transition-colors">
                        {cat.name}
                    </h3>
                </div>
            </div>
        ));
    }, [categories, onCategoryClick]);

    return (
        <section className="py-24 overflow-hidden">
            <div className="container mx-auto px-6 md:px-12 mb-10 flex justify-between items-end">
                <div>
                    <h2 className="text-3xl md:text-4xl font-serif text-slate-900 mb-2">Shop by Category</h2>
                    <Text type="secondary" className="font-light text-slate-500">Essentials for every occasion.</Text>
                </div>
                
                <div className="flex gap-3">
                    <button 
                        onClick={() => scroll('left')}
                        className="w-12 h-12 rounded-full border border-gray-100 bg-white shadow-sm flex items-center justify-center text-slate-600 hover:shadow-md hover:text-black transition-all duration-300"
                    >
                        <ArrowLeftOutlined />
                    </button>
                    <button 
                        onClick={() => scroll('right')}
                        className="w-12 h-12 rounded-full border border-gray-100 bg-white shadow-sm flex items-center justify-center text-slate-600 hover:shadow-md hover:text-black transition-all duration-300"
                    >
                        <ArrowRightOutlined />
                    </button>
                </div>
            </div>

            <div 
                ref={scrollContainerRef}
                className="flex gap-6 overflow-x-auto pb-10 px-6 md:px-12 snap-x snap-mandatory scroll-smooth hide-scrollbar"
                style={{ scrollbarWidth: 'none', msOverflowStyle: 'none' }}
            >
                {renderedCategories}
                <div className="min-w-[20px]"></div>
            </div>
        </section>
    );
};

export default memo(CategorySection);