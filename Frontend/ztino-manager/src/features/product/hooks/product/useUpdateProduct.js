import { useState, useCallback, useRef, useEffect } from 'react';
import { updateProduct } from '../../api';

export const useUpdateProduct = (options = {}) => {
    const { onSuccess, onError } = options;
    const [isUpdating, setIsUpdating] = useState(false);
    const isMounted = useRef(true);

    useEffect(() => {
        isMounted.current = true;
        return () => {
            isMounted.current = false;
        };
    }, []);

    const update = useCallback(async (payload, actionOptions = {}) => {
        if (isMounted.current) setIsUpdating(true);

        const currentOnSuccess = actionOptions.onSuccess || onSuccess;
        const currentOnError = actionOptions.onError || onError;

        try {
            const response = await updateProduct(payload);
            
            if (isMounted.current) {
                currentOnSuccess?.(response);
            }
            
            return response;
        } catch (error) {
            if (isMounted.current) {
                currentOnError?.(error);
            }
            throw error;
        } finally {
            if (isMounted.current) setIsUpdating(false);
        }
    }, [onSuccess, onError]);

    return { update, isUpdating };
};