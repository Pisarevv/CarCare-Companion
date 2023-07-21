import { Link } from 'react-router-dom';

import ISODateStringToString from '../../utils/IsoDateStringToString';

import './TaxRecordCard.css'

const TaxRecordCard = ({ taxRecordDetails }) => {

    const {id, title, validFrom, validTo, description, cost, vehicleMake, vehicleModel } = taxRecordDetails;
    return (

        <div className="tax-record-card-container">
            <div className="tax-record-card-border"></div>
            <div>Tax: "{title}".</div>
            <div>Valid from: {ISODateStringToString.ddmmyyyy(validFrom)}</div>
            <div>Valid to: {ISODateStringToString.ddmmyyyy(validTo)}</div>
            <div>Vehicle: {vehicleMake} {vehicleModel}.</div>
            <div>Cost: {cost} levs.</div>
            {description && <div>Description: {description}</div>}
            <div className="tax-record-card-border"></div>
            <div className="tax-record-actions-container">
                <Link to={`/TaxRecords/Edit/${id}`}>Edit</Link>
                <Link
                    to={`/TaxRecords/Delete/${id}`}
                    state={{ details: { title, validFrom, validTo, vehicleMake, vehicleModel } }}
                >Delete
                </Link>
            </div>
        </div>

    )
}

export default TaxRecordCard;