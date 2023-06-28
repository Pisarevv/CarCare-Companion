/**
 * PrivateGuard Component
 * ---------------------
 * This component usage is to stop unauthorized requests to access parts of the
 * website that are for users with authorization.
 * The component wraps around components that require authentication.
 * Example of a path that require authorization is editing a listing that a user has created.
 * Unauthorized request to access a protected path redirects to the login page.
 * 
 * Contexts:
 * - useAuthContext
 * The context provides the "isAuthenticated" constant that 
 * validates if an user is authenticated or not.
 * ---------------------- 
 * 
 **/
import { Navigate, Outlet } from "react-router-dom";

import { useAuthContext } from "../../contexts/AuthContext";

const PrivateGuard = ({children}) => {
    const {isAuthenticated} = useAuthContext();
   
    if (!isAuthenticated) {
        return <Navigate to='/login' replace/>
    } 

    return children ? children : <Outlet/>
}

export default PrivateGuard;