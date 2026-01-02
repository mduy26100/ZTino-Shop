import { createBrowserRouter, createRoutesFromElements, Navigate, Route, RouterProvider } from "react-router-dom";
import { ErrorPage, HomePage, ProductListingPage, ProductDetailPage } from "../pages";
import { MainLayout } from "../layouts";

const router = createBrowserRouter(
    createRoutesFromElements(
        <>
        <Route path="/" 
            element={<MainLayout />}
            errorElement={<ErrorPage />}
        >
            <Route index element={<HomePage />} />
            <Route path="products/:slug?" element={<ProductListingPage />} />
            <Route path="product/:slug?" element={<ProductDetailPage />} />
        </Route>
        </>
    )
)

const AppRouter = () => {
    return <RouterProvider router={router} />;
}

export default AppRouter;