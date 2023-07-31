import ISODateStringToString from '../../../utils/IsoDateStringToString';
import './RecentServiceCard.css'

const RecentServiceCard = ({details}) => {
    
    const {title ,performedOn,vehicleMake, vehicleModel} = details;
    return (
        
        <div className="user-recent-trip-card-container">
            <div className="recent-trip-card-border"></div>
            <div>"{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)} on {vehicleMake} {vehicleModel} </div>
            <div className="recent-trip-card-border"></div>
        </div>

    )
}

export default RecentServiceCard;