import { useEffect, useState } from "react";

import { Link } from "react-router-dom";

import { NotificationHandler } from '../../../utils/NotificationHandler'

import axios from "../../../api/axios/axios";
import IsLoadingHOC from "../../Common/IsLoadingHoc"

import ReviewCard from "../ReviewCard";

import "./UnauthenticatedHomePage.css"


const UnauthenticatedHomePage = (props) => {

    const [carouselAds, setCarouselAds] = useState([]);

    const { setLoading } = props;

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const loadCarouselAds = async () => {
           try {
            const response = await axios.get("/Home",{
                signal : controller.signal
            });
            isMounted && setCarouselAds(carouselAds => response.data);
           } catch (error) {
            NotificationHandler(error);
           }
           finally{
            setLoading(false);
           }
        }

        loadCarouselAds();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    },[])


    return (
        <>
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

        </>
    );

}


export default IsLoadingHOC(UnauthenticatedHomePage);