import { useState, useCallback } from 'react';
import { updateCategory } from '../../api/category.api';

export const useUpdateCategory = () => {
    const [isUpdating, setIsUpdating] = useState(false);

    const update = useCallback(async (payload, options = {}) => {
        const { onSuccess, onError } = options;
        
        setIsUpdating(true);
        try {
            const response = await updateCategory(payload);
            
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