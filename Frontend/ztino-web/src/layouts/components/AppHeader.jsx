import React, { useState, useMemo, useCallback, memo } from 'react';
import { Layout, Menu, Input, Badge, Button, Dropdown, Space, Avatar, Typography, Drawer, Divider, message } from 'antd';
import { 
    ShoppingCartOutlined, 
    UserOutlined, 
    SearchOutlined, 
    MenuOutlined,
    HeartOutlined,
    LogoutOutlined,
    ShoppingOutlined,
    HomeOutlined,
    PhoneOutlined,
    InfoCircleOutlined,
    LoginOutlined
} from '@ant-design/icons';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useGetCategories } from '../../features';
import { useAuth } from '../../contexts/AuthContext';

const { Header } = Layout;
const { Text } = Typography;

const AppHeader = memo(() => {
    const navigate = useNavigate();
    const location = useLocation();
    const [mobileMenuOpen, setMobileMenuOpen] = useState(false);
    const { user, isAuthenticated, logout, isInitialized } = useAuth();
    
    const { data: categories, isLoading, error } = useGetCategories();

    if (error) {
        console.error("Failed to load menu categories:", error); 
    }

    const handleLogout = useCallback(() => {
        logout();
        message.success('Logged out successfully');
        navigate('/');
    }, [logout, navigate]);

    const selectedKeys = useMemo(() => {
        const pathname = location.pathname;
        
        if (pathname === '/' || pathname === '') {
            return ['home'];
        }
        
        if (pathname === '/about') return ['about'];
        if (pathname === '/contact') return ['contact'];
        
        if (pathname.startsWith('/products')) {
            const slug = pathname.split('/products/')[1];
            
            if (!slug || slug === '') {
                return ['products'];
            }
            
            if (categories && categories.length > 0) {
                for (const rootCat of categories.filter(c => c.parentId === null)) {
                    if (rootCat.slug === slug) {
                        return [`root-${rootCat.id}`];
                    }
                    if (rootCat.children) {
                        for (const child of rootCat.children) {
                            if (child.slug === slug) {
                                return [`cat-${child.id}`];
                            }
                        }
                    }
                }
            }
        }
        
        if (pathname.startsWith('/product/')) {
            return [];
        }
        
        return [];
    }, [location.pathname, categories]);

    const openKeys = useMemo(() => {
        const pathname = location.pathname;
        
        if (!pathname.startsWith('/products') || !categories) return [];
        
        const slug = pathname.split('/products/')[1];
        if (!slug) return [];
        
        for (const rootCat of categories.filter(c => c.parentId === null)) {
            if (rootCat.children) {
                for (const child of rootCat.children) {
                    if (child.slug === slug) {
                        return [`root-${rootCat.id}`];
                    }
                }
            }
        }
        
        return [];
    }, [location.pathname, categories]);

    const menuItems = useMemo(() => {
        const items = [
            { key: 'home', label: <Link to="/">Home</Link>, icon: <HomeOutlined /> },
        ];

        if (categories && categories.length > 0) {
            const categoryMenuItems = categories
                .filter(cat => cat.parentId === null) 
                .map(rootCat => {
                    const childrenItems = rootCat.children?.map(child => ({
                        key: `cat-${child.id}`,
                        label: <Link to={`/products/${child.slug}`}>{child.name}</Link>
                    }));

                    return {
                        key: `root-${rootCat.id}`,
                        label: (
                            <span 
                                onClick={() => {
                                    navigate(`/products/${rootCat.slug}`);
                                    if (mobileMenuOpen) setMobileMenuOpen(false);
                                }}
                                className="cursor-pointer"
                            >
                                {rootCat.name}
                            </span>
                        ),
                        children: childrenItems?.length > 0 ? childrenItems : null, 
                        onClick: !childrenItems || childrenItems.length === 0 ? () => {
                            navigate(`/products/${rootCat.slug}`);
                            if (mobileMenuOpen) setMobileMenuOpen(false);
                        } : undefined 
                    };
                });
            
            items.push(...categoryMenuItems);
        } else {
            items.push({ key: 'products', label: <Link to="/products">Shop</Link>, icon: <ShoppingOutlined /> });
        }

        items.push(
            { key: 'about', label: <Link to="/about">About Us</Link>, icon: <InfoCircleOutlined /> },
            { key: 'contact', label: <Link to="/contact">Contact</Link>, icon: <PhoneOutlined /> }
        );

        return items;
    }, [categories, navigate, mobileMenuOpen]);

    const userMenuItems = useMemo(() => [
        { key: 'profile', label: 'My Profile', icon: <UserOutlined /> },
        { key: 'orders', label: 'My Orders', icon: <ShoppingOutlined />, onClick: () => navigate('/orders') },
        { type: 'divider' },
        { 
            key: 'logout', 
            label: 'Logout', 
            danger: true, 
            icon: <LogoutOutlined />,
            onClick: handleLogout
        },
    ], [handleLogout]);

    const closeMobileMenu = useCallback(() => setMobileMenuOpen(false), []);

    const displayName = useMemo(() => {
        if (!user) return '';
        return user.name || user.email?.split('@')[0] || 'User';
    }, [user]);

    const renderAuthenticatedActions = useCallback((isMobile = false) => {
        if (!isMobile) {
            return (
                <Space size="large" className="flex-shrink-0">
                    <Badge count={2} size="small" offset={[-2, 2]}>
                        <Button shape="circle" icon={<HeartOutlined />} type="text" className="text-gray-600 hover:text-red-500" />
                    </Badge>

                    <Badge count={5} size="small" offset={[-2, 2]}>
                        <Button 
                            shape="circle" 
                            icon={<ShoppingCartOutlined />} 
                            type="text" 
                            className="text-gray-600 hover:text-indigo-600"
                            onClick={() => navigate('/cart')}
                        />
                    </Badge>

                    <Dropdown menu={{ items: userMenuItems }} placement="bottomRight" arrow>
                        <Space className="cursor-pointer hover:bg-gray-50 p-1 pr-2 rounded-full transition-colors">
                            <Avatar icon={<UserOutlined />} className="bg-indigo-100 text-indigo-600" />
                            <Text strong className="hidden lg:block text-sm text-gray-700">
                                Hi, {displayName}
                            </Text>
                        </Space>
                    </Dropdown>
                </Space>
            );
        }

        return (
            <div className="flex flex-col gap-4 mt-4">
                <div className="flex items-center gap-3 p-3 bg-gray-50 rounded-lg">
                    <Avatar size="large" icon={<UserOutlined />} className="bg-indigo-600 text-white" />
                    <div>
                        <Text strong className="block text-base">Hi, {displayName}</Text>
                        <Text type="secondary" className="text-xs">
                            {user?.roles?.includes('Admin') ? 'Admin' : 'Member'}
                        </Text>
                    </div>
                </div>

                <div className="grid grid-cols-2 gap-3">
                    <Button block icon={<HeartOutlined />} className="flex items-center justify-center h-10">
                        Wishlist <Badge count={2} size="small" className="ml-2" color="red" />
                    </Button>
                    <Button block icon={<ShoppingCartOutlined />} onClick={() => { navigate('/cart'); closeMobileMenu(); }} className="flex items-center justify-center h-10">
                        Cart <Badge count={5} size="small" className="ml-2" color="blue" />
                    </Button>
                </div>

                <Divider style={{ margin: '12px 0' }} />
                
                <div className="flex flex-col gap-1">
                    {userMenuItems.map((item, index) => {
                        if (item.type === 'divider') return <Divider key={`div-${index}`} style={{ margin: '4px 0' }} />;
                        return (
                            <Button 
                                key={item.key} 
                                type="text" 
                                danger={item.danger}
                                icon={item.icon}
                                onClick={() => {
                                    if (item.onClick) item.onClick();
                                    closeMobileMenu();
                                }}
                                className="text-left flex items-center justify-start h-10 px-4"
                            >
                                {item.label}
                            </Button>
                        );
                    })}
                </div>
            </div>
        );
    }, [userMenuItems, displayName, user, navigate, closeMobileMenu]);

    const renderGuestActions = useCallback((isMobile = false) => {
        if (!isMobile) {
            return (
                <Space size="middle" className="flex-shrink-0">
                    <Badge count={5} size="small" offset={[-2, 2]}>
                        <Button 
                            shape="circle" 
                            icon={<ShoppingCartOutlined />} 
                            type="text" 
                            className="text-gray-500 hover:text-indigo-600 hover:bg-indigo-50 transition-colors"
                            onClick={() => navigate('/cart')}
                        />
                    </Badge>

                    <Button 
                        type="text"
                        icon={<LoginOutlined />}
                        onClick={() => navigate('/login')}
                        className="border border-indigo-600 text-indigo-600 font-medium hover:!text-indigo-700 hover:!border-indigo-700 hover:!bg-indigo-50"                    >
                        Login
                    </Button>

                    <Button 
                        type="primary"
                        onClick={() => navigate('/register')}
                        className="bg-indigo-600 font-medium hover:!bg-indigo-700 border-none shadow-none"                    >
                        Register
                    </Button>
                </Space>
            );
        }

        return (
            <div className="flex flex-col gap-4 mt-4">
                <div className="flex items-center gap-3 p-4 bg-gradient-to-r from-indigo-50 to-purple-50 rounded-lg border border-indigo-100">
                    <Avatar size="large" icon={<UserOutlined />} className="bg-gray-200 text-gray-500" />
                    <div className="flex-1">
                        <Text strong className="block text-base text-gray-800">Welcome, Guest</Text>
                        <Text type="secondary" className="text-xs">Sign in to access your account</Text>
                    </div>
                </div>

                <div className="grid grid-cols-1 gap-3">
                    <Button 
                        block 
                        type="primary" 
                        icon={<LoginOutlined />}
                        onClick={() => { navigate('/login'); closeMobileMenu(); }}
                        className="h-11 bg-indigo-600 hover:bg-indigo-700 border-none font-medium rounded-lg"
                    >
                        Login
                    </Button>
                    <Button 
                        block 
                        onClick={() => { navigate('/register'); closeMobileMenu(); }}
                        className="h-11 font-medium rounded-lg"
                    >
                        Create Account
                    </Button>
                </div>

                <Divider style={{ margin: '12px 0' }} />

                <Button 
                    block 
                    icon={<ShoppingCartOutlined />} 
                    onClick={() => { navigate('/cart'); closeMobileMenu(); }} 
                    className="flex items-center justify-center h-10"
                >
                    View Cart <Badge count={5} size="small" className="ml-2" color="blue" />
                </Button>
            </div>
        );
    }, [navigate, closeMobileMenu]);

    const renderActions = useCallback((isMobile = false) => {
        if (!isInitialized) {
            return null;
        }
        
        if (isAuthenticated) {
            return renderAuthenticatedActions(isMobile);
        }
        
        return renderGuestActions(isMobile);
    }, [isInitialized, isAuthenticated, renderAuthenticatedActions, renderGuestActions]);

    return (
        <>
            <Header 
                style={{ 
                    position: 'sticky', 
                    top: 0, 
                    zIndex: 1000, 
                    width: '100%', 
                    background: '#fff',
                    boxShadow: '0 2px 8px #f0f1f2',
                    padding: '0 24px',
                    height: 'auto'
                }}
            >
                <div className="max-w-7xl mx-auto h-16 flex items-center justify-between gap-4">
                    
                    <div className="flex-shrink-0 cursor-pointer flex items-center gap-2" onClick={() => navigate('/')}>
                        <div className="w-8 h-8 bg-indigo-600 rounded-lg flex items-center justify-center">
                            <span className="text-white font-bold text-lg">Z</span>
                        </div>
                        <Text strong className="text-xl text-slate-800">ZTino</Text>
                    </div>

                    <div className="flex-1 hidden md:flex justify-center">
                        <Menu 
                            mode="horizontal" 
                            items={menuItems} 
                            disabled={isLoading}
                            selectedKeys={selectedKeys}
                            className="border-none w-full max-w-2xl justify-center text-base font-medium"
                            style={{ background: 'transparent' }}
                        />
                    </div>

                    <div className="hidden lg:block w-64">
                        <Input 
                            placeholder="Search products..." 
                            prefix={<SearchOutlined className="text-gray-400" />} 
                            className="rounded-full bg-gray-50 border-gray-200 hover:bg-white focus:bg-white"
                        />
                    </div>

                    <div className="hidden md:block">
                        {renderActions(false)}
                    </div>

                    <Button 
                        className="md:hidden border-none text-gray-600" 
                        size="large"
                        icon={<MenuOutlined style={{ fontSize: '20px' }} />} 
                        onClick={() => setMobileMenuOpen(true)}
                    />
                </div>
            </Header>

            <Drawer
                title={
                    <div className="flex items-center gap-2">
                        <div className="w-6 h-6 bg-indigo-600 rounded flex items-center justify-center">
                            <span className="text-white font-bold text-xs">Z</span>
                        </div>
                        <Text strong>ZTino Menu</Text>
                    </div>
                }
                placement="right"
                onClose={closeMobileMenu}
                open={mobileMenuOpen}
                width={300}
                styles={{ body: { padding: '16px' } }}
            >
                <div className="flex flex-col h-full">
                    <Input 
                        placeholder="Search products..." 
                        prefix={<SearchOutlined className="text-gray-400" />} 
                        size="large"
                        className="rounded-lg bg-gray-50 border-gray-200 mb-6"
                    />

                    <Text type="secondary" className="text-xs uppercase font-bold tracking-wider mb-2 block">Navigation</Text>
                    
                    <Menu 
                        mode="inline" 
                        items={menuItems} 
                        className="border-none w-full -mx-4 px-2"
                        onClick={(e) => {
                            if (!e.keyPath || e.keyPath.length <= 1) {
                                closeMobileMenu();
                            }
                        }}
                        selectedKeys={selectedKeys}
                        defaultOpenKeys={openKeys}
                    />

                    <Divider />

                    <Text type="secondary" className="text-xs uppercase font-bold tracking-wider mb-2 block">Account</Text>
                    {renderActions(true)}
                </div>
            </Drawer>
        </>
    );
});

AppHeader.displayName = 'AppHeader';

export default AppHeader;