import React, { createContext, useContext, useState, useEffect, useMemo } from 'react';
import { 
    getToken, getUser, setToken, setUser as setStorageUser, clearAuth, 
} from '../utils/localStorage';
import { isTokenExpired } from '../utils/jwtDecode';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isInitialized, setIsInitialized] = useState(false);

    useEffect(() => {
        const initAuth = () => {
            const token = getToken();
            const savedUser = getUser();

            if (token && savedUser && !isTokenExpired(token)) {
                setUser(savedUser);
            } else {
                handleLogout(); 
            }
            setIsInitialized(true);
        };

        initAuth();
    }, []);

    const handleLogin = (accessToken, userPayload) => {
        setToken(accessToken);
        setStorageUser(userPayload);
        setUser(userPayload);
    };

    const handleLogout = () => {
        clearAuth();
        setUser(null);
    };

    const value = useMemo(() => ({
        user,
        isAuthenticated: !!user,
        isInitialized,
        login: handleLogin,
        logout: handleLogout,
        hasRole: (roleName) => user?.roles?.includes(roleName) || false
    }), [user, isInitialized]);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);