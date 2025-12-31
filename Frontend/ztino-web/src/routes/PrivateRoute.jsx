import { Navigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import { Spin } from "antd";

const PrivateRoute = ({ children }) => {
    const { isAuthenticated, isInitialized } = useAuth();

    if (!isInitialized) {
        return <div className="h-screen flex items-center justify-center"><Spin size="large"/></div>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return children;
}

export default PrivateRoute;