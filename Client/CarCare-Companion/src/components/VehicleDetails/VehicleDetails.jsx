import { NavLink, useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';

import { ErrorHandler } from '../../utils/ErrorHandler/ErrorHandler';

import IsLoadingHOC from '../Common/IsLoadingHoc';

import { getVehicleDetails } from '../../services/vehicleService';

import './VehicleDetails.css'

const VehicleDetails = (props) => {

    const { setLoading } = props;

    const { id } = useParams();

    const [vehicleDetails, setVehicleDetails] = useState({});


    useEffect(() => {
        (async () => {
            try {
                var vehicleDetailsResult = await getVehicleDetails(id)
                setVehicleDetails(vehicleDetails => vehicleDetailsResult);
                setLoading(false);
            }
            catch (error) {
                ErrorHandler(error);
                setLoading(false);
            }
        })()
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
                            <NavLink to={`/Vehicle/Details/`}>Vehicle details</NavLink>
                        </div>
                    </div>

                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(VehicleDetails);