/**
 *  NotificationsHandler
 *  This is a custom function that handles notifications or errors thrown by the REST api or application 
 *  and based on the input shows the user notifications.
 * -----------------
**/

import { Store } from 'react-notifications-component';

const notificationMessages = {
    "Invalid access token" : {
        title : "Invalid access token",
        message : "Your session has expired. Please log in again.",
        type : "warning"
    },
    "Login or password don't match" : {
        title : "Invalid credentials",
        message : "Login or password don't match",
        type : "warning"
    },
    "One or more validation errors occurred." : {
        title : "Invalid input fields",
        message : "Please make sure all inputs are filled with valid information before submiting",
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
    },
    "Generic error" : {
        title : "Unexpected error",
        message : "Something unexpected happend. Please refresh the page.",
        type : "danger"
    },
    "Successful record removal" : {
        title : "Sucess",
        message : "Sucessfully removed the record.",
        type : "success"
    }
}

const responseStatusNotificationType = (responeStatus) => {
    if(Number(responeStatus) >= 100 && Number(responeStatus) <= 199){
        return "info";
    }

    if(Number(responeStatus) >= 200 && Number(responeStatus) <= 299){
        return "success";
    }

    if(Number(responeStatus) >= 300 && Number(responeStatus) <= 399){
        return "default";
    }

    if(Number(responeStatus) >= 400 && Number(responeStatus) <= 599){
        return "warning";
    }

}

'success' | 'danger' | '' | 'default' | 'warning';

export const NotificationHandler = (title,message,responeStatus) => {

    // if(!notificationMessages.hasOwnProperty(input)){
    //    input = "Generic error"
    // }
    
    const type = responseStatusNotificationType(responeStatus);
    
    // const {title,message,type} = notificationMessages[input];
    return (
        Store.addNotification({
            title: title,
            message: message,
            type: type,
            insert: "top",
            container: "top-center",
            animationIn: ["animate__animated", "animate__fadeIn"],
            animationOut: ["animate__animated", "animate__fadeOut"],
            dismiss: {
                duration: 3000,
                onScreen: true
            }
        })
    )
}