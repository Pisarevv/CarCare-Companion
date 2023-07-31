import ISODateStringToString from '../../../../utils/IsoDateStringToString';

import './UpcomingTaxCard.css'

const UpcomingTaxCard = ({details}) => {
    
    const {title,validTo,vehicleMake, vehicleModel} = details;
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