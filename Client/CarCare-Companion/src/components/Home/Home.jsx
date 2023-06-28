import { NavLink } from "react-router-dom";
import "./Home.css"

const Home = () => {

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

            </div>

        </section>
    )
}

export default Home; 