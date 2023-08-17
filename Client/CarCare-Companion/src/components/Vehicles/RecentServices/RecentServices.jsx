// Required React imports.
import { useEffect, useState } from 'react';

//React-router hook for managing location
import { useLocation } from 'react-router-dom';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../hooks/useDeauthenticate';

// Utility for displaying notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Custom hook for making authenticated axios requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Child component import.
import RecentServiceCard from './RecentServiceCard';

// Higher Order Component for handling loading state.
import IsLoadingHOC from '../../Common/IsLoadingHoc';

import './RecentServices.css'

/**
 * The RecentServices component fetches and displays the latest service records.
 * 
 * @param {Object} props - Props received by the component.
 * @param {Function} props.setLoading - Function to update the loading state.
 */
const RecentServices = (props) => {

    // Extract the setLoading function from the passed props.
    const { setLoading } = props;

    // Provides access to the current location (route).
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Initialize a custom hook for making authenticated axios requests.
    const axiosPrivate = useAxiosPrivate();

    // State to store the fetched recent service records.
    const [recentServiceRecords, setRecentServiceRecords] = useState([]);

    // Effect hook to run side effects after component mount. Here, fetching the data.
    useEffect(() => {
        // Flag to check if the component is still mounted when updating state.
        let isMounted = true;

        // Controller for the Abort API, useful for cancelling promises.
        const controller = new AbortController();

        // Function to fetch the recent services data.
        const getRecentServices = async () => {
            try {
                const response = await axiosPrivate.get(`/ServiceRecords/Last/${3}`, {
                    signal: controller.signal // Attach signal for potential aborting of request.
                });

                // Only update the state if the component is still mounted.
                isMounted && setRecentServiceRecords(response.data);
            } catch (err) {
                // Handle error and redirect to login in case of an error.
                if (err.response.status == 401) {
                    // On error, show a notification and redirect to the login page.
                    NotificationHandler("Something went wrong", "Plese log in again", 400);
                    logUserOut(location);
                }
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status);
            }
            finally {
                // Set loading to false once data is fetched, or an error has occurred.
                setLoading(false);
            }
        }

        // Invoke the data fetching function.
        getRecentServices();

        // Cleanup function to update the isMounted flag and potentially abort ongoing requests.
        return () => {
            isMounted = false;
            controller.abort();
        }
    }, []) // Dependency array is empty, so this effect runs only on mount and unmount.

    return (
        // Container for the recent services.
        <div className="recent-services-container">
            <div className="services-header">Recently added service records</div>
            <div className="services-list">
                {
                    // Display the services if present, else show a placeholder message.
                    recentServiceRecords.length > 0
                        ? recentServiceRecords.map(rs => <RecentServiceCard key={rs.id} details={rs} />)
                        : <div>You haven't added any service records yet.</div>
                }
            </div>
        </div>
    )
}

// Export the RecentServices component wrapped with a Higher Order Component (HOC) for loading state.
export default IsLoadingHOC(RecentServices);
