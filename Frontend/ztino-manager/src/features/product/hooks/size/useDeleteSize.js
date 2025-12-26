import { useState, useCallback, useRef, useEffect } from 'react';
import { deleteSize } from '../../api/size.api';

export const useDeleteSize = (options = {}) => {
    const { onSuccess, onError } = options;
    const [isDeleting, setIsDeleting] = useState(false);
    const isMounted = useRef(true);

    useEffect(() => {
        isMounted.current = true;
        return () => {
            isMounted.current = false;
        };
    }, []);

    const remove = useCallback(async (id, actionOptions = {}) => {
        if (isMounted.current) setIsDeleting(true);

        const currentOnSuccess = actionOptions.onSuccess || onSuccess;
        const currentOnError = actionOptions.onError || onError;

        try {
            const response = await deleteSize(id);
            
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
            if (isMounted.current) setIsDeleting(false);
        }
    }, [onSuccess, onError]);

    return { remove, isDeleting };
};