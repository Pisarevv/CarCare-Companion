// React's hooks for managing state and side-effects.
import { useEffect, useState } from 'react';

//React-router hook for managing location
import { useLocation } from 'react-router-dom';

// Custom hook for making authenticated Axios requests.
import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../../hooks/useDeauthenticate';

// Child component responsible for displaying individual trip record.
import LatestTripsCard from './LatestTripsCard';

//Notification component responsible for displaying notifications
import { NotificationHandler } from '../../../../utils/NotificationHandler';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../../../Common/IsLoadingHoc';

// CSS styles specific to this component.
import './LatestTrips.css'




/**
 * LatestTrips Component
 * This component fetches and displays the recent trips of the user.
 */

const LatestTrips = (props) => {

    // Destructure the setLoading function from the props.
    const { setLoading } = props;

    // React-router hook for location management.
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Use the custom hook to get the Axios instance for making authenticated requests.
    const axiosPrivate = useAxiosPrivate();

    // State variable to store the recent user trips.
    const [recentUserTrips, setRecentUserTrips] = useState([]);

    // Effect hook to fetch the latest trips upon component mount.
    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecentTrips = async () => {
            try {
                // Fetch the last 6 trips of the user.
                const response = await axiosPrivate.get(`/Trips/Last/${6}`, {
                    signal: controller.signal
                });
                // If the component is still mounted, update the state with the fetched trips.
                isMounted && setRecentUserTrips(recentUserTrips => response.data);
            } catch (err) {
                // On error, notify the user and navigate them to the login page.
                if(err.response.status == 401){
                    // On error, show a notification and redirect to the login page.
                   NotificationHandler("Something went wrong","Plese log in again", 400);
                   logUserOut(location);
               }   
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status);        
            }
            finally{
                // Set the loading state to false once the fetching process completes (either successfully or with an error).
                setLoading(false);
            }
        }

        getRecentTrips();

        // Cleanup function to abort the request if the component gets unmounted.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])


    return (
        <div className="home-latest-trips-container">
            <div className="home-latest-trips-header">Latest trips</div>
            <div className="home-latest-trips-list">
                {
                    recentUserTrips.length > 0
                        ?
                        recentUserTrips.map(rt => <LatestTripsCard key = {rt.id} details = {rt}/> )
                        :
                        <div>You haven't added any trip records yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(LatestTrips);