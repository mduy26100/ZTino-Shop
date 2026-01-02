import React, { memo } from 'react';
import { Button, Row, Col, Space } from 'antd';

const EditorialSection = () => {
    return (
        <section className="py-24 px-6 md:px-12">
            <div className="max-w-[1600px] mx-auto">
                <Row gutter={[80, 64]} align="middle">
                    <Col xs={24} md={12}>
                        <div className="relative h-[600px] overflow-hidden rounded-[2rem] shadow-2xl shadow-gray-200">
                            <img 
                                src="https://images.unsplash.com/photo-1504198458649-3128b932f49e?q=80&w=1887&auto=format&fit=crop" 
                                alt="Lookbook" 
                                className="w-full h-full object-cover"
                            />
                        </div>
                    </Col>
                    <Col xs={24} md={12}>
                        <div className="md:pl-8">
                            <h2 className="text-4xl md:text-6xl font-serif mb-6 leading-tight text-slate-900">
                                Functional<br/><span className="italic text-slate-500">Minimalism</span>
                            </h2>
                            <p className="text-slate-500 text-lg mb-10 leading-relaxed max-w-md font-light">
                                Our latest drop focuses on utility and clean aesthetics. Designed for the city, built to last.
                                Experience the perfect blend of form and function.
                            </p>
                            <Space size="large">
                                <Button type="link" className="text-slate-900 p-0 h-auto text-sm font-bold uppercase tracking-widest hover:text-indigo-600 transition-colors">
                                    Shop Jackets
                                </Button>
                                <Button type="link" className="text-slate-900 p-0 h-auto text-sm font-bold uppercase tracking-widest hover:text-indigo-600 transition-colors">
                                    Read The Story
                                </Button>
                            </Space>
                        </div>
                    </Col>
                </Row>
            </div>
        </section>
    );
};

export default memo(EditorialSection);