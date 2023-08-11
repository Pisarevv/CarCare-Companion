// Custom instance of axios configured for HTTP requests.
import axios from '../api/axios/axios';

// Custom React Context for user authentication data and methods.
import { useAuthContext } from '../contexts/AuthContext';

/**
 * useRefreshToken Hook
 *
 * This hook provides a method to refresh the user's authentication token.
 * It makes a request to the '/Refresh' endpoint to obtain a new access token 
 * and then updates the user's details in the authentication context.
 *
 * Returns:
 * - A function that can be called to refresh the user's access token.
 */
const useRefreshToken = () => {
    // Accessing the setAuth method from the authentication context to update user details.
    const { setAuth } = useAuthContext();

    /**
     * The refresh function is an asynchronous function that:
     * - Makes a GET request to the '/Refresh' endpoint.
     * - Updates the authentication context with the new access token and other user details.
     * - Returns the new access token.
     */
    const refresh = async () => {
        // Making a GET request to the '/Refresh' endpoint with credentials.
        const response = await axios.get('/Refresh', {
            withCredentials: true
        });

        // Updating the authentication context with the new user details.
        setAuth(prev => {
            return {
                ...prev,
                email: response.data.email,
                role: response.data.role,
                accessToken: response.data.accessToken
            };
        });

        // Returning the new access token.
        return response.data.accessToken;
    }

    return refresh;
};

export default useRefreshToken;
