// React-router's Link component for creating navigation links.
import { Link } from 'react-router-dom';

// Utility function to convert ISO date strings to formatted strings.
import ISODateStringToString from '../../utils/IsoDateStringToString';

// CSS styles specific to this component.
import './ServiceRecordCard.css'

/**
 * ServiceRecordCard is a component that displays details of a single service record.
 * It allows users to view service record details and offers options to edit or delete the service record.
 * 
 * @param {Object} serviceRecordDetails - The details of a single service record.
 */
const ServiceRecordCard = ({ serviceRecordDetails }) => {

    // Destructure the required properties from serviceRecordDetails for cleaner access in the JSX.
    const {
        id, 
        title, 
        performedOn, 
        mileage, 
        description, 
        cost, 
        vehicleMake, 
        vehicleModel
    } = serviceRecordDetails;

    return (
        // The main container for the service record card.
        <div className="service-record-card-container">
            
            // A decorative border at the top of the card.
            <div className="service-record-card-border"></div>
            
            // Displaying the title of the service and the date it was performed on.
            <div>Service "{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)}.</div>
            
            // Displaying the make and model of the vehicle associated with the service record.
            <div>Vehicle: {vehicleMake} {vehicleModel}.</div>
            
            // Displaying the mileage of the vehicle at the time of service.
            <div>Mileage: {mileage} km.</div>
            
            // Displaying the cost of the service.
            <div>Cost: {cost} levs.</div>
            
            // Conditionally rendering the description if it exists.
            {description && <div>Description: {description}</div>}
            
            // A decorative border at the bottom of the card.
            <div className="service-record-card-border"></div>

            // Container for action buttons/links - Edit and Delete.
            <div className="service-record-actions-container">
                // Link to edit the service record.
                <Link to={`/ServiceRecords/Edit/${id}`}>Edit</Link>
                
                // Link to delete the service record. We're passing some necessary details for the deletion process via the 'state' prop.
                <Link
                    to={`/ServiceRecords/Delete/${id}`}
                    state={{ details: { title, performedOn, vehicleMake, vehicleModel } }}
                >
                    Delete
                </Link>
            </div>
        </div>
    )
}

export default ServiceRecordCard;
