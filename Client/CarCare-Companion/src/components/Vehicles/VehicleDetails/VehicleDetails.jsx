import { Link, NavLink, useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';

import { NotificationHandler } from '../../../utils/NotificationHandler'

import IsLoadingHOC from '../../Common/IsLoadingHoc';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import './VehicleDetails.css'


const VehicleDetails = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const { id } = useParams();

    const [vehicleDetails, setVehicleDetails] = useState({});


    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getVehicleDetails= async () => {
            try {
                const response = await axiosPrivate.get(`/Vehicles/Details/${id}`, {
                    signal: controller.signal
                });
                isMounted && setVehicleDetails(vehicleDetails => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getVehicleDetails();

        return () => {
            isMounted = false;
            controller.abort();
        }
    }, [])

    
    return (
        <section className="vehicle-details">
            <div className="vehicle-details-container">
                <div className="management-buttons">
                    <NavLink to={`/Vehicle/Details`}>Trip manager</NavLink>
                    <NavLink to={`/Vehicle/Details`}>Service manager</NavLink>
                </div>
                <div className="last-three-taxes-container"> TEST2</div>
                <div className="last-three-trips-container">TEST3 </div>
                <div className="vehicle-information">
                    <div className="vehicl-details-card">
                        {
                            vehicleDetails.imageUrl
                                ? <img src={vehicleDetails.imageUrl} className="vehicle-details-image"></img>
                                :
                                <img src='/car-logo.png' className="vehicle-details-image"></img>
                        }
                        <div className="vehicle-details-card-container">
                            <div className="vehicle-details-information">Make: {vehicleDetails.make}</div>
                            <div className="vehicle-details-information">Model: {vehicleDetails.model}</div>
                            <div className="vehicle-details-information">Mileage: {vehicleDetails.mileage}</div>
                            <div className="vehicle-details-information">Fuel: {vehicleDetails.fuelType}</div>
                            <div className="vehicle-details-information">Type: {vehicleDetails.vehicleType}</div>
                            <NavLink to={`/Vehicle/Edit/${id}`}>Edit</NavLink>
                            <Link 
                            to={`/Vehicle/Delete/${id}`}
                            state = {{details : {make:vehicleDetails.make, model:vehicleDetails.model}}}
                            >Delete
                            </Link>
                        </div>
                    </div>

                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(VehicleDetails);