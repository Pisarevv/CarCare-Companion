// Required React imports.
import { useEffect, useState } from 'react';

// React-router imports for navigation and routing.
import { NavLink, useLocation, useNavigate } from 'react-router-dom';

// Utility for displaying notifications.
import { NotificationHandler } from '../../utils/NotificationHandler';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../hooks/useDeauthenticate';

// Custom hook to make authenticated API requests using Axios.
import useAxiosPrivate from '../../hooks/useAxiosPrivate';

// Higher Order Component for handling loading state.
import IsLoadingHOC from '../Common/IsLoadingHoc';

// Child components imports.
import TripsStatistics from './TripsStatistics/TripsStatistics';
import UserTripCard from './UserTripCard';

// Component-specific styles.
import './Trips.css';


// React component to display and manage user trips.
const Trips = (props) => {

    // Hook for making authorized API requests.
    const axiosPrivate = useAxiosPrivate();

    // Hooks from React Router to programmatically navigate and get the current location.
    const navigate = useNavigate();
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // State variable for storing the trips of a user.
    const [userTrips, setUserTrips] = useState([]);

    // Destructure the setLoading function from props, which controls the loading state.
    const { setLoading } = props;

    // useEffect hook to fetch the trips when the component is mounted.
    useEffect(() => {
        // Flag to ensure state updates only occur if the component is still mounted.
        let isMounted = true;
        const controller = new AbortController();

        const getTrips = async () => {
            try {
                // Fetch the trips data.
                const response = await axiosPrivate.get('/Trips', {
                    signal: controller.signal
                });
                isMounted && setUserTrips(userTrips => response.data);
            } catch (err) {
                // Handle error and redirect to login in case of an error.
                if(err.response.status == 401){
                    // On error, show a notification and redirect to the login page.
                   NotificationHandler("Something went wrong","Plese log in again", 400);
                   logUserOut(location);
               }   
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status);
            } finally {
                // Stop the loading state regardless of success or error.
                setLoading(false);
            }
        }

        getTrips();

        // Cleanup function to ensure no unintended side effects.
        return () => {
            isMounted = false;
            controller.abort();
        }
    }, []);

    // Render the UI for the trips, including statistics and a list of trip cards.
    return (
        <section className="trips-section">
            <div className="trips-container">
                <div className="trips-statistics">
                    <div className="add-trip-button"><NavLink to="/Trips/Add">Add trip</NavLink></div>
                    <div className='trip-records-statistics'> <TripsStatistics /></div>
                </div>
                <div className="trips-list-container">
                    {/* Map through the user trips and display each one using the UserTripCard component. */}
                    {userTrips.map(ut => <UserTripCard key={ut.id} tripDetails={ut} />)}
                </div>
            </div>
        </section>
    )
}

// Export the component wrapped in the IsLoadingHOC to handle loading states.
export default IsLoadingHOC(Trips);
