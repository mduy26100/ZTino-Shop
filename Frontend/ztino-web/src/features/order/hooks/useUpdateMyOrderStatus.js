import { useState, useCallback } from 'react';
import { updateMyOrderStatus } from '../api';

export const useUpdateMyOrderStatus = () => {
    const [isUpdating, setIsUpdating] = useState(false);

    const updateStatus = useCallback(async (payload, options = {}) => {
        const { onSuccess, onError } = options;

        setIsUpdating(true);
        try {
            const response = await updateMyOrderStatus(payload);

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
