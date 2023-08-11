// React's useState hook for managing component local state.
import { useState } from 'react';

// React-router's NavLink for creating navigation links within the app.
import { NavLink, useNavigate } from 'react-router-dom';

// Helper utility for handling notifications.
import { NotificationHandler } from '../../utils/NotificationHandler';

// Axios instance specifically configured for the app.
import axios from '../../api/axios/axios';

// CSS styles specific to the Register component.
import './Register.css';


// Object containing regexes for validation of email and password.
const ValidationRegexes = {
    emailRegex: new RegExp('^[a-zA-Z0-9._:$!%-]+@[a-zA-Z0-9.-]+.[a-zA-Z]+$'),
    passwordRegex: new RegExp(/^(?=.*[a-zA-Z]).{6,}$/)
}

// Object containing error messages for validation.
const ValidationErrors = {
    email: "Please enter a valid email address",
    firstName: "First name cannot be less than two symbols",
    lastName: "Last name cannot be less than two symbols",
    password: "Password must contain minimum eight characters and at least one letter.",
    confirmPassword: "Passwords do not match"
}

/**
 * Register Component: Allows users to register on the platform.
 */
const Register = () => {
    const navigate = useNavigate();

    // State management for form inputs and their errors.
    const [email, setEmail] = useState("");
    const [emailError, setEmailError] = useState("");
    const [firstName, setFirstName] = useState("");
    const [firstNameError, setFirstNameError] = useState("");
    const [lastName, setLastName] = useState("");
    const [lastNameError, setLastNameError] = useState("");
    const [password, setPassword] = useState("");
    const [passwordError, setPasswordError] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [confirmPasswordError, setConfirmPasswordError] = useState("");

    // Handlers for input changes.
    const onEmailChange = (e) => setEmail(e.target.value);
    const onFirstNameChange = (e) => setFirstName(e.target.value);
    const onLastNameChange = (e) => setLastName(e.target.value);
    const onPasswordChange = (e) => setPassword(e.target.value);
    const onConfirmPasswordChange = (e) => setConfirmPassword(e.target.value);

    // Validation functions.
    const validateEmailInput = () => {
        if (!ValidationRegexes.emailRegex.test(email)) {
            setEmailError(ValidationErrors.email);
            return false;
        }
        setEmailError("");
        return true;
    }

    const validateFirstNameInput = () => {
        if (firstName.length < 2) {
            setFirstNameError(ValidationErrors.firstName);
            return false;
        }
        setFirstNameError("");
        return true;
    }

    const validateLastNameInput = () => {
        if (lastName.length < 2) {
            setLastNameError(ValidationErrors.lastName);
            return false;
        }
        setLastNameError("");
        return true;
    }

    const validatePasswordInput = () => {
        if (!ValidationRegexes.passwordRegex.test(password)) {
            setPasswordError(ValidationErrors.password);
            return false;
        }
        setPasswordError("");
        return true;
    }

    const validateConfirmPasswordInput = () => {
        if (confirmPassword !== password) {
            setConfirmPasswordError(ValidationErrors.confirmPassword);
            return false;
        }
        setConfirmPasswordError("");
        return true;
    }

    /**
     * Handler for the register form submission.
     * Validates input, makes API request to register, and navigates to login upon success.
     */
    const registerHandler = async (e) => {
        e.preventDefault();
        let isEmailValid = validateEmailInput();
        let isFirstNameValid = validateFirstNameInput();
        let isLastNameValid = validateLastNameInput();
        let isPasswordValid = validatePasswordInput();
        let isConfirmPasswordValid = validateConfirmPasswordInput();

        if (isEmailValid && isPasswordValid && isConfirmPasswordValid && isFirstNameValid && isLastNameValid) {
            try {
                await axios.post("/Register", { email, firstName, lastName, password, confirmPassword });
                navigate('/Login');
            } catch (error) {
                window.scrollTo({ top: 0, behavior: 'smooth' });
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status);
            }
        }
    }

    return (
        <div className="user">
            <header className="user__header">
                <img className='register-img' src="https://cdn4.iconfinder.com/data/icons/photo-camera-ui/512/car-auto-drive-automobile-form-7-512.png" alt="" />
                <h1 className="user__title">Join the journey today.</h1>
            </header>

            <form className="form" onSubmit={registerHandler}>
                <div className="form__group">
                    <input type="text" placeholder="Email" className="form__input" value={email} onChange={onEmailChange} />
                    {emailError && <p className='input__error'>{emailError}</p>}
                </div>

                <div className="form__group">
                    <input type="text" placeholder="First name" className="form__input" value={firstName} onChange={onFirstNameChange} />
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
                    <input type="password" placeholder="Repeat password" className="form__input" value={confirmPassword} onChange={onConfirmPasswordChange} />
                    {confirmPasswordError && <p className='input__error'>{confirmPasswordError}</p>}
                </div>

                <button className="btn" type="submit">Register</button>

                <p className="sign-up">Already have an account? <NavLink to="/login">Log in</NavLink>.</p>
            </form>
        </div>
    )
}


export default Register;