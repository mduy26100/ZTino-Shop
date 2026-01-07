import React, { memo, useMemo, useState, useCallback } from 'react';
import { Typography, Breadcrumb, Alert, message } from 'antd';
import { HomeOutlined, ShoppingCartOutlined } from '@ant-design/icons';
import { Link } from 'react-router-dom';
import { useAuth } from '../../contexts';
import { useGetMyCart, useGetCartById, useUpdateCart, useDeleteCart, CartItemList, CartSummary } from '../../features';
import { getGuestCartId } from '../../utils';

const { Title } = Typography;

const CartPage = memo(() => {
    const [messageApi, contextHolder] = message.useMessage();
    const { isAuthenticated, isInitialized } = useAuth();
    const [selectedItems, setSelectedItems] = useState([]);
    
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
    
    const { totalItems, totalPrice } = useMemo(() => {
        if (!cartItems.length || !selectedItems.length) {
            return { totalItems: 0, totalPrice: 0 };
        }
        
        const selectedCartItems = cartItems.filter(item => 
            selectedItems.includes(item.cartItemId) && item.isAvailable
        );
        
        return {
            totalItems: selectedCartItems.reduce((sum, item) => sum + item.quantity, 0),
            totalPrice: selectedCartItems.reduce((sum, item) => sum + item.itemTotal, 0)
        };
    }, [cartItems, selectedItems]);

    const handleSelectedItemsChange = useCallback((newSelectedItems) => {
        setSelectedItems(newSelectedItems);
    }, []);

    const cartId = useMemo(() => {
        if (isAuthenticated) {
            return data?.cartId || data?.id;
        }
        return guestCartId;
    }, [isAuthenticated, data?.cartId, data?.id, guestCartId]);

    const { update: updateCartItem, isLoading: isUpdating, updatingItemId } = useUpdateCart();

    const handleQuantityChange = useCallback((cartItemId, newQuantity, productVariantId) => {
        const item = cartItems.find(i => i.cartItemId === cartItemId);
        if (!item) return;
        if (newQuantity > item.stockQuantity) {
            messageApi.open({
                type: 'warning',
                content: `Only ${item.stockQuantity} items available in stock`,
            });
            return;
        }

        if (newQuantity < 1) {
            messageApi.open({
                type: 'warning',
                content: 'Quantity must be at least 1',
            });
            return;
        }

        if (!cartId) {
            messageApi.open({
                type: 'error',
                content: 'Cart ID not found',
            });
            return;
        }

        updateCartItem(
            {
                cartId,
                cartItemId,
                productVariantId,
                quantity: newQuantity
            },
            {
                onSuccess: () => {
                    refetch();
                },
                onError: (error) => {
                    messageApi.open({
                        type: 'error',
                        content: error?.error?.message || error?.message || 'Failed to update cart',
                    });
                    refetch();
                }
            }
        );
    }, [cartItems, cartId, updateCartItem, refetch, messageApi]);

    const { remove: removeCartItem, isLoading: isDeleting, deletingItemId } = useDeleteCart();

    const handleRemoveItem = useCallback((cartItemId) => {
        removeCartItem(
            cartItemId,
            {
                onSuccess: () => {
                    setSelectedItems(prev => prev.filter(id => id !== cartItemId));
                    refetch();
                },
                onError: (error) => {
                    messageApi.open({
                        type: 'error',
                        content: error?.error?.message || error?.message || 'Failed to remove item',
                    });
                }
            }
        );
    }, [removeCartItem, refetch, messageApi]);

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
            {contextHolder}
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
                        description={error?.error?.message || error?.message || "An error occurred while loading your cart."}
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
                            updatingItemId={updatingItemId}
                            deletingItemId={deletingItemId}
                            selectedItems={selectedItems}
                            onSelectedItemsChange={handleSelectedItemsChange}
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
                                updatingItemId={updatingItemId}
                                deletingItemId={deletingItemId}
                                selectedItems={selectedItems}
                                onSelectedItemsChange={handleSelectedItemsChange}
                            />
                        </div>

                        <div className="lg:col-span-1 order-1 lg:order-2">
                            <CartSummary
                                totalItems={totalItems}
                                totalPrice={totalPrice}
                                isLoading={isLoading}
                                selectedItems={selectedItems}
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