/**
 * AuthContext 
 * ---------------------
 * React authentication context and provider,
 * as well as a custom hook to access the authentication state and functions in any component.
 * It contains the user data - access token, id and email address.
 * ---------------------- 
 * 
 * States:
 * ----------------------
 * - auth (object): This state uses the custom UseLocalStorage hook that
 *   allows the state to persist even when the page is refreshed. 
 * ---------------------
 * 
 * Functions: 
 * - userLogin, userLogout - These functions are used to update the authentication state when a user logs in or logs out.   
**/

import { createContext, useContext } from "react";
import useLocalStorage from "../hooks/useLocalStorage";

export const AuthContext = createContext();

export const AuthProvider = ({
    children
}) => {
    const [auth, setAuth] = useLocalStorage('user', {});

    const userLogin = (userData) => {
        setAuth(userData);
    }

    const userLogout = () => {
        setAuth({})
    }

    return (
        <AuthContext.Provider value = {{user: auth, userLogin, userLogout, isAuthenticated : !!auth.accessToken}}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuthContext = () => {
    const context = useContext(AuthContext);

    return context;
}