import { useState, useCallback } from 'react';
import { createColor } from '../../api/color.api';

export const useCreateColor = () => {
    const [isCreating, setIsCreating] = useState(false);

    const create = useCallback(async (values, options = {}) => {
        const { onSuccess, onError } = options;
        
        setIsCreating(true);
        try {
            const response = await createColor(values);
            
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
            setIsCreating(false);
        }
    }, []);

    return { create, isCreating };
};