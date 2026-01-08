import { useState, useCallback } from 'react';
import { updateOrderStatus } from '../api';

export const useUpdateOrderStatus = () => {
    const [isUpdating, setIsUpdating] = useState(false);

    const updateStatus = useCallback(async (payload, options = {}) => {
        const { onSuccess, onError } = options;

        setIsUpdating(true);
        try {
            const response = await updateOrderStatus(payload);

            if (typeof onSuccess === 'function') {
                onSuccess(response);
            }

            return response;
        } catch (error) {
            if (typeof onError === 'function') {
                onError(error);
            }
            throw error;
        } finally {
            setIsUpdating(false);
        }
    }, []);

    return {
        updateStatus,
        isUpdating
    };
};
