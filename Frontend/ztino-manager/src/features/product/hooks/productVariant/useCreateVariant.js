import { useState, useCallback, useRef, useEffect } from 'react';
import { createProductVariant } from '../../api/productVariant.api';

export const useCreateVariant = (options = {}) => {
    const { onSuccess, onError } = options;
    const [isCreating, setIsCreating] = useState(false);
    const isMounted = useRef(true);

    useEffect(() => {
        isMounted.current = true;
        return () => {
            isMounted.current = false;
        };
    }, []);

    const create = useCallback(async (payload, actionOptions = {}) => {
        if (isMounted.current) setIsCreating(true);

        const currentOnSuccess = actionOptions.onSuccess || onSuccess;
        const currentOnError = actionOptions.onError || onError;

        try {
            const response = await createProductVariant(payload);
            
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
            if (isMounted.current) setIsCreating(false);
        }
    }, [onSuccess, onError]);

    return { create, isCreating };
};