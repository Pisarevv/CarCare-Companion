import { Link, useLocation, useNavigate, useParams } from 'react-router-dom';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import { NotificationHandler } from '../../../utils/NotificationHandler'

import './VehicleDelete.css'


const VehicleDelete = () => {
    const location = useLocation();
    const { id } = useParams();

    const axiosPrivate = useAxiosPrivate();

    const details = location.state.details;
    const { make, model } = details;

    const navigate = useNavigate();

   

    const onClickHander = async (e) => {
      e.preventDefault();
      try {
        await axiosPrivate.delete(`/Vehicles/Delete/${id}`);
        navigate("/MyVehicles")
      } 
      catch (error) {
        NotificationHandler(error)
        navigate(`/Vehicle/Details/${id}`)
      }
    }

    return (
        <section className="vehicle-delete-section" >
            <div className="vehicle-delete-card">
                <div className="vehicle-delete-card-container">
                    <div>Are you sure you want to delete your {make} {model}?</div>
                    <div>All the trip,service and tax records related to the vehicle will be deleted.</div>
                </div>
            </div>
            <div className="vehicle-delete-buttons">
                <Link className="float" to={`/Vehicle/Details/${id}`}>Cancel</Link>
                <button className="float" onClick={onClickHander}>Confirm</button>
            </div>
        </section>
    )
}

export default VehicleDelete;