/**
 * Login Component
 * ---------------------
 * This component displays the login form for the user
 * to authenticate.
 * ---------------------- 
 * 
 * States:
 * ----------------------
 * - email (string): Holding the user email input.
 * - password (string): Holding hte user password input.
 * ----------------------
 * 
 * Contexts:
 * ----------------
 * - useAuthContext
 *  In this component this context provides the "userLogin" function.
 *  The purpose of this function is to set the user data after successful login 
 *  in the custom localStorage hook.
 *  
 *  - useCartContext
 *  In this component this context provides the "addProductToCart" function.
 *  The purpose of this function is to set the user cart products after successful login.
 * -----------------
 * 
 * Functions:
 * -----------------
 * - onEmailChange:
 *  Function for handling user input for email.
 * - onPasswordChange:
 *  Function for handling user input for password.
 * - loginHandler:
 *   Function that sends the user input.
 *   If the sent data is valid the user is authenticated and redircted.
 * 
 * - ErrorHandler
 *  This is a custom function that handles errors thrown by the REST api  
 *  and based on the error shows the user notifications.
 *  In the current case with a invalid access  token the user recieves a 
 *  notification containing :
 *  title : "Invalid access token"
 *  message : "Your session has expired. Please log in again."
 * -----------------
**/

import { useContext, useState } from "react";
import { NavLink, useNavigate } from 'react-router-dom';

import { NotificationHandler } from '../../utils/NotificationHandler'

import { AuthContext } from "../../contexts/AuthContext";

import './Login.css';
import useAxiosPrivate from "../../hooks/useAxiosPrivate";



const Login = () => {
    
    const {userLogin} = useContext(AuthContext);

    const navigate = useNavigate();

    const axiosPrivate = useAxiosPrivate();

    //States

    const [email,setEmail] = useState("");

    const [password,setPassword] = useState("");

    //Event handlers
    const onEmailChange = (e) => {
        setEmail(email => e.target.value);
    }

    const onPasswordChange = (e) => {
        setPassword(password => e.target.value);
    }

    const loginHandler = async (e) => {
        e.preventDefault();
        try {
            const returnedUserData = await axiosPrivate.post("/Login", {email,password});
            userLogin(returnedUserData.data);
            navigate("/Home");      
        } 
        catch (error) {
            NotificationHandler(error);
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