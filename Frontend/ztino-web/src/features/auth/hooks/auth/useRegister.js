import { useMutation } from '../../../../hooks/utils';
import { registerAPI } from '../../api/auth.api';

export const useRegister = () => {
    const { mutate, isLoading } = useMutation(registerAPI);
    return { register: mutate, isRegistering: isLoading };
};