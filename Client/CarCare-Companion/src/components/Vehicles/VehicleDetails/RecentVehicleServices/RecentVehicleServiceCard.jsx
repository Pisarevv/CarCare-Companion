import ISODateStringToString from '../../../../utils/IsoDateStringToString';
import './RecentVehicleServiceCard.css'

const RecentVehicleServiceCard = ({details}) => {
    
    const {title ,performedOn,vehicleMake, vehicleModel} = details;
    return (
        
        <div className="vehicle-recent-services-card-container">
            <div className="vehicle-recent-services-card-border"></div>
            <div>"{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)} on {vehicleMake} {vehicleModel} </div>
            <div className="vehicle-recent-services-card-border"></div>
        </div>

    )
}

export default RecentVehicleServiceCard;