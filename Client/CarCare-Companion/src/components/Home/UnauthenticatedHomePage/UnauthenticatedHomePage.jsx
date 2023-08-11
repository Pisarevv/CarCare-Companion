// React's hooks for managing state and side-effects.
import { useEffect, useState } from "react";

// React-router's Link component for creating navigation links and useNavigate 
// for programmatically redirecting users to different routes/pages.
import { Link, useNavigate } from "react-router-dom";

// Helper utility for handling notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler'

// Context providing authentication-related functions and data.
import { useAuthContext } from "../../../contexts/AuthContext";

// Axios instance specifically configured for the app's API.
import axios from "../../../api/axios/axios";

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from "../../Common/IsLoadingHoc"

// Child component responsible for displaying individual reviews.
import ReviewCard from "./ReviewCard";

// CSS styles specific to this component.
import "./UnauthenticatedHomePage.css"

const UnauthenticatedHomePage = (props) => {

    // State for maintaining the list of advertisements for the carousel.
    const [carouselAds, setCarouselAds] = useState([]);

    // Extract the setLoading function from props to manage loading states.
    const { setLoading } = props;

    // Retrieve the current user's data from the authentication context.
    const {user} = useAuthContext();

    // Hook to get a function that lets us programmatically navigate to different routes.
    const navigate = useNavigate();

    useEffect(() => {
        // If a user is authenticated, redirect them to the home page.
        if(user.accessToken){
            navigate("/Home")
        }

        // Flag to ensure state updates only happen if the component is still mounted.
        let isMounted = true;

        // AbortController to cancel the fetch request if the component unmounts before the request completes.
        const controller = new AbortController();

        // Function to fetch and load carousel advertisements.
        const loadCarouselAds = async () => {
           try {
                // Make a GET request to retrieve advertisements for the carousel.
                const response = await axios.get("/Home",{
                    signal : controller.signal
                });
                // If the component is still mounted, update the carouselAds state with the received data.
                isMounted && setCarouselAds(carouselAds => response.data);
           } catch (error) {
                // Handle any errors during the API call by notifying the user.
                NotificationHandler(error);
           }
           finally {
                // Once data is fetched or an error occurred, stop showing the loading state.
                setLoading(false);
           }
        }

        // Immediately invoke the loadCarouselAds function.
        loadCarouselAds();

        // Cleanup function to run when the component is unmounted.
        return () => {
            isMounted = false;
            // Cancel the fetch request if the component unmounts before it completes.
            controller.abort();
        }
    }, [])  // Empty dependency array ensures the effect runs only once when the component mounts.


    return (
        <section className="home">
        <div className="container">
            <div className="introduction-container">
                <p>Welcome to CarCare Companion — your ultimate destination for all your car repair,trips and service management needs.</p>
                <p>With our platform, effortlessly add and manage your vehicles, trips, tax details, and service records. </p>
            </div>

            <div className="video-container">
                <video muted autoPlay loop src='video.mp4'></video>
            </div>

            <div className="join-container">
                <h2>Join Us Today!</h2>
                <p>Become a part of our dynamic community.</p>
                <p>By joining us, you will never miss another due date with our email reminder feature!.</p>
                <Link to="/Register">Sign up.</Link>
            </div>


            <div className="review-container">
                <div className="cards">
                    <div className="cards-inner">
                        <div className="cards-border"></div>
                        <div className="cards-row">
                            {carouselAds.map(x => (
                                <div className="card-col" key={x.id}>
                                    <ReviewCard reviewInfo={x} />
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>

       </div>
     </section>
    );

}


export default IsLoadingHOC(UnauthenticatedHomePage);