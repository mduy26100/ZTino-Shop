import { useState, useCallback } from 'react';
import { registerAPI } from '../../api/auth.api';

export const useRegister = () => {
    const [isRegistering, setIsRegistering] = useState(false);

    const register = useCallback(async (values, options = {}) => {
        const { onSuccess, onError } = options;
        
        setIsRegistering(true);
        try {
            const response = await registerAPI(values);
            
            if (onSuccess) {
                onSuccess(response);
            }
            
            return response;
        } catch (error) {
            if (onError) {
                onError(error);
            }
            throw error;
        } finally {
            setIsRegistering(false);
        }
    }, []);

    return { register, isRegistering };
};