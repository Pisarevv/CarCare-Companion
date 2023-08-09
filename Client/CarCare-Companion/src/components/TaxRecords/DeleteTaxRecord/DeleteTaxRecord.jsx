import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import { NotificationHandler } from '../../../utils/NotificationHandler'
import ISODateStringToString from '../../../utils/IsoDateStringToString';

import './DeleteTaxRecord.css'


const DeleteTaxRecord = () => {
  
    const location = useLocation();

    const { id } = useParams();

    const axiosPrivate = useAxiosPrivate();

    const details = location.state.details;
    const { title, validFrom, validTo, vehicleMake, vehicleModel  } = details;

    const navigate = useNavigate();

    const onClickHander = async (e) => {
      e.preventDefault();
      try {
        await axiosPrivate.delete(`/TaxRecords/Delete/${id}`);
        navigate("/TaxRecords")
        NotificationHandler("Success", "Sucessfully deleted tax record!", 200);
      } 
      catch (error) {
        window.scrollTo({ top: 0, behavior: 'smooth' });
        const { title, status } = error.response.data;
        NotificationHandler("Warning", title, status);
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