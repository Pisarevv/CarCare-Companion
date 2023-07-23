import { useEffect } from "react";
import { useAuthContext } from "../../../contexts/AuthContext";

import IsLoadingHOC from "../../Common/IsLoadingHoc";
import { NotificationHandler } from "../../../utils/NotificationHandler";

const AuthenticatedHomePage = (props) => {

    const { userLogout } = useAuthContext();

    const { setLoading } = props;

    useEffect(() => {
        (async () => {
            try {
                window.scrollTo(0, 0);

                setLoading(false);
            }
            catch (error) {
                if (error === "Invalid access token") {
                    NotificationHandler(error);
                    userLogout();
                    setLoading(false);

                };
            }
        }
        )()
    }, [])

   
    return (
        <>
         <div className="greeting-container"></div>
            Welcome back!
        </>
    );

}


export default IsLoadingHOC(AuthenticatedHomePage);