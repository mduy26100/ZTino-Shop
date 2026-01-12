import { useState, useCallback, useRef, useEffect } from 'react';

export const useMutation = (mutationFn, options = {}) => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [mutatingVariables, setMutatingVariables] = useState(null);
    const isMounted = useRef(true);

    useEffect(() => {
        return () => { isMounted.current = false; };
    }, []);

    const mutate = useCallback(async (variables, callOptions = {}) => {
        const mergedOptions = { ...options, ...callOptions };
        
        setError(null);
        setIsLoading(true);
        setMutatingVariables(variables);

        try {
            mergedOptions.onMutate?.(variables);
            
            const result = await mutationFn(variables);
            
            if (isMounted.current) {
                mergedOptions.onSuccess?.(result, variables);
            }
            return result;
        } catch (err) {
            if (isMounted.current) {
                setError(err);
                mergedOptions.onError?.(err, variables);
            }
            throw err;
        } finally {
            if (isMounted.current) {
                setIsLoading(false);
                setMutatingVariables(null);
                mergedOptions.onSettled?.();
            }
        }
    }, [mutationFn, options]);

    const reset = useCallback(() => {
        setIsLoading(false);
        setError(null);
        setMutatingVariables(null);
    }, []);

    return { mutate, isLoading, error, mutatingVariables, reset };
};
