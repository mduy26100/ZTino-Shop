import React, { memo, useMemo, useEffect, useCallback } from 'react';
import { Typography, Breadcrumb, Alert, Form, Card, message } from 'antd';
import { HomeOutlined, ShoppingCartOutlined, CreditCardOutlined } from '@ant-design/icons';
import { Link, useSearchParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts';
import { useGetMyCart, useGetCartById } from '../../features/cart';
import { useCreateOrder, CheckoutItemCard, CheckoutForm, CheckoutSummary } from '../../features/order';
import { getGuestCartId } from '../../utils';

const { Title } = Typography;

const CheckoutPage = memo(() => {
    const [messageApi, contextHolder] = message.useMessage();
    const [form] = Form.useForm();
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    
    const { isAuthenticated, isInitialized } = useAuth();
    const guestCartId = useMemo(() => getGuestCartId(), []);

    const selectedIds = useMemo(() => {
        const selectedParam = searchParams.get('selected');
        if (!selectedParam) return [];
        return selectedParam.split(',')
            .map(id => parseInt(id.trim(), 10))
            .filter(id => !isNaN(id));
    }, [searchParams]);

    const authenticatedCart = useGetMyCart({ enabled: isAuthenticated });
    const guestCart = useGetCartById(guestCartId, { 
        enabled: !isAuthenticated && !!guestCartId 
    });

    const cartData = useMemo(() => {
        if (!isInitialized) return { data: null, isLoading: true, error: null };
        if (isAuthenticated) return authenticatedCart;
        if (guestCartId) return guestCart;
        return { data: null, isLoading: false, error: null };
    }, [isAuthenticated, isInitialized, authenticatedCart, guestCart, guestCartId]);

    const { data, isLoading: isCartLoading, error: cartError } = cartData;

    const cartId = useMemo(() => {
        if (isAuthenticated) return data?.cartId || data?.id;
        return guestCartId;
    }, [isAuthenticated, data?.cartId, data?.id, guestCartId]);

    const checkoutItems = useMemo(() => {
        if (!data?.items) return [];
        return data.items.filter(item => 
            selectedIds.includes(item.cartItemId) && item.isAvailable
        );
    }, [data?.items, selectedIds]);

    const { totalItems, totalPrice } = useMemo(() => {
        if (!checkoutItems.length) return { totalItems: 0, totalPrice: 0 };
        return {
            totalItems: checkoutItems.reduce((sum, item) => sum + item.quantity, 0),
            totalPrice: checkoutItems.reduce((sum, item) => sum + item.itemTotal, 0)
        };
    }, [checkoutItems]);

    useEffect(() => {
        if (!isCartLoading && isInitialized && selectedIds.length > 0 && checkoutItems.length > 0) {
            if (checkoutItems.length < selectedIds.length) {
                messageApi.open({
                    type: 'warning',
                    content: 'Some products are no longer available.',
                });
            }
        }
    }, [isCartLoading, isInitialized, selectedIds.length, checkoutItems.length, messageApi]);

    useEffect(() => {
        if (!isCartLoading && isInitialized) {
            if (selectedIds.length === 0) {
                messageApi.open({
                    type: 'error',
                    content: 'Please select items to checkout.',
                });
                navigate('/cart', { replace: true });
            } else if (checkoutItems.length === 0 && data?.items) {
                messageApi.open({
                    type: 'error',
                    content: 'Selected items are no longer available.',
                });
                navigate('/cart', { replace: true });
            }
        }
    }, [isCartLoading, isInitialized, selectedIds.length, checkoutItems.length, data?.items, navigate, messageApi]);

    const { create: createOrder, isLoading: isCreatingOrder } = useCreateOrder();

    const handleFinish = useCallback(async (values) => {
        if (!cartId) {
            messageApi.open({
                type: 'error',
                content: 'Cart not found. Please try again.',
            });
            return;
        }

        const payload = {
            cartId,
            selectedCartItemIds: selectedIds,
            customerName: values.customerName,
            customerPhone: values.customerPhone,
            customerEmail: values.customerEmail || null,
            shippingAddress: {
                recipientName: values.recipientName,
                phoneNumber: values.shippingPhoneNumber,
                street: values.street,
                ward: values.ward,
                district: values.district,
                city: values.city
            },
            note: values.note || null
        };

        createOrder(payload, {
            onSuccess: (response) => {
                messageApi.open({
                    type: 'success',
                    content: 'Order placed successfully!',
                });
                navigate('/order-success', { 
                    state: { 
                        orderId: response?.data?.orderId || response?.orderId,
                        orderData: response?.data || response
                    },
                    replace: true 
                });
            },
            onError: (error) => {
                const errorMessage = error?.error?.message || error?.message || 'Failed to place order.';
                messageApi.open({
                    type: 'error',
                    content: `${errorMessage} Redirecting to cart...`,
                    duration: 5,
                });
                setTimeout(() => navigate('/cart', { replace: true }), 5000);
            }
        });
    }, [cartId, selectedIds, createOrder, navigate, messageApi]);

    const handleSubmit = useCallback(() => {
        form.submit();
    }, [form]);

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
                <Link to="/cart" className="flex items-center gap-1 hover:text-indigo-600">
                    <ShoppingCartOutlined />
                    Cart
                </Link>
            )
        },
        {
            title: (
                <span className="flex items-center gap-1 text-gray-600">
                    <CreditCardOutlined />
                    Checkout
                </span>
            )
        }
    ], []);

    if (isCartLoading || !isInitialized) {
        return (
            <div className="bg-gray-50 min-h-screen py-6 sm:py-8">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <Breadcrumb items={breadcrumbItems} className="mb-4 sm:mb-6" />
                    <div className="flex justify-center items-center py-20">
                        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-indigo-600" />
                    </div>
                </div>
            </div>
        );
    }

    if (cartError) {
        return (
            <div className="bg-gray-50 min-h-screen py-6 sm:py-8">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <Breadcrumb items={breadcrumbItems} className="mb-4 sm:mb-6" />
                    <Alert
                        message="Failed to load checkout data"
                        description={cartError?.error?.message || cartError?.message || "An error occurred."}
                        type="error"
                        showIcon
                    />
                </div>
            </div>
        );
    }

    return (
        <div className="bg-gray-50 min-h-screen py-6 sm:py-8">
            {contextHolder}
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                <Breadcrumb items={breadcrumbItems} className="mb-4 sm:mb-6" />

                <Title level={2} className="!mb-6 sm:!mb-8 !text-gray-800 !text-xl sm:!text-2xl">
                    Checkout
                </Title>

                <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 lg:gap-8">
                    <div className="lg:col-span-2 space-y-6">
                        <Card 
                            className="border-0 shadow-md rounded-xl"
                            styles={{ body: { padding: '24px' } }}
                        >
                            <Form
                                form={form}
                                layout="vertical"
                                onFinish={handleFinish}
                                autoComplete="off"
                            >
                                <CheckoutForm form={form} />
                            </Form>
                        </Card>

                        <Card 
                            className="border-0 shadow-md rounded-xl"
                            styles={{ body: { padding: '24px' } }}
                        >
                            <Title level={5} className="!mb-4 !text-gray-800 flex items-center gap-2">
                                <ShoppingCartOutlined className="text-indigo-600" />
                                Order Items ({checkoutItems.length})
                            </Title>
                            <div className="space-y-3">
                                {checkoutItems.map(item => (
                                    <CheckoutItemCard key={item.cartItemId} item={item} />
                                ))}
                            </div>
                        </Card>
                    </div>

                    <div className="lg:col-span-1">
                        <CheckoutSummary
                            totalItems={totalItems}
                            totalPrice={totalPrice}
                            isLoading={isCreatingOrder}
                            onSubmit={handleSubmit}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
});

CheckoutPage.displayName = 'CheckoutPage';

export default CheckoutPage;