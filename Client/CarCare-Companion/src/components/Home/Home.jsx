import { useEffect } from 'react';

import { useAuthContext } from "../../contexts/AuthContext";

import AuthenticatedHomePage from './AuthenticatedHomePage/AuthenticatedHomePage';
import UnauthenticatedHomePage from './UnauthenticatedHomePage/UnauthenticatedHomePage';

import useRefreshToken from '../../hooks/useRefreshToken';
import IsLoadingHOC from '../Common/IsLoadingHoc';

import "./Home.css"



const Home = (props) => {

    const refresh = useRefreshToken();
    const { isAuthenticated } = useAuthContext();
    const { setLoading } = props;

    useEffect(() => {
        const refreshUser = async () => {
            try {
                await refresh();
            } catch (err) {

            }
            finally {
                setLoading(false);
            }
        }

        refreshUser();
    }, [])



    return (
        <section className="home">

            <div className="container">
                {isAuthenticated ? <AuthenticatedHomePage /> : <UnauthenticatedHomePage />}
            </div>

        </section>
    )
}

export default IsLoadingHOC(Home); 