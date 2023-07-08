import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';

import { getUserVehicles } from '../../services/vehicleService';

import VehicleCard from './VehicleCard';
import RecentTrips from './RecentTrips';

import { ErrorHandler } from "../../utils/ErrorHandler/ErrorHandler";

import IsLoadingHOC from '../Common/IsLoadingHoc';

import './Vehicles.css';

const Vehicles = (props) => {

    const { setLoading } = props;

    const [userVehicles, setUserVehicles] = useState([]);


    useEffect(() => {
        (async () => {
            try {
                const vehicles = await getUserVehicles();
                setUserVehicles(userVehicles => vehicles);
                setLoading(false);
            }
            catch (error) {
                ErrorHandler(error)
                setLoading(false);
            }
        })()
    }, [])


    return (
        <section className="vehicles-section">
            <div className="vehicle-container">
                <div className="create-button"> <NavLink to="/Vehicle/Create">Add vehicle</NavLink></div>
                {/* <div className="user-recent-trips"></div> */}
                <div className="user-recent-services"> test3</div>
                <div className="user-recent-trips"><RecentTrips/></div>
                <div className="user-vehicles"> {userVehicles.map(uv => <VehicleCard key={uv.id} vehicleData={uv} />)}</div>
            </div>
        </section>
    );
}

export default IsLoadingHOC(Vehicles);