import { Link } from 'react-router-dom';

import ISODateStringToString from '../../utils/IsoDateStringToString';

import './ServiceRecordCard.css'

const ServiceRecordCard = ({ serviceRecordDetails }) => {

    const {id, title, performedOn, mileage, description, cost, vehicleMake, vehicleModel } = serviceRecordDetails;
    return (

        <div className="service-record-card-container">
            <div className="service-record-card-border"></div>
            <div>Service "{title}" performed on {ISODateStringToString.ddmmyyyy(performedOn)}.</div>
            <div>Vehicle: {vehicleMake} {vehicleModel}.</div>
            <div>Mileage: {mileage} km.</div>
            <div>Cost: {cost} levs.</div>
            {description && <div>Description: {description}</div>}
            <div className="service-record-card-border"></div>
            <div className="service-record-actions-container">
                <Link to={`/ServiceRecords/Edit/${id}`}>Edit</Link>
                <Link
                    to={`/ServiceRecords/Delete/${id}`}
                    state={{ details: { title, performedOn, vehicleMake, vehicleModel } }}
                >Delete
                </Link>
            </div>
        </div>

    )
}

export default ServiceRecordCard;