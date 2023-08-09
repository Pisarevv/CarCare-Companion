import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import { NotificationHandler } from '../../../utils/NotificationHandler'

import './DeleteTrip.css'


const DeleteTrip = () => {

  const location = useLocation();
  const { id } = useParams();

  const axiosPrivate = useAxiosPrivate();

  const details = location.state.details;
  const { startDestination, endDestination, vehicleMake, vehicleModel } = details;

  const navigate = useNavigate();


  const onClickHander = async (e) => {
    e.preventDefault();
    try {
      await axiosPrivate.delete(`/Trips/Delete/${id}`);
      navigate("/Trips")
      NotificationHandler("Success", "Sucessfully deleted trip record!", 200);
    }
    catch (error) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      const { title, status } = error.response.data;
      NotificationHandler("Warning", title, status);
    }

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