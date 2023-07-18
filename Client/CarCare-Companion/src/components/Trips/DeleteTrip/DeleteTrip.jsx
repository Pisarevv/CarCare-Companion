import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';
import './DeleteTrip.css'
import { deleteTrip } from '../../../services/tripService';
import { NotificationHandler } from '../../../utils/NotificationHandler'

const DeleteTrip = () => {

    const location = useLocation();
    const { id } = useParams();

    const details = location.state.details;
    const { startDestination, endDestination, vehicleMake, vehicleModel } = details;

    const navigate = useNavigate();


    const onClickHander = async (e) => {
      e.preventDefault();
      try {
        await deleteTrip(id); 
      } 
      catch (error) {
        NotificationHandler(error)
      }
      navigate("/Trips")
    }

    return (
        <section className="trip-delete-section" >
            <div className="trip-delete-card">
                <div className="trip-delete-card-container">
                    <div>Are you sure you want to delete your trip from {startDestination} to {endDestination} with {vehicleMake} {vehicleModel}?</div>
                </div>
            </div>
            <div className="trip-delete-buttons">
                <Link className="float" to={`/Trips`}>Cancel</Link>
                <button className="float" onClick={onClickHander}>Confirm</button>
            </div>
        </section>
    )
}

export default DeleteTrip;