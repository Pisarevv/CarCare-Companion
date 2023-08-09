import { Link, NavLink, useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';

import { NotificationHandler } from '../../../utils/NotificationHandler'

import IsLoadingHOC from '../../Common/IsLoadingHoc';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import './VehicleDetails.css'
import RecentVehicleServices from './RecentVehicleServices/RecentVehicleServices';


const VehicleDetails = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const { id } = useParams();

    const [vehicleDetails, setVehicleDetails] = useState({});


    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getVehicleDetails = async () => {
            try {
                const response = await axiosPrivate.get(`/Vehicles/Details/${id}`, {
                    signal: controller.signal
                });
                isMounted && setVehicleDetails(vehicleDetails => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
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
                <div className="recent-actions-information"> 
                <RecentVehicleServices/>
                </div>
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
                            <div className="vehicle-details-information">Mileage: {vehicleDetails.mileage} km</div>
                            <div className="vehicle-details-information">Fuel: {vehicleDetails.fuelType}</div>
                            <div className="vehicle-details-information">Type: {vehicleDetails.vehicleType}</div>
                            <div className="vehicle-details-buttons">
                            <Link to={`/Vehicle/Edit/${id}`}>Edit</Link>
                            <Link
                                to={`/Vehicle/Delete/${id}`}
                                state={{ details: { make: vehicleDetails.make, model: vehicleDetails.model } }}
                            >Delete
                            </Link></div>
                        </div>
                    </div>

                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(VehicleDetails);