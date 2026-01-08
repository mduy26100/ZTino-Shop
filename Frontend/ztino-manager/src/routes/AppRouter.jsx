import { MainLayout } from "../layouts";
import { 
    LoginPage, 
    Dashboard, 
    ErrorPage, 
    CategoryPage, 
    ColorPage, 
    ProductDetailPage, 
    ProductPage, 
    SizePage, 
    OrderPage,
    OrderDetailPage
} from "../pages";
import { PrivateRoute, PublicRoute } from ".";
import { createBrowserRouter, createRoutesFromElements, Navigate, Route, RouterProvider } from "react-router-dom";

const router = createBrowserRouter(
    createRoutesFromElements(
        <>
        <Route
            path="/login"
            element={
                <PublicRoute>
                    <LoginPage />
                </PublicRoute>
            }
        />

        <Route path="/" 
            element={
                <PrivateRoute>
                    <MainLayout /> 
                </PrivateRoute>
            }
            errorElement={<ErrorPage />}
        >
            <Route index element={<Navigate to="/dashboard" replace />} />
            <Route path="dashboard" element={<Dashboard />} />

            <Route path="products">
                <Route index element={<ProductPage />} />
                <Route path=":id" element={<ProductDetailPage />} />
            </Route>
            
            <Route path="categories" element={<CategoryPage />} />
            <Route path="colors" element={<ColorPage />} />
            <Route path="sizes" element={<SizePage />} />

            <Route path="orders" element={<OrderPage />} />
            <Route path="orders/:orderCode" element={<OrderDetailPage />} />
        </Route>
        </>
    )
)

const AppRouter = () => {
    return <RouterProvider router={router} />;
}

export default AppRouter;