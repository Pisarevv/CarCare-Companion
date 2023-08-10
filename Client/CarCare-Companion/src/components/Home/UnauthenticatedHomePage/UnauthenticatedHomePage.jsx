import { useEffect, useState } from "react";

import { Link, useNavigate } from "react-router-dom";

import { NotificationHandler } from '../../../utils/NotificationHandler'

import { useAuthContext } from "../../../contexts/AuthContext";

import axios from "../../../api/axios/axios";
import IsLoadingHOC from "../../Common/IsLoadingHoc"

import ReviewCard from "./ReviewCard";

import "./UnauthenticatedHomePage.css"

const UnauthenticatedHomePage = (props) => {

    const [carouselAds, setCarouselAds] = useState([]);

    const { setLoading } = props;

    const {user} = useAuthContext();

    const navigate = useNavigate();

    useEffect(() => {
        if(user.accessToken){
            navigate("/Home")
        }
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