// Utility import for converting ISO date strings to a readable format.
import ISODateStringToString from '../../../../utils/ISODateStringToString';

// Component-specific styles.
import './RecentVehicleServiceCard.css'

/**
 * The RecentVehicleServiceCard component represents an individual service record.
 * It takes the details of the service as props and displays them in a formatted manner.
 * 
 * @param {Object} details - Details of the service record.
 * @param {string} details.title - The title or name of the service.
 * @param {string} details.performedOn - ISO date string of when the service was performed.
 * @param {string} details.vehicleMake - The make of the vehicle the service was performed on.
 * @param {string} details.vehicleModel - The model of the vehicle the service was performed on.
 */
const RecentVehicleServiceCard = ({ details }) => {
    
    const { title, performedOn, vehicleMake, vehicleModel } = details;

    return (
        <div className="vehicle-recent-services-card-container">
            {/* Decorative border element */}
            <div className="vehicle-recent-services-card-border"></div>
            
            {/* Displaying the service details with the date converted to a readable format */}
            <div>
                "{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)} on {vehicleMake} {vehicleModel}
            </div>
            
            {/* Another decorative border element */}
            <div className="vehicle-recent-services-card-border"></div>
        </div>
    )
}

// Exporting the RecentVehicleServiceCard component.
export default RecentVehicleServiceCard;
