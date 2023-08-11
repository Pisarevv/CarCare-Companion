// Utility function to convert ISO date strings to a specific format.
import ISODateStringToString from '../../../../utils/IsoDateStringToString';

// CSS styles specific to this component.
import './UpcomingTaxCard.css'

/**
 * UpcomingTaxCard Component
 *
 * This component displays the details of an upcoming tax as a card.
 *
 * Props:
 *     - details: An object containing details about the upcoming tax.
 *     - title: The title or name of the tax.
 *     - validTo: The expiration date of the tax in ISO string format.
 *     - vehicleMake: The make of the vehicle associated with the tax.
 *     - vehicleModel: The model of the vehicle associated with the tax.
 */
const UpcomingTaxCard = ({details}) => {
    
    // Destructure the details prop to extract individual properties.
    const {title, validTo, vehicleMake, vehicleModel} = details;
    return (
        
        <div className="upcoming-tax-card-container">
            <div className="upcoming-tax-card-border"></div>
            <div>Tax: "{title}"</div>
            <div>Valid to: {ISODateStringToString.ddmmyyyy(validTo)}</div>
            <div>Vehicle: {vehicleMake} {vehicleModel}</div>
            <div className="upcoming-tax-card-border"></div>
        </div>
    )
}

export default UpcomingTaxCard;