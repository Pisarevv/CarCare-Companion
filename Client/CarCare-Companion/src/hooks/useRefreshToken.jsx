import axios from '../api/axios/axios';
import { useAuthContext } from '../contexts/AuthContext';

const useRefreshToken = () => {
    const { setAuth } = useAuthContext();

    const refresh = async () => {
        const response = await axios.get('/Refresh', {
            withCredentials: true
        });
        setAuth(prev => {
            return { ...prev,email: response.data.email, accessToken: response.data.accessToken }
        });
        return response.data.accessToken;
    }
    return refresh;
};

export default useRefreshToken;
