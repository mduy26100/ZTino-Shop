import { useState, useCallback } from 'react';
import { updateProductVariant } from '../../api';

export const useUpdateProductVariant = () => {
    const [isUpdating, setIsUpdating] = useState(false);

    const update = useCallback(async (payload, options = {}) => {
        const { onSuccess, onError } = options;
        
        setIsUpdating(true);
        try {
            const response = await updateProductVariant(payload);
            
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
            setIsUpdating(false);
        }
    }, []);

    return { update, isUpdating };
};