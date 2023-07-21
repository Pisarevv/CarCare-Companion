import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

import { deleteTaxRecord } from '../../../services/taxRecordsService';

import { NotificationHandler } from '../../../utils/NotificationHandler'
import ISODateStringToString from '../../../utils/IsoDateStringToString';

import './DeleteTaxRecord.css'

const DeleteTaxRecord = () => {
    const location = useLocation();
    const { id } = useParams();

    const details = location.state.details;
    const { title, validFrom, validTo, vehicleMake, vehicleModel  } = details;

    const navigate = useNavigate();

    const onClickHander = async (e) => {
      e.preventDefault();
      try {
        await deleteTaxRecord(id);
        navigate("/TaxRecords")
        NotificationHandler('Successful record removal');
      } 
      catch (error) {
        NotificationHandler(error)
        navigate(`/TaxRecords`)
      }
    }

    return (
        <section className="tax-record-delete-section" >
            <div className="tax-record-delete-card">
                <div className="tax-record-delete-card-container">
                    <div>Are you sure you want to delete the {title} tax record valid
                     from {ISODateStringToString.ddmmyyyy(validFrom)} to {ISODateStringToString.ddmmyyyy(validTo)} on your {vehicleMake} {vehicleModel}?</div>
                </div>
            </div>
            <div className="tax-record-delete-buttons">
                <Link className="float" to={`/TaxRecords`}>Cancel</Link>
                <button className="float" onClick={onClickHander}>Confirm</button>
            </div>
        </section>
    )
}

export default DeleteTaxRecord;