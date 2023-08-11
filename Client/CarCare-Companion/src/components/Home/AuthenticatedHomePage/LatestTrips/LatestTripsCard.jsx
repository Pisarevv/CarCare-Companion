// CSS styles specific to this component.
import './LatestTripsCard.css';

/**
 * LatestTripsCard Component
 *
 * This component is designed to display details of a single trip record.
 * It presents the trip's start and end destinations, the distance covered, 
 * and the vehicle used.
 *
 * Props:
 * - details: An object containing the specifics of the trip. It includes:
 *   - startDestination: Starting point of the trip.
 *   - endDestination: Ending point of the trip.
 *   - mileageTravelled: Distance covered during the trip (in kilometers).
 *   - vehicle: The vehicle used for the trip.
 */
const LatestTripsCard = ({ details }) => {
    
    // Destructure the trip details from the props.
    const { startDestination, endDestination, mileageTravelled, vehicle } = details;

    return (
        <div className="home-latest-trip-card-container">
            <div className="home-latest-trip-card-border"></div>
            <div>From {startDestination} to {endDestination}.</div>
            <div>Distance: {mileageTravelled} km</div>
            <div>Vehicle: {vehicle}</div>
            <div className="home-latest-trip-card-border"></div>
        </div>
    );
}

export default LatestTripsCard;