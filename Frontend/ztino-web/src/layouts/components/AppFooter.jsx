import React from 'react';
import { Layout, Row, Col, Typography, Space, Input, Button, Divider } from 'antd';
import { 
    FacebookFilled, 
    InstagramFilled, 
    TwitterSquareFilled, 
    LinkedinFilled,
    SendOutlined 
} from '@ant-design/icons';

const { Footer } = Layout;
const { Title, Text, Link } = Typography;

const AppFooter = () => {
    return (
        <Footer style={{ background: '#0f172a', color: '#cbd5e1', padding: '64px 24px 24px' }}>
            <div className="max-w-7xl mx-auto">
                <Row gutter={[48, 32]}>
                    <Col xs={24} md={8}>
                        <div className="flex items-center gap-2 mb-4">
                            <div className="w-8 h-8 bg-indigo-600 rounded-lg flex items-center justify-center">
                                <span className="text-white font-bold text-lg">Z</span>
                            </div>
                            <Text strong className="text-2xl text-white">ZTino</Text>
                        </div>
                        <Text className="text-slate-400 block mb-6 leading-relaxed">
                            We provide high-quality products for your modern lifestyle. 
                            Sustainability and customer satisfaction are our top priorities.
                        </Text>
                        <Space size="middle">
                            <Link href="#" className="text-2xl text-slate-400 hover:text-white transition-colors"><FacebookFilled /></Link>
                            <Link href="#" className="text-2xl text-slate-400 hover:text-white transition-colors"><InstagramFilled /></Link>
                            <Link href="#" className="text-2xl text-slate-400 hover:text-white transition-colors"><TwitterSquareFilled /></Link>
                            <Link href="#" className="text-2xl text-slate-400 hover:text-white transition-colors"><LinkedinFilled /></Link>
                        </Space>
                    </Col>

                    <Col xs={12} sm={8} md={4}>
                        <Title level={5} style={{ color: 'white', marginBottom: '24px' }}>Shop</Title>
                        <Space direction="vertical" size="middle">
                            <Link href="#" className="text-slate-400 hover:text-white">All Products</Link>
                            <Link href="#" className="text-slate-400 hover:text-white">Featured</Link>
                            <Link href="#" className="text-slate-400 hover:text-white">New Arrivals</Link>
                            <Link href="#" className="text-slate-400 hover:text-white">Discounts</Link>
                        </Space>
                    </Col>

                    <Col xs={12} sm={8} md={4}>
                        <Title level={5} style={{ color: 'white', marginBottom: '24px' }}>Support</Title>
                        <Space direction="vertical" size="middle">
                            <Link href="#" className="text-slate-400 hover:text-white">FAQ</Link>
                            <Link href="#" className="text-slate-400 hover:text-white">Shipping & Returns</Link>
                            <Link href="#" className="text-slate-400 hover:text-white">Privacy Policy</Link>
                            <Link href="#" className="text-slate-400 hover:text-white">Terms of Service</Link>
                        </Space>
                    </Col>

                    <Col xs={24} sm={16} md={8}>
                        <Title level={5} style={{ color: 'white', marginBottom: '24px' }}>Stay Updated</Title>
                        <Text className="text-slate-400 block mb-4">
                            Subscribe to get special offers, free giveaways, and once-in-a-lifetime deals.
                        </Text>
                        <div className="flex gap-2">
                            <Input 
                                placeholder="Enter your email" 
                                className="rounded-lg bg-slate-800 border-slate-700 text-white placeholder-slate-500 hover:border-indigo-500 focus:border-indigo-500"
                            />
                            <Button type="primary" icon={<SendOutlined />} className="bg-indigo-600 border-none rounded-lg">
                                Join
                            </Button>
                        </div>
                    </Col>
                </Row>

                <Divider style={{ borderColor: '#334155', margin: '48px 0 24px' }} />

                <div className="flex flex-col md:flex-row justify-between items-center gap-4 text-center md:text-left">
                    <Text className="text-slate-500 text-sm">
                        © {new Date().getFullYear()} Shopify Inc. All rights reserved.
                    </Text>
                    <Space size="large" className="text-slate-500 text-sm">
                        <span>Currency: <span className="text-slate-300">VND</span></span>
                        <span>Language: <span className="text-slate-300">English</span></span>
                    </Space>
                </div>
            </div>
        </Footer>
    );
};

export default AppFooter;