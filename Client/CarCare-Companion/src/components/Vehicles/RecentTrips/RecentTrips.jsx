// React hooks.
import { useEffect, useState } from 'react';

// Custom hook for making authenticated Axios calls.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Subcomponent for displaying individual recent trips.
import RecentTripCard from './RecentTripCard';

// Higher Order Component (HOC) to handle loading states.
import IsLoadingHOC from '../../Common/IsLoadingHoc';

// Component-specific styles.
import './RecentTrips.css'

/**
 * The RecentTrips component displays the last three trips made by a user.
 * 
 * @param {Object} props - Props passed to the component.
 * @param {Function} props.setLoading - Function to toggle loading state (from Higher Order Component).
 */
const RecentTrips = (props) => {

    // Destructure `setLoading` from props.
    const { setLoading } = props;

    // Custom hook instance for making authenticated Axios calls.
    const axiosPrivate = useAxiosPrivate();

    // State variable to store recent trips data.
    const [recentUserTrips, setRecentUserTrips] = useState([]);

    // Effect hook to fetch the recent trips on component mount.
    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        // Async function to fetch the recent trips.
        const getRecentTrips = async () => {
            try {
                const response = await axiosPrivate.get(`/Trips/Last/${3}`, {
                    signal: controller.signal
                });
                isMounted && setRecentUserTrips(recentUserTrips => response.data);
            } catch (err) {
                // Handle any errors, such as network issues or token expiry, with a NotificationHandler.
                NotificationHandler(err);
                // On error, navigate to login page (Note: `navigate` and `location` are not defined in the provided code).
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
                // Set the loading state to false after fetching is complete.
                setLoading(false);
            }
        }

        getRecentTrips();

        // Cleanup function to abort any pending Axios calls.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    return (
        <div className="recent-trips-container">
            <div className="trips-header">Recent trips</div>
            <div className="trips-list-vehicle">
                {
                    // Conditionally render either the recent trips or a message if no trips are found.
                    recentUserTrips.length > 0
                        ?
                        recentUserTrips.map(rt => <RecentTripCard key={rt.id} details={rt} />)
                        :
                        <div>You haven't added any trip records yet.</div>
                }
            </div>
        </div>
    )
}

// Exporting the RecentTrips component wrapped in IsLoadingHOC.
export default IsLoadingHOC(RecentTrips);
