import { createBrowserRouter, createRoutesFromElements, Navigate, Route, RouterProvider } from "react-router-dom";
import ErrorPage from "../pages/Error/ErrorPage";
import MainLayout from "../layouts/MainLayout";

const router = createBrowserRouter(
    createRoutesFromElements(
        <>
        <Route path="/" 
            element={<MainLayout />}
            errorElement={<ErrorPage />}
        >
            
        </Route>
        </>
    )
)

const AppRouter = () => {
    return <RouterProvider router={router} />;
}

export default AppRouter;