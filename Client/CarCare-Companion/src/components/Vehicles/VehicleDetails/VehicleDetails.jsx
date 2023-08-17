// Required React imports.
import { useEffect, useState } from 'react';

// React-router imports for navigation, routing, and extracting route parameters.
import { Link, useParams, useLocation } from 'react-router-dom';

// Utility for displaying notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler'

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../hooks/useDeauthenticate';

// Higher Order Component for handling loading state.
import IsLoadingHOC from '../../Common/IsLoadingHoc';

// Custom hook for making authenticated axios requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Component-specific styles.
import './VehicleDetails.css'

// Child component imports.
import RecentVehicleServices from './RecentVehicleServices/RecentVehicleServices';

/**
 * The VehicleDetails component is responsible for displaying detailed information 
 * about a specific vehicle, including its image, make, model, mileage, fuel type, and vehicle type.
 * It also provides options to edit or delete the vehicle.
 * 
 * @param {Object} props - Component properties.
 * @param {Function} props.setLoading - Function to set the loading state.
 */
const VehicleDetails = (props) => {

    const { setLoading } = props;

    // Initializing the axios instance for authenticated requests.
    const axiosPrivate = useAxiosPrivate();

    // Provides access to the current location (route).
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Extracting the vehicle ID from the URL parameters.
    const { id } = useParams();

    // State to hold the fetched vehicle details.
    const [vehicleDetails, setVehicleDetails] = useState({});

    // Effect to fetch vehicle details based on the provided ID.
    useEffect(() => {
        let isMounted = true;  // Flag to handle component unmount scenario.
        const controller = new AbortController();  // For aborting the fetch request.

        const getVehicleDetails = async () => {
            try {
                const response = await axiosPrivate.get(`/Vehicles/Details/${id}`, {
                    signal: controller.signal
                });
                isMounted && setVehicleDetails(vehicleDetails => response.data);
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

        getVehicleDetails();

        // Clean up function to handle component unmount.
        return () => {
            isMounted = false;
            controller.abort();
        }
    }, [])

    return (
        <section className="vehicle-details">
            <div className="vehicle-details-container">
                <div className="recent-actions-information">
                    <RecentVehicleServices />
                </div>
                <div className="vehicle-information">
                    <div className="vehicl-details-card">
                        {/* Rendering the vehicle image or a default one if not available. */}
                        {vehicleDetails.imageUrl
                            ? <img src={vehicleDetails.imageUrl} alt="Vehicle" className="vehicle-details-image"></img>
                            : <img src='/car-logo.png' alt="Default Vehicle" className="vehicle-details-image"></img>
                        }
                        <div className="vehicle-details-card-container">
                            {/* Displaying the vehicle details. */}
                            <div className="vehicle-details-information">Make: {vehicleDetails.make}</div>
                            <div className="vehicle-details-information">Model: {vehicleDetails.model}</div>
                            <div className="vehicle-details-information">Mileage: {vehicleDetails.mileage} km</div>
                            <div className="vehicle-details-information">Fuel: {vehicleDetails.fuelType}</div>
                            <div className="vehicle-details-information">Type: {vehicleDetails.vehicleType}</div>
                            {/* Edit and Delete links for the vehicle. */}
                            <div className="vehicle-details-buttons">
                                <Link to={`/Vehicle/Edit/${id}`}>Edit</Link>
                                <Link
                                    to={`/Vehicle/Delete/${id}`}
                                    state={{ details: { make: vehicleDetails.make, model: vehicleDetails.model } }}
                                >Delete
                                </Link>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}

// Exporting the component wrapped with the IsLoadingHOC.
export default IsLoadingHOC(VehicleDetails);
