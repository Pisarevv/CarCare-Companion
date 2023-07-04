import { NavLink } from 'react-router-dom';
import './VehicleCard.css'

const VehicleCard = ({vehicleData}) => {
    
    const { model, make, imageUrl, id } = vehicleData;
    return (
        <div className="vehicle-card">
            <img src={imageUrl} className="vehicle-image"></img>
            <div className="vehicle-card-container">
                <div className="vehicle-make">{make}</div>
                <div className="vehicle-model">{model}</div>
                <NavLink to="/Vehicle/Create">Vehicle details</NavLink>
            </div>

        </div>
    )
}


export default VehicleCard;