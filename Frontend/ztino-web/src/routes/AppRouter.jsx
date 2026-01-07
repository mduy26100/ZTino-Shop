import { createBrowserRouter, createRoutesFromElements, Route, RouterProvider } from "react-router-dom";
import { 
    ErrorPage, 
    HomePage, 
    ProductListingPage, 
    ProductDetailPage, 
    LoginPage, 
    RegisterPage, 
    CartPage, 
    CheckoutPage, 
    OrderSuccessPage,
    OrderPage,
    OrderDetailPage 
} from "../pages";
import { MainLayout } from "../layouts";
import PublicRoute from "./PublicRoute";
import PrivateRoute from "./PrivateRoute";

const router = createBrowserRouter(
    createRoutesFromElements(
        <>
        <Route path="/" 
            element={<MainLayout />}
            errorElement={<ErrorPage />}
        >

            <Route index element={<HomePage />} />
            
            <Route 
                path="/login" 
                element={
                    <PublicRoute>
                        <LoginPage />
                    </PublicRoute>
                }
            />

            <Route 
                path="/register" 
                element={
                    <PublicRoute>
                        <RegisterPage />
                    </PublicRoute>
                }
            />

            <Route path="products/:slug?" element={<ProductListingPage />} />
            <Route path="product/:slug?" element={<ProductDetailPage />} />
            <Route path="cart" element={<CartPage />} />
            <Route path="checkout" element={<CheckoutPage />} />
            <Route path="order-success" element={<OrderSuccessPage />} />

            <Route 
                path="/orders" 
                element={
                    <PrivateRoute>
                        <OrderPage />
                    </PrivateRoute>
                }
            />

            <Route 
                path="/orders/:orderCode" 
                element={
                    <PrivateRoute>
                        <OrderDetailPage />
                    </PrivateRoute>
                }
            />
        </Route>

        </>
    )
)

const AppRouter = () => {
    return <RouterProvider router={router} />;
}

export default AppRouter;