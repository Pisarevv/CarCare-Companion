// React-router hooks for accessing route parameters, the current location, and navigating.
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

// Custom Axios hook for making authenticated requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Helper utility to handle notifications (e.g., error messages).
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Utility function for converting ISO date strings to human-readable format.
import ISODateStringToString from '../../../utils/IsoDateStringToString';

// Component specific styles.
import './DeleteTaxRecord.css';

/**
 * DeleteTaxRecord component allows users to delete a specific tax record.
 * It fetches details of the tax record and prompts the user for confirmation before deletion.
 */
const DeleteTaxRecord = () => {

    // Hook to access the current route's state and parameters.
    const location = useLocation();
    const { id } = useParams();

    // Axios instance for authenticated requests.
    const axiosPrivate = useAxiosPrivate();

    // Extracting details from the route's state.
    const details = location.state.details;
    const { title, validFrom, validTo, vehicleMake, vehicleModel } = details;

    // Hook for programmatic navigation.
    const navigate = useNavigate();

    /**
     * Handler for the confirm button click event.
     * Tries to delete the specified tax record and handles success and error scenarios.
     */
    const onClickHander = async (e) => {
        e.preventDefault();
        try {
            await axiosPrivate.delete(`/TaxRecords/Delete/${id}`);
            navigate("/TaxRecords");  // Redirects to the tax records page on success.
            NotificationHandler("Success", "Sucessfully deleted tax record!", 200);
        } catch (error) {
            // Scrolls the window to the top, typically to show an error message.
            window.scrollTo({ top: 0, behavior: 'smooth' });
            const { title, status } = error.response.data;
            NotificationHandler("Warning", title, status);
        }
    }

    return (
        <section className="tax-record-delete-section">
            <div className="tax-record-delete-card">
                <div className="tax-record-delete-card-container">
                    <div>
                        Are you sure you want to delete the {title} tax record valid 
                        from {ISODateStringToString.ddmmyyyy(validFrom)} to {ISODateStringToString.ddmmyyyy(validTo)}
                        on your {vehicleMake} {vehicleModel}?
                    </div>
                </div>
            </div>
            <div className="tax-record-delete-buttons">
                <Link className="float" to={`/TaxRecords`}>Cancel</Link>  {/* Link to navigate back without making changes. */}
                <button className="float" onClick={onClickHander}>Confirm</button>  {/* Button to confirm the delete operation. */}
            </div>
        </section>
    )
}

export default DeleteTaxRecord;
