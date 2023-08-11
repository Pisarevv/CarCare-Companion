// Required React imports.
import { useEffect, useState } from 'react';

// React-router imports for navigation and routing.
import { NavLink, useLocation, useNavigate } from 'react-router-dom';

// Child components imports.
import VehicleCard from './VehicleCard';
import RecentTrips from './RecentTrips/RecentTrips';
import RecentServices from './RecentServices/RecentServices';

// Utility for displaying notifications.
import { NotificationHandler } from '../../utils/NotificationHandler'

// Custom hook for making authenticated axios requests.
import useAxiosPrivate from '../../hooks/useAxiosPrivate';

// Higher Order Component for handling loading state.
import IsLoadingHOC from '../Common/IsLoadingHoc';

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
    const navigate = useNavigate();
    const location = useLocation();

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
                // Handling errors and redirecting to login in case of failure.
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
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
                    <RecentTrips/>
                    <RecentServices/>
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
