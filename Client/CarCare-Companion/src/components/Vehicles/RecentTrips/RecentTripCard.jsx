// Component-specific styles.
import './RecentTripCard.css'

/**
 * The RecentTripCard component renders details of a specific trip.
 * 
 * @param {Object} props - Props passed to the component.
 * @param {Object} props.details - Contains details about the trip such as starting point, ending point, distance, and vehicle used.
 */
const RecentTripCard = ({ details }) => {

    // Destructure necessary trip details from the `details` prop.
    const {
        startDestination,
        endDestination,
        mileageTravelled,
        vehicle
    } = details;

    return (
        // Main container for the recent trip card.
        <div className="user-recent-trip-card-container-vehicle">

            <div className="recent-trip-card-border"></div>
            <div>From {startDestination} to {endDestination}.</div>
            <div>Distance: {mileageTravelled} km</div>
            <div>Vehicle: {vehicle}</div>
            <div className="recent-trip-card-border"></div>

        </div>
    )
}

// Export the RecentTripCard component.
export default RecentTripCard;
