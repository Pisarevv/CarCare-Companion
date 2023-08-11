// Import required utilities and hooks from React Router, utilities, and utility functions
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

// Custom Axios hook for authenticated requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Utility to handle notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler'

// Utilities to format and validate date strings.
import ISODateStringToString from '../../../utils/IsoDateStringToString';

// CSS styles specific to this component.
import './DeleteServiceRecord.css';

/**
 * DeleteServiceRecord component allows users to delete a specific service record.
 */
const DeleteServiceRecord = () => {
    // Provides access to the current location (route).
    const location = useLocation();

    // Extracts the service record ID from the URL parameters.
    const { id } = useParams();

    // Custom hook for authenticated Axios requests.
    const axiosPrivate = useAxiosPrivate();

    // Extracting service record details passed through route state.
    const details = location.state.details;
    const { title, performedOn, vehicleMake, vehicleModel  } = details;

    // For programmatically navigating to different routes.
    const navigate = useNavigate();

    /**
     * Handler for the click event to delete the service record.
     * 
     * @param {Object} e - Event object.
     */
    const onClickHander = async (e) => {
        e.preventDefault();

        try {
            // Send a delete request to the server for the specified service record.
            await axiosPrivate.delete(`/ServiceRecords/Delete/${id}`);

            // Navigate to the list of service records.
            navigate("/ServiceRecords");

            // Show a success notification.
            NotificationHandler("Success", "Successfully deleted service record!", 200);
        } 
        catch (error) {
            // Scroll to the top of the page smoothly.
            window.scrollTo({ top: 0, behavior: 'smooth' });

            // Extract error details from the server's response.
            const { title, status } = error.response.data;

            // Show a warning notification.
            NotificationHandler("Warning", title, status);
        }
    }

    return (
        <section className="service-record-delete-section">
            <div className="service-record-delete-card">
                <div className="service-record-delete-card-container">
                    <div>
                        Are you sure you want to delete the {title} service performed
                        on {ISODateStringToString.ddmmyyyy(performedOn)} on your {vehicleMake} {vehicleModel}?
                    </div>
                </div>
            </div>
            <div className="service-record-delete-buttons">
                {/* Link to navigate back to the service records page. */}
                <Link className="float" to={`/ServiceRecords`}>Cancel</Link>
                
                {/* Button to confirm the deletion. */}
                <button className="float" onClick={onClickHander}>Confirm</button>
            </div>
        </section>
    )
}

// Export the component.
export default DeleteServiceRecord;
