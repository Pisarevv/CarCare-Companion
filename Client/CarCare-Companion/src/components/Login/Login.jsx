// React's hooks for managing component state and accessing the context.
import { useContext, useState } from "react";

// React-router's NavLink for creating navigation links and useNavigate for programmatic navigation.
import { NavLink, useNavigate } from 'react-router-dom';

// Custom hook for making authenticated Axios requests. This ensures that API requests made from this component are authenticated.
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

// React's context for user authentication. This provides functions and data related to user authentication across the app.
import { AuthContext } from "../../contexts/AuthContext";

// Helper utility for displaying notifications to the user. This can show success, error, or other types of messages.
import { NotificationHandler } from "../../utils/NotificationHandler";

// CSS styles specific to this component, enhancing its visual appearance.
import './Login.css';


/**
 * Login Component: Allows users to log into the platform.
 */
const Login = () => {

    // Retrieve the userLogin function from the AuthContext to authenticate and set user data.
    const { userLogin } = useContext(AuthContext);

    // React Router's navigate hook for programmatic navigation.
    const navigate = useNavigate();

    // Use the custom hook to fetch data in a secure way.
    const axiosPrivate = useAxiosPrivate();

    // States to manage input fields.
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    // Event handlers for input changes.
    const onEmailChange = (e) => setEmail(e.target.value);
    const onPasswordChange = (e) => setPassword(e.target.value);

    /**
     * loginHandler: Handles form submission for login.
     * Makes an API request to validate user credentials, updates the user context on success, and redirects to the home page.
     */
    const loginHandler = async (e) => {
        e.preventDefault();
        try {
            const returnedUserData = await axiosPrivate.post("/Login", { email, password });
            userLogin(returnedUserData.data);
            navigate("/Home");
            NotificationHandler("Success", "Welcome back!", returnedUserData.response.status);
        } catch (error) {
            window.scrollTo({ top: 0, behavior: 'smooth' });  // Scroll to top for visibility of any notifications.
            const { title, status } = error.response.data;
            NotificationHandler(title, title, status);  // Display notification using the handler.
        }
    }


    return (
        <div className="user">
            <header className="user__header">
                <img className="login-img" src="https://cdn3.iconfinder.com/data/icons/car-icons-front-views/480/Sports_Car_Front_View-512.png" alt="" />
                <h1 className="user__title">Welcome back.</h1>
            </header>

            <form className="form" onSubmit={loginHandler}>
                <div className="form__group">
                    <input type="text" placeholder="Email" className="form__input" value={email} onChange={onEmailChange} />
                </div>

                <div className="form__group">
                    <input type="password" placeholder="Password" className="form__input" value={password} onChange={onPasswordChange} />
                </div>

                <button className="btn" type="submit">Login</button>

                <p className="sign-up">Don't have an account yet? <NavLink to="/register">Sign up.</NavLink></p>
            </form>
        </div>
    )
}


export default Login;