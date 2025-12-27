import MainLayout from "../layouts/MainLayout";
import Login from "../pages/Auth/LoginPage";
import Dashboard from "../pages/Dashboard/Dashboard";
import ErrorPage from "../pages/Error/ErrorPage";
import CategoryPage from "../pages/Product/CategoryPage";
import ColorPage from "../pages/Product/ColorPage";
import ProductDetailPage from "../pages/Product/ProductDetailPage";
import ProductPage from "../pages/Product/ProductPage";
import SizePage from "../pages/Product/SizePage";
import PrivateRoute from "./PrivateRoute";
import PublicRoute from "./PublicRoute";
import { createBrowserRouter, createRoutesFromElements, Navigate, Route, RouterProvider } from "react-router-dom";

const router = createBrowserRouter(
    createRoutesFromElements(
        <>
        <Route
            path="/login"
            element={
                <PublicRoute>
                    <Login />
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
        </Route>
        </>
    )
)

const AppRouter = () => {
    return <RouterProvider router={router} />;
}

export default AppRouter;