/**
 *  NotificationsHandler
 *  This is a custom function that handles notifications or errors thrown by the REST api or application 
 *  and based on the input shows the user notifications.
 * -----------------
**/


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
    
    const type = responseStatusNotificationType(responeStatus);
    
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