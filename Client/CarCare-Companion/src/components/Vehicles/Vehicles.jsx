// Required React imports.
import { useEffect, useState } from 'react';

// React-router imports for navigation and routing.
import { NavLink, useLocation } from 'react-router-dom';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../hooks/useDeauthenticate';

// Custom hook for making authenticated axios requests.
import useAxiosPrivate from '../../hooks/useAxiosPrivate';

// Utility for displaying notifications.
import { NotificationHandler } from '../../utils/NotificationHandler'

// Higher Order Component for handling loading state.
import IsLoadingHOC from '../Common/IsLoadingHoc';

// Child components imports.
import VehicleCard from './VehicleCard';
import RecentTrips from './RecentTrips/RecentTrips';
import RecentServices from './RecentServices/RecentServices';

// Component-specific styles.
import './Vehicles.css';

/**
 * The Vehicles component is responsible for displaying user vehicles, 
 * recent trips, and services. It also provides an option to add a new vehicle.
 * 
 * @param {Object} props - Component properties.
 * @param {Function} props.setLoading - Function to set the loading state.
 */
const Vehicles = (props) => {

    // Initializing the axios instance for authenticated requests.
    const axiosPrivate = useAxiosPrivate();

    const { setLoading } = props;

    // State to hold fetched user vehicles.
    const [userVehicles, setUserVehicles] = useState([]);

    // React-router hooks for navigation and location management.
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Effect to fetch user vehicles on component mount.
    useEffect(() => {
        let isMounted = true;  // Flag to handle component unmount scenario.
        const controller = new AbortController();  // For aborting the fetch request.

        const getUserVehicles = async () => {
            try {
                const response = await axiosPrivate.get('/Vehicles', {
                    signal: controller.signal
                });
                isMounted && setUserVehicles(response.data);
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
                // Set loading state to false after data fetching.
                setLoading(false);
            }
        }

        getUserVehicles();

        // Clean up function to handle component unmount.
        return () => {
            isMounted = false;
            controller.abort();
        }
    }, [])

    return (
        <section className="vehicles-section">
            <div className="vehicle-container">
                <div className="user-recent-services-table">
                    <div className="add-vehicle-button">
                        <NavLink to="/Vehicle/Create">Add vehicle</NavLink>
                    </div>
                    <RecentTrips />
                    <RecentServices />
                </div>
                <div className="user-vehicles">
                    {/* Mapping through user vehicles to display each one */}
                    {userVehicles.map(uv => <VehicleCard key={uv.id} vehicleData={uv} />)}
                </div>
            </div>
        </section>
    );
}

// Exporting the component wrapped with the IsLoadingHOC.
export default IsLoadingHOC(Vehicles);
