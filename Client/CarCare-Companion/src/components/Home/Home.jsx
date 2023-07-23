    
import "./Home.css"
import {  useAuthContext } from "../../contexts/AuthContext";
import AuthenticatedHomePage from './AuthenticatedHomePage/AuthenticatedHomePage';
import UnauthenticatedHomePage from './UnauthenticatedHomePage/UnauthenticatedHomePage';



const Home = () => {
    
    const { isAuthenticated } = useAuthContext();
    
    return (
        <section className="home">

            <div className="container">
              { isAuthenticated ? <AuthenticatedHomePage/> : <UnauthenticatedHomePage/>   }
            </div>

        </section>
    )
}

export default Home; 