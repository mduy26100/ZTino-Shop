import { useState, useCallback } from 'react';
import { createProductColor } from '../../api';

export const useCreateProductColor = () => {
    const [isCreating, setIsCreating] = useState(false);

    const create = useCallback(async (payload, options = {}) => {
        const { onSuccess, onError } = options;

        setIsCreating(true);
        try {
            const response = await createProductColor(payload);

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