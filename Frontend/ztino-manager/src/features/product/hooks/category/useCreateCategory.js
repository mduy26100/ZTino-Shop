import { useState, useCallback } from 'react';
import { createCategory } from '../../api/category.api';

export const useCreateCategory = () => {
    const [isLoading, setIsLoading] = useState(false);

    const create = useCallback(async (values, options = {}) => {
        const { onSuccess, onError } = options;
        
        setIsLoading(true);
        try {
            const response = await createCategory(values);
            
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
            setIsLoading(false);
        }
    }, []);

    return { create, isLoading };
};