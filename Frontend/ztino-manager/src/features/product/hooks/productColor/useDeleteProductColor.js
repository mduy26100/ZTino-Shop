import { useState, useCallback } from 'react';
import { deleteProductColor } from '../../api/productColor.api';

export const useDeleteProductColor = () => {
    const [isDeleting, setIsDeleting] = useState(false);

    const remove = useCallback(async (id, options = {}) => {
        const { onSuccess, onError } = options;

        setIsDeleting(true);
        try {
            const response = await deleteProductColor(id);

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
            setIsDeleting(false);
        }
    }, []);

    return { remove, isDeleting };
};