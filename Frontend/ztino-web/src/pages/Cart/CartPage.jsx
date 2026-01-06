import React, { memo, useMemo } from 'react';
import { Typography, Breadcrumb, Alert } from 'antd';
import { HomeOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import { useAuth } from '../../contexts';
import { useGetMyCart, useGetCartById, CartItemList, CartSummary } from '../../features';
import { getGuestCartId } from '../../utils';

const { Title } = Typography;

const CartPage = memo(() => {
    const { isAuthenticated, isInitialized } = useAuth();
    
    const guestCartId = useMemo(() => getGuestCartId(), []);
    
    const authenticatedCart = useGetMyCart({ enabled: isAuthenticated });
    const guestCart = useGetCartById(guestCartId, { 
        enabled: !isAuthenticated && !!guestCartId 
    });
    
    const cartData = useMemo(() => {
        if (!isInitialized) return { data: null, isLoading: true, error: null };
        
        if (isAuthenticated) {
            return authenticatedCart;
        }
        
        if (guestCartId) {
            return guestCart;
        }
        
        return { data: null, isLoading: false, error: null };
    }, [isAuthenticated, isInitialized, authenticatedCart, guestCart, guestCartId]);

    const { data, isLoading, error, refetch } = cartData;

    const cartItems = useMemo(() => data?.items || [], [data]);
    const totalPrice = useMemo(() => data?.totalPrice || 0, [data]);
    const totalItems = useMemo(() => data?.totalItems || cartItems.length, [data, cartItems]);

    const handleQuantityChange = (cartItemId, newQuantity) => {
        console.log('Quantity change:', cartItemId, newQuantity);
    };

    const handleRemoveItem = (cartItemId) => {
        console.log('Remove item:', cartItemId);
    };

    const breadcrumbItems = useMemo(() => [
        {
            title: (
                <Link to="/" className="flex items-center gap-1 hover:text-indigo-600">
                    <HomeOutlined />
                    Home
                </Link>
            )
        },
        {
            title: (
                <span className="flex items-center gap-1 text-gray-600">
                    <ShoppingCartOutlined />
                    Shopping Cart
                </span>
            )
        }
    ], []);

    const isCartEmpty = !isLoading && isInitialized && cartItems.length === 0;

    return (
        <div className="bg-gray-50 min-h-screen py-6 sm:py-8">
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <Breadcrumb items={breadcrumbItems} className="mb-4 sm:mb-6" />

                {!isCartEmpty && (
                    <Title level={2} className="!mb-6 sm:!mb-8 !text-gray-800 !text-xl sm:!text-2xl">
                        Shopping Cart
                    </Title>
                )}

                {error && (
                    <Alert
                        message="Failed to load cart"
                        description={error?.message || "An error occurred while loading your cart."}
                        type="error"
                        showIcon
                        className="mb-6"
                        action={
                            <button 
                                onClick={refetch}
                                className="text-indigo-600 hover:text-indigo-800 font-medium"
                            >
                                Retry
                            </button>
                        }
                    />
                )}

                {!isAuthenticated && !guestCartId && !isLoading && isInitialized && (
                    <Alert
                        message="Not logged in"
                        description={
                            <span>
                                <Link to="/login" className="text-indigo-600 hover:text-indigo-800 font-medium">Sign in</Link>
                                {' '}to view your saved cart or continue shopping to add items.
                            </span>
                        }
                        type="info"
                        showIcon
                        className="mb-6"
                    />
                )}

                {isCartEmpty ? (
                    <div className="w-full">
                        <CartItemList 
                            items={cartItems}
                            isLoading={false}
                            onQuantityChange={handleQuantityChange}
                            onRemove={handleRemoveItem}
                        />
                    </div>
                ) : (
                    <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 lg:gap-8">
                        <div className="lg:col-span-2 order-2 lg:order-1">
                            <CartItemList 
                                items={cartItems}
                                isLoading={isLoading || !isInitialized}
                                onQuantityChange={handleQuantityChange}
                                onRemove={handleRemoveItem}
                            />
                        </div>

                        <div className="lg:col-span-1 order-1 lg:order-2">
                            <CartSummary
                                totalItems={totalItems}
                                totalPrice={totalPrice}
                                isLoading={isLoading}
                            />
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
});

CartPage.displayName = 'CartPage';

export default CartPage;
