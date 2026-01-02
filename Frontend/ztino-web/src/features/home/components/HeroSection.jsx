import React, { memo } from 'react';
import { Button, Typography, Space } from 'antd';

const { Text } = Typography;

const HeroSection = ({ data, onCtaClick }) => {
    return (
        <section className="relative w-full h-[85vh] bg-[#f0f0f0]">
            <img 
                src={data.image} 
                alt="Hero" 
                className="w-full h-full object-cover object-top"
            />
            
            <div className="absolute inset-0 flex flex-col justify-center items-start px-6 md:px-16 lg:px-24 bg-gradient-to-r from-black/40 to-transparent">
                <div className="max-w-2xl animate-fade-in-up">
                    <Space direction="vertical" size={0}>
                        <Text className="text-white/90 text-xs font-bold uppercase tracking-[0.3em] mb-4 block pl-1">
                            DESIGNED FOR EVERY SEASON
                        </Text>
                        <h1 className="text-5xl md:text-7xl lg:text-8xl font-serif text-white mb-6 leading-tight tracking-tight drop-shadow-lg">
                            {data.title}
                        </h1>
                        <div className="w-20 h-0.5 bg-white mb-8 opacity-70"></div>
                        <p className="text-white/90 text-lg md:text-xl font-light mb-10 max-w-md leading-relaxed drop-shadow-md">
                            {data.subtitle}
                        </p>
                        <Button 
                            size="large" 
                            className="h-14 px-12 bg-white text-black border-none hover:!bg-black hover:!text-white font-semibold uppercase tracking-widest text-xs rounded-full transition-all duration-300 shadow-xl hover:shadow-2xl hover:scale-105"
                            onClick={onCtaClick}
                        >
                            {data.btnText}
                        </Button>
                    </Space>
                </div>
            </div>
        </section>
    );
};

export default memo(HeroSection);