// React Router's Link component for navigation.
import { Link } from 'react-router-dom';

// Utility import for converting ISO date strings to a readable format.
import ISODateStringToString from '../../utils/IsoDateStringToString';

// Component-specific styles.
import './UserTripCard.css';

/**
 * The UserTripCard component displays details about a specific trip taken by a user.
 * It showcases details like date, start and end destinations, mileage, fuel statistics, 
 * and the vehicle involved. Additionally, it provides links for editing and deleting the trip.
 * 
 * @param {Object} tripDetails - Details of the user's trip.
 * @param {стринг} tripDetails.id - The ID of the trip.
 * @param {string} tripDetails.dateCreated - ISO date string of when the trip was created.
 * @param {string} tripDetails.startDestination - The starting point of the trip.
 * @param {string} tripDetails.endDestination - The destination point of the trip.
 * @param {number} tripDetails.mileageTravelled - Distance travelled during the trip.
 * @param {number} tripDetails.fuelPrice - Price of the fuel during the trip.
 * @param {number} tripDetails.usedFuel - Amount of fuel used for the trip.
 * @param {number} tripDetails.tripCost - Total cost of the trip.
 * @param {string} tripDetails.vehicleMake - The make of the vehicle used.
 * @param {string} tripDetails.vehicleModel - The model of the vehicle used.
 */
const UserTripCard = ({ tripDetails }) => {
    
    // Destructuring trip details for easier access and cleaner JSX.
    const {id, dateCreated, startDestination, endDestination, mileageTravelled, fuelPrice, usedFuel, tripCost, vehicleMake, vehicleModel } = tripDetails;

    return (
        <div className="user-trip-card-container">
            <div className="trip-card-border"></div>
            <div>Trip on {ISODateStringToString.ddmmyyyy(dateCreated)} from {startDestination} to {endDestination}.</div>
            <div>Distance: {mileageTravelled} km</div>
            {usedFuel && <div>Used fuel (liters): {usedFuel} liters.</div>}
            {fuelPrice && <div>Fuel price during trip: {fuelPrice} levs.</div>}
            {tripCost && <div>Trip cost: {tripCost} levs.</div>}
            <div>Vehicle: {vehicleMake} {vehicleModel}</div>
            <div className="trip-card-border"></div>
            <div className="trip-actions-container">
                <Link to={`/Trips/Edit/${id}`}>Edit</Link>
                <Link
                    to={`/Trips/Delete/${id}`}
                    state={{ details: { startDestination, endDestination, vehicleMake, vehicleModel } }}
                >Delete
                </Link>
            </div>
        </div>
    )
}

// Exporting the UserTripCard component.
export default UserTripCard;
