import { useState } from 'react';
import { loginAPI } from '../../api/authentication.api';
import { decodeToken, getRolesFromToken } from '../../../../utils/jwtDecode';
import { useAuth } from '../../../../contexts/AuthContext';

const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;

export const useLogin = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  
  const { login: contextLogin } = useAuth();

  const validate = (credentials) => {
    if (!credentials.email || !EMAIL_REGEX.test(credentials.email)) return "Invalid email format";
    if (!credentials.password || !PWD_REGEX.test(credentials.password)) return "Password validation failed...";
    return null;
  };

  const login = async (credentials) => {
    setIsLoading(true);
    setError(null);

    try {
      const validationError = validate(credentials);
      if (validationError) throw new Error(validationError);

      const data = await loginAPI(credentials);

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
      let msg = err?.Error?.Message || err?.message || "An unexpected error occurred";
      setError(msg);
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  return { login, isLoading, error };
};