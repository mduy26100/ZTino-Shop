import MainLayout from "../layouts/MainLayout";
import Login from "../pages/Auth/LoginPage";
import Dashboard from "../pages/Dashboard/Dashboard";
import ErrorPage from "../pages/Error/ErrorPage";
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
        </Route>
        </>
    )
)

const AppRouter = () => {
    return <RouterProvider router={router} />;
}

export default AppRouter;