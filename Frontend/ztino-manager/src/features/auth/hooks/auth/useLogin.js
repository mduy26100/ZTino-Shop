import { useState } from 'react';
import { loginAPI } from '../../api/auth.api';
import { decodeToken, getRolesFromToken } from '../../../../utils/jwtDecode';
import { useAuth } from '../../../../contexts/AuthContext';

export const useLogin = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  
  const { login: contextLogin } = useAuth();

  const isValidEmail = (email) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  };

  const isValidUsername = (username) => {
    if (!username || !username.trim()) return false;
    if (username.length < 3 || username.length > 50) return false;
    const usernameRegex = /^[a-zA-Z0-9_.]+$/; 
    return usernameRegex.test(username);
  };

  const validate = (credentials) => {
    const { identifier, password } = credentials;

    if (!password) return "Password is required.";
    if (password.length < 6) return "Password must be at least 6 characters.";

    if (!identifier) return "Identifier (Email or Username) is required.";

    if (identifier.includes('@')) {
      if (!isValidEmail(identifier)) return "Invalid email format.";
    } else {
      if (!isValidUsername(identifier)) return "Invalid username format (3-50 chars, allowed: letters, numbers, _, .).";
    }

    return null;
  };

  const login = async (values) => {
    setIsLoading(true);
    setError(null);

    try {
      const payload = {
        identifier: values.identifier || values.email || values.username,
        password: values.password
      };

      const validationError = validate(payload);
      if (validationError) throw new Error(validationError);

      const data = await loginAPI(payload);

      if (!data || !data.accessToken) throw new Error("System failed to provide access token");

      const { accessToken } = data;
      const roles = getRolesFromToken(accessToken);

      if (!roles.includes("Manager")) {
        throw new Error("Access denied: Manager privileges required");
      }

      const decoded = decodeToken(accessToken);
      const userPayload = {
        id: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
        email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
        name: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
        roles: roles
      };

      contextLogin(accessToken, userPayload);

      return data;

    } catch (err) {
      let msg = err?.response?.data?.errors 
        ? Object.values(err.response.data.errors).flat().join(', ') 
        : err?.Error?.Message || err?.message || "An unexpected error occurred";
      
      setError(msg);
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  return { login, isLoading, error };
};