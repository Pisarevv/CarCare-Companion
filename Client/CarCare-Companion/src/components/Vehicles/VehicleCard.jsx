// Importing NavLink for routing purposes.
import { NavLink } from 'react-router-dom';

// Importing component-specific styles.
import './VehicleCard.css'

/**
 * The VehicleCard component is responsible for displaying individual vehicle details.
 * 
 * @param {Object} vehicleData - The data associated with a vehicle.
 * @param {string} vehicleData.model - The model of the vehicle.
 * @param {string} vehicleData.make - The make/brand of the vehicle.
 * @param {string} vehicleData.imageUrl - URL for the vehicle's image.
 * @param {string} vehicleData.id - Unique ID of the vehicle.
 */
const VehicleCard = ({ vehicleData }) => {
    
    // Destructuring properties from the vehicleData prop.
    const { model, make, imageUrl, id } = vehicleData;

    return (
        <div className="vehicle-card">
            {/* Rendering the image from the URL if available, otherwise a default image. */}
            {imageUrl 
                ? <img src={imageUrl} alt="Vehicle" className="vehicle-image"></img> 
                : <img src='/car-logo.png' alt="Default Vehicle" className="vehicle-image"></img> 
            }
            <div className="vehicle-card-container">
                {/* Displaying the make and model of the vehicle. */}
                <div className="vehicle-make">{make}</div>
                <div className="vehicle-model">{model}</div>

                {/* Link to detailed page for the specific vehicle. */}
                <NavLink to={`/Vehicle/Details/${id}`}>Vehicle details</NavLink>
            </div>
        </div>
    )
}

// Exporting the VehicleCard component.
export default VehicleCard;
