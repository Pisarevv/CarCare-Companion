import { NavLink } from "react-router-dom";

import { useContext, useEffect, useState } from "react";


import IsLoadingHOC from '../Common/IsLoadingHoc';

import "./Home.css"

import { getAllCarouselAds } from "../../services/adService"; 
import { AuthContext } from "../../contexts/AuthContext";
import ReviewCard from "./ReviewCard";


const Home = (props) => {

    const  [carouselAds,setCarouselAds] = useState([]);

    const { userLogout } = useContext(AuthContext);

    const { setLoading } = props;

    useEffect(() => {
        (async () => {
            try {
                window.scrollTo(0, 0);
                const result = await getAllCarouselAds();
                setCarouselAds(carouselAds => result);
                console.log(result);
                setLoading(false);
            }
            catch (error) {
                if (error === "Invalid access token") {
                    ErrorHandler(error);
                    userLogout();
                    setLoading(false);
                    
                };
            }
        }
        )()
    },[])

    return (
        <section className="home">

            <div className="container">

                <div className="introduction-container">
                    <p>Welcome to CarCare Companion — your ultimate destination for all your car repair,trips and service management needs.</p>
                    <p>Discover expert advice, valuable resources, and a vibrant community of automotive enthusiasts. Start exploring now!</p>
                </div>

                <div className="video-container">
                    <video muted autoPlay loop src='video.mp4'></video>
                </div>

                <div className="join-container">
                    <h2>Join Us Today!</h2>
                    <p>Become a part of our dynamic community of car enthusiasts, mechanics, and DIYers.</p>
                    <p> By joining us, you'll gain access to a wealth of knowledge, valuable tips, and engaging discussions on car repairs and service management.</p>
                    <NavLink to="/register">Sign up.</NavLink>
                </div>

                
                <div className="review-container">
                    <div className="cards">
                        <div className="cards-inner">
                             <div className="cards-border"></div>
                             <div className="cards-row">
                                {carouselAds.map(x => (
                                    <div className="card-col" key={x.id}>
                                     <ReviewCard  reviewInfo = {x}/>
                                    </div>
                                ))}
                             </div>
                        </div>
                    </div>
                </div>

            </div>

        </section>
    )
}

export default IsLoadingHOC(Home); 