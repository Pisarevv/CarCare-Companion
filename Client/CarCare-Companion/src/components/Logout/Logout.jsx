import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useAuthContext } from "../../contexts/AuthContext";



const Logout =  () => {

    const {userLogout} = useAuthContext();
    const navigate = useNavigate();

    useEffect(() => {
        userLogout();
        navigate("/");
    },[])

    return null;
}


export default Logout;