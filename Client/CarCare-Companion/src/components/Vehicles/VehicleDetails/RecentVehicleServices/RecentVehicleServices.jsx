// Required React imports.
import { useEffect, useState } from 'react';

// React-router imports for extracting route parameters.
import { useParams } from 'react-router-dom';

// Custom hook for making authenticated axios requests.
import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

// Child component import.
import RecentVehicleServiceCard from './RecentVehicleServiceCard';

// Utility for displaying notifications.
import { NotificationHandler } from '../../../../utils/NotificationHandler';

// Higher Order Component for handling loading state.
import IsLoadingHOC from '../../../Common/IsLoadingHoc';

// Component-specific styles.
import './RecentVehicleServices.css'

/**
 * The RecentVehicleServices component is responsible for fetching and displaying
 * the recent service records of a vehicle. It will display each service record using 
 * the RecentVehicleServiceCard component. If there are no records, a message is shown to the user.
 * 
 * @param {Object} props - Component properties.
 * @param {Function} props.setLoading - Function to set the loading state.
 */
const RecentVehicleServices = (props) => {
 
    const { setLoading } = props;

    // Extracting the vehicle ID from the URL parameters.
    const { id } = useParams();

    // Initializing the axios instance for authenticated requests.
    const axiosPrivate = useAxiosPrivate();

    // State to hold the fetched recent service records.
    const [recentServiceRecords, setRecentServiceRecords] = useState([]);

    // Effect to fetch recent service records based on the provided vehicle ID.
    useEffect(() => {
        let isMounted = true;  // Flag to handle component unmount scenario.
        const controller = new AbortController();  // For aborting the fetch request.

        const getRecentVehicleServices = async () => {
            try {
                // The endpoint fetches the last three service records for the vehicle.
                const response = await axiosPrivate.get(`/ServiceRecords/${id}/Last/${3}`, {
                    signal: controller.signal
                });
                isMounted && setRecentServiceRecords(recentServiceRecords => response.data);
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

        getRecentVehicleServices();

        // Clean up function to handle component unmount.
        return () => {
            isMounted = false;
            controller.abort();
        }
    }, [])

    
    return (
        <div className="vehicle-recent-services-container">
            <div className="vehicle-services-header">Recent service records</div>
            <div className="vehicle-services-list">
                {/* Displaying recent service records or a message if no records exist. */}
                {
                    recentServiceRecords.length > 0
                        ? recentServiceRecords.map(rs => <RecentVehicleServiceCard key={rs.id} details={rs} />)
                        : <div>You haven't added any service records for this vehicle yet.</div>
                }
            </div>
        </div>
    )
}

// Exporting the component wrapped with the IsLoadingHOC.
export default IsLoadingHOC(RecentVehicleServices);
