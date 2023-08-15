// React-router import for navigation.
import { useNavigate } from "react-router-dom";

// Context providing authentication-related functions and data.
import { useAuthContext } from "../contexts/AuthContext"

const useDeauthenticate = () => {

   //React-router hook for navigation management
    const navigate = useNavigate();

   //Custom hook import for userLogout function
    const {userLogout} = useAuthContext();

   //Function for removing user credentials from the state and navigating to login
     const logUserOut = (location) => {
        userLogout();
        navigate('/Login', { state: { from: location }, replace: true });
     };
     
     return logUserOut;
}

export default useDeauthenticate;