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
            <div className="service-record-card-border"></div>          
            <div>Service "{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)}.</div>         
            <div>Vehicle: {vehicleMake} {vehicleModel}.</div>         
            <div>Mileage: {mileage} km.</div>          
            <div>Cost: {cost} levs.</div>          
            {description && <div>Description: {description}</div>}      
            <div className="service-record-card-border"></div>

            <div className="service-record-actions-container">
                <Link to={`/ServiceRecords/Edit/${id}`}>Edit</Link>
                
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
