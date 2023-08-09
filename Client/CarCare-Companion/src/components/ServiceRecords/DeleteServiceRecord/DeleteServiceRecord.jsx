import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import { NotificationHandler } from '../../../utils/NotificationHandler'

import ISODateStringToString from '../../../utils/IsoDateStringToString';

import './DeleteServiceRecord.css'


const DeleteServiceRecord = () => {

    const location = useLocation();

    const { id } = useParams();

    const axiosPrivate = useAxiosPrivate();

    const details = location.state.details;
    const { title, performedOn, vehicleMake, vehicleModel  } = details;

    const navigate = useNavigate();

    const onClickHander = async (e) => {
      e.preventDefault();
      try {
        await axiosPrivate.delete(`/ServiceRecords/Delete/${id}`)
        navigate("/ServiceRecords")
        NotificationHandler("Success", "Sucessfully deleted service record!", 200);
      } 
      catch (error) {
        window.scrollTo({ top: 0, behavior: 'smooth' });
        const {title, status} = error.response.data;
        NotificationHandler("Warning",title,status);
      }
    }

    return (
        <section className="service-record-delete-section" >
            <div className="service-record-delete-card">
                <div className="service-record-delete-card-container">
                    <div>Are you sure you want to delete the {title} service performed
                     on {ISODateStringToString.ddmmyyyy(performedOn)} on your {vehicleMake} {vehicleModel}?</div>
                </div>
            </div>
            <div className="service-record-delete-buttons">
                <Link className="float" to={`/ServiceRecords`}>Cancel</Link>
                <button className="float" onClick={onClickHander}>Confirm</button>
            </div>
        </section>
    )
}

export default DeleteServiceRecord;