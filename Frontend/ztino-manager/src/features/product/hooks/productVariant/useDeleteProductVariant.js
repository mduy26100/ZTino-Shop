import { useState, useCallback } from 'react';
import { deleteProductVariant } from '../../api';

export const useDeleteProductVariant = () => {
    const [isDeleting, setIsDeleting] = useState(false);

    const remove = useCallback(async (id, options = {}) => {
        const { onSuccess, onError } = options;
        
        setIsDeleting(true);
        try {
            const response = await deleteProductVariant(id);
            
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