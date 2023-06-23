/**
 *  ErrorHandler
 *  This is a custom function that handles errors thrown by the REST api or application 
 *  and based on the error shows the user notifications.
 * -----------------
**/

import { Store } from 'react-notifications-component';

const errors = {
    "Invalid access token" : {
        title : "Invalid access token",
        message : "Your session has expired Please log in again.",
        type : "warning"
    },
    "Login or password don't match" : {
        title : "Invalid credentials",
        message : "Login or password don't match",
        type : "warning"
    },
    "Invalid input fields" : {
        title : "Invalid input fields",
        message : "Please make sure all inputs are valid before submiting",
        type : "warning"
    },
    "A user with the same email already exists" : {
        title : "A user with the same email already exists",
        message : "Please use another email to register",
        type : "warning"
    },
    "Comment cannot be empty" : {
        title : "Comment field cannot be empty",
        message : "Please write your comment befor submiting",
        type : "warning"
    }
}




export const ErrorHandler = (inputError) => {
    const {title,message,type} = errors[inputError];
    return (
        Store.addNotification({
            title: title,
            message: message,
            type: type,
            insert: "top",
            container: "top-right",
            animationIn: ["animate__animated", "animate__fadeIn"],
            animationOut: ["animate__animated", "animate__fadeOut"],
            dismiss: {
                duration: 3000,
                onScreen: true
            }
        })
    )
}