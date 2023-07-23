import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';

import { getUserVehicles } from '../../services/vehicleService';

import VehicleCard from './VehicleCard';


import { NotificationHandler } from '../../utils/NotificationHandler'

import IsLoadingHOC from '../Common/IsLoadingHoc';

import './Vehicles.css';

import RecentTrips from './RecentTrips/RecentTrips';
import RecentServices from './RecentServices/RecentServices';

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
                NotificationHandler(error)
                setLoading(false);
            }
        })()
    }, [])


    return (
        <section className="vehicles-section">
            <div className="vehicle-container">
                <div className="user-recent-services-table">
                    <NavLink to="/Vehicle/Create">Add vehicle</NavLink>
                    <RecentTrips/>
                    <RecentServices/>
                </div>
                <div className="user-vehicles"> {userVehicles.map(uv => <VehicleCard key={uv.id} vehicleData={uv} />)}</div>
            </div>  
            
        </section>
    );
}

export default IsLoadingHOC(Vehicles);