import React, { useMemo, memo } from 'react';
import { GlobalOutlined, SyncOutlined, SafetyOutlined, CustomerServiceOutlined } from '@ant-design/icons';

const ServicesSection = () => {
    
    const renderedServices = useMemo(() => {
        const items = [
            { icon: <GlobalOutlined />, title: "Worldwide Shipping" },
            { icon: <SyncOutlined />, title: "Easy Returns" },
            { icon: <SafetyOutlined />, title: "Secure Payment" },
            { icon: <CustomerServiceOutlined />, title: "24/7 Support" },
        ];

        return items.map((item, idx) => (
            <div key={idx} className="flex flex-col items-center gap-4 group cursor-default">
                <div className="w-16 h-16 rounded-full bg-white shadow-sm flex items-center justify-center text-2xl text-slate-400 group-hover:text-indigo-600 group-hover:scale-110 transition-all duration-300 mb-2">
                    {item.icon}
                </div>
                <span className="text-xs font-bold uppercase tracking-widest text-slate-700">{item.title}</span>
            </div>
        ));
    }, []);

    return (
        <section className="py-24 border-t border-gray-100 bg-[#fafafa]">
            <div className="max-w-6xl mx-auto px-6">
                <div className="grid grid-cols-2 md:grid-cols-4 gap-12 text-center">
                    {renderedServices}
                </div>
            </div>
        </section>
    );
};

export default memo(ServicesSection);