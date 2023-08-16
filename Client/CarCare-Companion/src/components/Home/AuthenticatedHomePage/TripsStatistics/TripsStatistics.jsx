// React core dependencies for handling side effects and state.
import { useEffect, useState } from "react";

//React-router hook for managing location
import { useLocation } from 'react-router-dom';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../../hooks/useDeauthenticate';

// High-Order Component for loading state management.
import IsLoadingHOC from "../../../Common/IsLoadingHoc";

// Utility function to handle and display notifications to the user.
import { NotificationHandler } from "../../../../utils/NotificationHandler";

// Component-specific styles.
import './TripsStatistics.css';

// Custom hook for making authenticated Axios requests.
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

/**
 * The TripsStatistics component fetches and displays the user's trip statistics.
 * Specifically, it showcases the total number of trips and their combined cost.
 * If there's an error while fetching data, an appropriate notification is shown.
 * 
 * @param {Object} props 
 * @param {Function} props.setLoading - Function to manage the loading state in the parent component.
 */
const TripsStatistics = (props) => {

    // React-router hook for location management.
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Initialization of the custom hook to make authenticated Axios requests.
    const axiosPrivate = useAxiosPrivate();

    // Destructuring for easier props access.
    const { setLoading } = props;

    // State to manage the count of user trips and their total cost.
    const [userTripsCount, setUserTripsCount] = useState(null);
    const [userTripsCost, setUserTripsCost] = useState(null);

    // Effect to fetch user vehicles on component mount.
    useEffect(() => {
      let isMounted = true;  // Flag to handle component unmount scenario.
      const controller = new AbortController();  // For aborting the fetch request.
    
        // Fetch user's trip statistics from the server.
        const getTripStatistics = async () => {
            try {
                const requests = [
                    axiosPrivate.get('/Trips/Count', { signal: controller.signal }),
                    axiosPrivate.get('/Trips/Cost', { signal: controller.signal })
                ];
                
                Promise.all(requests).then(responses => {
                    const tripsCount = responses[0].data;
                    const tripsCost = responses[1].data;

                    if (isMounted) {
                        setUserTripsCount(userTripsCount => tripsCount);
                        setUserTripsCost(userTripsCost => tripsCost);
                    }        
                });
            } catch (err) {
                // Handling errors and redirecting to login in case of failure.
                if(err.response.status == 401){
                    // On error, show a notification and redirect to the login page.
                   NotificationHandler("Something went wrong","Plese log in again", 400);
                   logUserOut(location);
               }   
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status);
            } finally {
              // Set loading state to false after data fetching.
                setLoading(false);
            }
        };
    
        getTripStatistics();
    
        // Cleanup function to prevent updates on unmounted components.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, []);

    return (

        <div className="home-page-trips-statistics-list">
            <h4>Trip records:</h4>
            <div className="home-page-trip-statistics-border"></div>
            <div className="home-page-trips-count">You have completed {userTripsCount} so far.</div>
            <div className="home-page-trips-cost">The total cost of your trips is: {userTripsCost} lv.</div>      
        </div>   
        
    )
}


export default IsLoadingHOC(TripsStatistics);