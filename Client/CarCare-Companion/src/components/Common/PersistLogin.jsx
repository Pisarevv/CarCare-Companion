import { Outlet } from "react-router-dom";

import { useState, useEffect } from "react";

import useRefreshToken from "../../hooks/useRefreshToken";

import { useAuthContext } from "../../contexts/AuthContext";

const PersistLogin = () => {
    const [isLoading,setIsLoading] = useState(true);
    const refresh = useRefreshToken();
    const {isAuthenticated} = useAuthContext();

    useEffect(() => {
        const verifyRefreshToken = async () => {
            try{
                await refresh();
            }
            catch(err){
              
            }
            finally{
                setIsLoading(false);
            }
        }
      
        !isAuthenticated ? verifyRefreshToken() : setIsLoading(false);
    }, [])

    return (
        <>
        { isLoading ? "" : <Outlet/>  }
        </>
    )
}

export default PersistLogin;