// React's useEffect hook for performing side effects (like API calls) in function components.
import { useEffect } from "react";

// React-router's useLocation to access the current route's location object, 
// and useNavigate for programmatically redirecting users to different routes/pages.
import { useLocation, useNavigate } from "react-router-dom";

// Context providing authentication-related functions and data.
import { useAuthContext } from "../../contexts/AuthContext";

// Custom hook for making authenticated Axios requests.
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from "../Common/IsLoadingHoc";

// Helper utility for handling notifications, useful for providing feedback to users (like success or error messages).
import { NotificationHandler } from "../../utils/NotificationHandler";


const Logout = (props) => {

    // Retrieve the userLogout function from the authentication context 
    // to be used when user needs to be logged out.
    const {userLogout} = useAuthContext();

    // Extract the setLoading function from props to manage loading states.
    const {setLoading} = props;

    // Hook to get a function that lets us programmatically navigate to different routes.
    const navigate = useNavigate();

    // Hook to get the current route's location object. This can be useful to 
    // determine where the user was before getting redirected, among other things.
    const location = useLocation();

    // Instance of Axios for making authenticated API requests.
    const axiosPrivate = useAxiosPrivate();

    // Using the useEffect hook to perform the logout operation when the component mounts.
    useEffect(() => {
        const logoutUser = async () => {
            try {
                // Make a POST request to the logout endpoint to invalidate the user's session or token.
                const response = await axiosPrivate.post('/Logout');
            } catch (err) {
                // If an error occurs during logout, notify the user and redirect them to the root page.
                NotificationHandler(err);
                navigate('/', { state: { from: location }, replace: true });
            }
            finally {
                // Regardless of success or failure, logout the user from the client side,
                // redirect them to the root page and stop showing the loading state.
                userLogout();
                navigate("/");
                setLoading(false);
            }
        }

        // Invoke the logout function immediately.
        logoutUser();
    
    }, [])  // Empty dependency array ensures the effect runs only once when the component mounts.

    // Since this component's main task is to perform the logout operation, 
    // it doesn't render any UI elements and thus returns null.
    return null;
}

// Wrap the Logout component with the IsLoadingHOC to show a loading state while the logout operation is in progress.
export default IsLoadingHOC(Logout);
