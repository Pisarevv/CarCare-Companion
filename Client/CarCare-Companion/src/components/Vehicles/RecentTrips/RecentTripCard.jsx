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

            {/* Decorative border element */}
            <div className="recent-trip-card-border"></div>

            {/* Display the starting and ending destination of the trip */}
            <div>From {startDestination} to {endDestination}.</div>

            {/* Display the total distance travelled during the trip */}
            <div>Distance: {mileageTravelled} km</div>

            {/* Display the vehicle used for the trip */}
            <div>Vehicle: {vehicle}</div>

            {/* Another decorative border element */}
            <div className="recent-trip-card-border"></div>

        </div>
    )
}

// Export the RecentTripCard component.
export default RecentTripCard;
