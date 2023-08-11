// Importing necessary hooks and components from React and React Router.
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

// Custom hook for authenticated API requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Utility function for handling notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Component-specific styling.
import './VehicleDelete.css';

// VehicleDelete component that provides functionality to delete a vehicle.
const VehicleDelete = () => {
  // Hooks to get current location, navigate, and fetch URL parameter.
  const location = useLocation();
  const { id } = useParams();
  const navigate = useNavigate();

  // Instance of the custom axios hook for authenticated requests.
  const axiosPrivate = useAxiosPrivate();

  // Retrieving the vehicle details passed through the location state.
  const details = location.state.details;
  const { make, model } = details;

  // Event handler for the confirm delete button.
  const onClickHander = async (e) => {
    e.preventDefault(); // Preventing default form behavior.

    try {
      // Attempting to delete the vehicle using an authenticated API request.
      await axiosPrivate.delete(`/Vehicles/Delete/${id}`);
      navigate("/MyVehicles"); // Navigating to the MyVehicles page upon successful deletion.
      NotificationHandler("Success", "Successfully deleted vehicle!", 200); // Display success notification.
    }
    catch (error) {
      // Scroll to the top smoothly in case of error.
      window.scrollTo({ top: 0, behavior: 'smooth' });

      // Extracting error details from the response.
      const { title, status } = error.response.data;

      // Displaying an error notification.
      NotificationHandler("Warning", title, status);
    }
  };

  // Component JSX that displays a warning about deletion and provides options to confirm or cancel the operation.
  return (
    <section className="vehicle-delete-section">
      <div className="vehicle-delete-card">
        <div className="vehicle-delete-card-container">
          <div>Are you sure you want to delete your {make} {model}?</div>
          <div>All the trip, service, and tax records related to the vehicle will be deleted.</div>
        </div>
      </div>
      <div className="vehicle-delete-buttons">
        <Link className="float" to={`/Vehicle/Details/${id}`}>Cancel</Link>
        <button className="float" onClick={onClickHander}>Confirm</button>
      </div>
    </section>
  );
}

export default VehicleDelete;
