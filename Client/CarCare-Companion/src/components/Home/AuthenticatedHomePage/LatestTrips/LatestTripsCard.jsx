import './LatestTripsCard.css'

const LatestTripsCard = ({details}) => {
    
    const {startDestination,endDestination,mileageTravelled, vehicle} = details;
    return (
        
        <div className="home-latest-trip-card-container">
            <div className="home-latest-trip-card-border"></div>
            <div>From {startDestination} to {endDestination}.</div>
            <div>Distance: {mileageTravelled} km</div>
            <div>Vehicle: {vehicle}</div>
            <div className="home-latest-trip-card-border"></div>
        </div>

    )
}

export default LatestTripsCard;