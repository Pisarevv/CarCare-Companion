// React's hook for handling side-effects.
import { useEffect } from "react";

// Custom hook to refresh the authentication token.
import useRefreshToken from "./useRefreshToken";

// Custom React Context for user authentication data and methods.
import { useAuthContext } from "../contexts/AuthContext";

// Custom instance of axios configured for making authenticated requests.
import { axiosPrivate } from "../api/axios/axios";

/**
 * useAxiosPrivate Hook
 *
 * This hook is designed to manage authenticated HTTP requests using axios.
 * It intercepts both request and response to add an Authorization header,
 * and handles token refresh on 401 errors.
 *
 * It also cleans up the interceptors when the component unmounts.
 *
 * Returns:
 * - A configured axios instance ready to make authenticated requests.
 */
const useAxiosPrivate = () => {
    // Using the refresh token hook to obtain a new access token if needed.
    const refresh = useRefreshToken();

    // Accessing user details and logout method from the authentication context.
    const { user } = useAuthContext();

    useEffect(() => {

        // Interceptor to modify outgoing requests.
        const requestIntercept = axiosPrivate.interceptors.request.use(
            config => {
                // Add the Authorization header with the user's access token if not present.
                if (!config.headers['Authorization']) {
                    config.headers['Authorization'] = `Bearer ${user?.accessToken}`;
                }
                return config;
            }, (error) => Promise.reject(error)
        );

        // Interceptor to handle responses and errors.
        const responseIntercept = axiosPrivate.interceptors.response.use(
            response => response,
            async (error) => {
                const prevRequest = error?.config;
                // If a 401 error occurred and the request was not previously sent, refresh the token.
                if (error?.response?.status === 401 && !prevRequest?.sent) {
                    prevRequest.sent = true;
                    const newAccessToken = await refresh();
                    prevRequest.headers['Authorization'] = `Bearer ${newAccessToken}`;
                    return axiosPrivate(prevRequest);
                }
                return Promise.reject(error);
            }
        );

        // Cleanup: Eject interceptors when the component unmounts.
        return () => {
            axiosPrivate.interceptors.request.eject(requestIntercept);
            axiosPrivate.interceptors.response.eject(responseIntercept);
        }
    }, [user, refresh])

    return axiosPrivate;
}

export default useAxiosPrivate;
