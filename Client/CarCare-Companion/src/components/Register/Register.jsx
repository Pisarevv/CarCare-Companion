import './Register.css'

import { useContext, useState } from 'react';
import { NavLink, useNavigate } from 'react-router-dom';

import { AuthContext } from '../../contexts/AuthContext';

import { register } from '../../services/authService';  

const Register = () => {
    return (
        <div className="user">
            <header className="user__header">
                <img src="https://cdn4.iconfinder.com/data/icons/photo-camera-ui/512/car-auto-drive-automobile-form-7-512.png" alt="" />
                <h1 className="user__title">Join the journey today.</h1>
            </header>

            <form className="form">
                <div className="form__group">
                    <input type="email" placeholder="Email" className="form__input" />
                </div>

                <div className="form__group">
                    <input type="text" placeholder="First name" className="form__input" />
                </div>

                <div className="form__group">
                    <input type="text" placeholder="Last name" className="form__input" />
                </div>

                <div className="form__group">
                    <input type="password" placeholder="Password" className="form__input" />
                </div>

                <div className="form__group">
                    <input type="password" placeholder="Repeat password" className="form__input" />
                </div>

                <button className="btn" type="button">Register</button>
            </form>
        </div>
    )
}


export default Register;