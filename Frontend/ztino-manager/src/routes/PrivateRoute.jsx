import { Navigate } from "react-router-dom";
import { useAuth } from "../contexts";
import { Spin } from "antd";

const PrivateRoute = ({ children }) => {
    const { isAuthenticated, isInitialized, hasRole } = useAuth();

    if (!isInitialized) {
        return <div className="h-screen flex items-center justify-center"><Spin size="large"/></div>;
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    if (!hasRole("Manager")) {
        return <Navigate to="/login" replace />;
    }

    return children;
}

export default PrivateRoute;