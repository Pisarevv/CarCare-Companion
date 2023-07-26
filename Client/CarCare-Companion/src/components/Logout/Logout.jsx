import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useAuthContext } from "../../contexts/AuthContext";

import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import IsLoadingHOC from "../Common/IsLoadingHoc";

import { NotificationHandler } from "../../utils/NotificationHandler";


const Logout = (props) => {

    const {userLogout} = useAuthContext();
    const {setLoading} = props;
    const navigate = useNavigate();
    const axiosPrivate = useAxiosPrivate();

    useEffect(() => {
        const logoutUser = async () => {
            try {
                const response = await axiosPrivate.post('/Logout');
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                userLogout();
                navigate("/");
                setLoading(false);
            }
        }
        logoutUser();
    
    }, [])


    return null;
}


export default IsLoadingHOC(Logout);