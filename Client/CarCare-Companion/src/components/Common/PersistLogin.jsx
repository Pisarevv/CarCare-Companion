import { Outlet } from "react-router-dom";
import { useState, useEffect } from "react";
import useRefreshToken from "../../hooks/useRefreshToken";
import { useAuthContext } from "../../contexts/AuthContext";

/**
 * PersistLogin Component
 *
 * This component aims to maintain user sessions across page refreshes.
 * When the component is mounted, it checks the authentication status.
 * If the user is not authenticated, it tries to refresh the token.
 * Only when these operations are completed, the child routes (via Outlet) will be rendered.
 * 
 * Children components (routes) should be wrapped by this component to benefit from 
 * persistent login capabilities.
 */
const PersistLogin = () => {
    // State to track loading status.
    const [isLoading, setIsLoading] = useState(true);

    // Hooks to manage authentication.
    const refresh = useRefreshToken();
    const { isAuthenticated } = useAuthContext();

    useEffect(() => {
        /**
         * verifyRefreshToken Function
         *
         * If the user is not authenticated, this function will try to refresh the token.
         * Regardless of the result (success or failure), the loading state will be set to false.
         */
        const verifyRefreshToken = async () => {
            try {
                await refresh();
            } catch (err) {
                // Handle the error appropriately, e.g., you might want to log the error
                // or show a notification to the user.
            } finally {
                setIsLoading(false);
            }
        }

        // Check if the user is authenticated. If not, attempt to refresh the token.
        !isAuthenticated ? verifyRefreshToken() : setIsLoading(false);
    }, [])

    return (
        <>
            {/* Only render the Outlet (children routes) when not loading */}
            {isLoading ? "" : <Outlet />}
        </>
    )
}

export default PersistLogin;
