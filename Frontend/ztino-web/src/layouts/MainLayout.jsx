import React, { useEffect } from 'react';
import { Layout, FloatButton } from 'antd';
import { Outlet, useLocation } from 'react-router-dom';
import { ArrowUpOutlined } from '@ant-design/icons';
import { AppHeader, AppFooter } from './components';

const { Content } = Layout;

const MainLayout = () => {
    const location = useLocation();

    useEffect(() => {
        window.scrollTo(0, 0);
    }, [location.pathname]);

    return (
        <Layout className="min-h-screen flex flex-col bg-white">
            <AppHeader />

            <Content className="flex-grow">
                <div className="w-full">
                    <Outlet />
                </div>
            </Content>

            <AppFooter />

            <FloatButton.BackTop 
                icon={<ArrowUpOutlined />} 
                type="primary" 
                style={{ right: 24, bottom: 24 }}
            />
        </Layout>
    );
};

export default MainLayout;