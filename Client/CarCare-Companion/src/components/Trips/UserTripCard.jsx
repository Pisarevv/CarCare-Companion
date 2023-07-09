import './UserTripCard.css'

const UserTripCard = ({tripDetails}) => {
    
    const {dateCreated, startDestination,endDestination,mileageTravelled,fuelPrice, usedFuel, tripCost, vehicleMake, vehicleModel} = tripDetails;
    return (
        
        <div className="user-trip-card-container">
            <div className="trip-card-border"></div>
            <div>Trip on: {dateCreated}. From {startDestination} to {endDestination}.</div>
            <div>Distance: {mileageTravelled} km</div>
            {usedFuel &&  <div>Used fuel (liters): {usedFuel} levs.</div>}  
            {fuelPrice && <div>Fuel price during trip: {fuelPrice} levs.</div>}
            {tripCost && <div>Trip cost: {tripCost} levs.</div>}
            <div>Vehicle: {vehicleMake} {vehicleModel}</div>
            <div className="trip-card-border"></div>
        </div>

    )
}

export default UserTripCard;