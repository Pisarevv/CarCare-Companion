import './RecentTripCard.css'

const RecentTripCard = ({details}) => {
    
    const {startDestination,endDestination,mileageTravelled, vehicle} = details;
    return (
        
        <div className="user-recent-trip-card-container">
            <div className="recent-trip-card-border"></div>
            <div>From {startDestination} to {endDestination}.</div>
            <div>Distance: {mileageTravelled} km</div>
            <div>Vehicle: {vehicle}</div>
            <div className="recent-trip-card-border"></div>
        </div>

    )
}

export default RecentTripCard;