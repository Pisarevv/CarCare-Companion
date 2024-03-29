// Import utility function to convert ISO date string to a specific format.
import ISODateStringToString from '../../../utils/ISODateStringToString';

// Importing associated styles for this component.
import './RecentServiceCard.css'

/**
 * Represents a card that displays details of a recent service.
 *
 * @param {Object} props - Props passed to the component.
 * @param {Object} props.details - Details of the recent service.
 * @param {string} props.details.title - Title or name of the service.
 * @param {string} props.details.performedOn - Date the service was performed on.
 * @param {string} props.details.vehicleMake - Make of the vehicle.
 * @param {string} props.details.vehicleModel - Model of the vehicle.

 */
const RecentServiceCard = ({details}) => {
    
    // Destructuring service details from the passed prop.
    const {title, performedOn, vehicleMake, vehicleModel} = details;

    // Return the JSX structure for the service card.
    return (
        // Main container for the service card.
        <div className="user-recent-trip-card-container">
            <div className="recent-trip-card-border"></div>          
            <div>"{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)} on {vehicleMake} {vehicleModel} </div>
            <div className="recent-trip-card-border"></div>
        </div>
    )
}

// Exporting the component to be used elsewhere.
export default RecentServiceCard;
