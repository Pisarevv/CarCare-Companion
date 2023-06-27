import './Register.css'

import { useContext, useState } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

import { AuthContext } from '../../contexts/AuthContext';

import { register } from '../../services/authService';  

import { ErrorHandler } from '../../utils/ErrorHandler/ErrorHandler';

const ValidationRegexes = {
    //The current regex validates that the input email address 
    //begins with a string, contains a "@" symbol and "." after the domain
    // and end with a top-level-domain TLD
    emailRegex: new RegExp('^[a-zA-Z0-9._:$!%-]+@[a-zA-Z0-9.-]+.[a-zA-Z]+$'),

    //This regex validates that the input has minimul 6 charecters and one of them 
    //must be a letter
    passwordRegex: new RegExp( /^(?=.*[a-zA-Z]).{6,}$/)
}

const ValidationErrors = {
    email : "Please enter a valid email address",
    firstName : "First name cannot be less than two symbols",
    lastName : "Last name cannot be less than two symbols",
    password: "Password must contain minimum eight characters and at least one letter.",
    rePassword: "Passwords do not match"
}

const Register = () => {


    const {userLogin} = useContext(AuthContext);
    const navigate = useNavigate();

    //States

    const [email,setEmail] = useState("");
    const [emailError, setEmailError] = useState("");

    const [firstName, setFirstName] = useState("");
    const [firstNameError,setFirstNameError] = useState("");

    const [lastName, setLastName] = useState("");
    const [lastNameError,setLastNameError] = useState("");

    const [password,setPassword] = useState("");
    const [passwordError, setPasswordError] = useState("");

    const [rePassword,setRepassword] = useState("");
    const [rePasswordError, setRePasswordError] = useState("");

 
    //Event handlers
    const onEmailChange = (e) => {
        setEmail(email => e.target.value);
    }

    const onFirstNameChange = (e) => {
        setFirstName(firstName => e.target.value);
    }

    const onLastNameChange = (e) => {
        setLastName(lastName => e.target.value);
    }

    const onPasswordChange = (e) => {
        setPassword(password => e.target.value);
    }

    const onRePasswordChange = (e) => {
        setRepassword(rePassword => e.target.value)
    }

    const validateEmailInput = () => {   
        if(!ValidationRegexes.emailRegex.test(email)){
            setEmailError(emailError => ValidationErrors.email);
            return false;
        }
        setEmailError(emailError => "");
        return true;
    }

    const validateFirstNameInput = () => {
        if(firstName.length < 2){
            setFirstNameError(firstName => ValidationErrors.firstName);
            return false;
        }
        setFirstNameError(firstName => "");
        return true;
    }

    const validateLastNameInput = () => {
        if(lastName.length < 2){
            setLastNameError(lastName => ValidationErrors.lastName);
            return false;
        }
        setLastNameError(lastName => "");
        return true;
    }


    const validatePasswordInput = () => {
        if(!ValidationRegexes.passwordRegex.test(password)){
            setPasswordError(passwordError => ValidationErrors.password);
            return false;
        }
        setPasswordError(passwordError => "");
        return true;
    }

    const validateRePasswordInput = () => {
        if(rePassword !== password){
            setRePasswordError(rePasswordError => ValidationErrors.rePassword);
            return false;
        }
        setRePasswordError(rePasswordError => "");
        return true;
    }

    const registerHandler = async(e) => {
        e.preventDefault();
        try{
            let isEmailValid = validateEmailInput(email);
            let isFirstNameValid = validateFirstNameInput(firstName);
            let isLastNameValid = validateLastNameInput(lastName);
            let isPasswordValid = validatePasswordInput(password);
            let isRePasswordValid = validateRePasswordInput(rePassword);

            if(isEmailValid && isPasswordValid && isRePasswordValid && isFirstNameValid && isLastNameValid){
                let returnedUserData = await register(email,firstName,lastName,password,rePassword);
                userLogin(returnedUserData);
                navigate('/');
            }
            else{
                throw("Invalid input fields")
            }
        }
        catch(error){
            ErrorHandler(error);
        }
        
       }

    return (
        <div className="user">
            <header className="user__header">
                <img src="https://cdn4.iconfinder.com/data/icons/photo-camera-ui/512/car-auto-drive-automobile-form-7-512.png" alt="" />
                <h1 className="user__title">Join the journey today.</h1>
            </header>

            <form className="form" onSubmit={registerHandler}>
                <div className="form__group">
                    <input type="text" placeholder="Email" className="form__input" value={email} onChange={onEmailChange} />
                    {emailError && <p className='input__error'>{emailError}</p>}
                </div>

                <div className="form__group">
                    <input type="text" placeholder="First name" className="form__input" value={firstName} onChange={onFirstNameChange}/>
                    {firstNameError && <p className='input__error'>{firstNameError}</p>}
                </div>

                <div className="form__group">
                    <input type="text" placeholder="Last name" className="form__input" value={lastName} onChange={onLastNameChange} />
                    {lastNameError && <p className='input__error' >{lastNameError}</p>}
                </div>

                <div className="form__group">
                    <input type="password" placeholder="Password" className="form__input" value={password} onChange={onPasswordChange} />
                    {passwordError && <p className='input__error'>{passwordError}</p>}
                </div>

                <div className="form__group">
                    <input type="password" placeholder="Repeat password" className="form__input" value={rePassword} onChange={onRePasswordChange} />
                    {rePasswordError && <p className='input__error'>{rePasswordError}</p>}
                </div>

                <button className="btn" type="submit">Register</button>

                <p className="sign-up">Already have an account? <NavLink to="/login">Log in</NavLink>.</p>
            </form>
        </div>
    )
}


export default Register;