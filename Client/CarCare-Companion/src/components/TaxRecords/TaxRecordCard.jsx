// Importing the necessary components and utilities from react-router-dom
import { Link } from 'react-router-dom';

// Utility for converting ISO date strings to human-readable format
import ISODateStringToString from '../../utils/ISODateStringToString';

// Component-specific styles
import './TaxRecordCard.css'

/**
 * TaxRecordCard component renders the details of a given tax record in a card format.
 *
 * @param {Object} taxRecordDetails - An object containing the details of the tax record.
 */
const TaxRecordCard = ({ taxRecordDetails }) => {

    // Destructuring the necessary details from the taxRecordDetails prop
    const { id, title, validFrom, validTo, description, cost, vehicleMake, vehicleModel } = taxRecordDetails;

    return (
        <div className="tax-record-card-container">

            <div className="tax-record-card-border"></div>

            {/* Displaying tax title */}
            <div>Tax: "{title}".</div>

            {/* Displaying the valid from and valid to dates in a formatted way */}
            <div>Valid from: {ISODateStringToString.ddmmyyyy(validFrom)}</div>
            <div>Valid to: {ISODateStringToString.ddmmyyyy(validTo)}</div>

            {/* Displaying the vehicle details associated with the tax record */}
            <div>Vehicle: {vehicleMake} {vehicleModel}.</div>

            {/* Displaying the cost */}
            <div>Cost: {cost} levs.</div>

            {/* Conditionally rendering the description if it exists */}
            {description && <div>Description: {description}</div>}

            <div className="tax-record-card-border"></div>

            {/* Links for editing and deleting the tax record */}
            <div className="tax-record-actions-container">
                <Link to={`/TaxRecords/Edit/${id}`}>Edit</Link>
                <Link
                    to={`/TaxRecords/Delete/${id}`}
                    // Passing additional details as state to the delete route for further use
                    state={{ details: { title, validFrom, validTo, vehicleMake, vehicleModel } }}
                >Delete
                </Link>
            </div>
        </div>
    )
}

export default TaxRecordCard;
