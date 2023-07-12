import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';

import { getUserVehicles } from '../../services/vehicleService';

import VehicleCard from './VehicleCard';


import { ErrorHandler } from "../../utils/ErrorHandler/ErrorHandler";

import IsLoadingHOC from '../Common/IsLoadingHoc';

import './Vehicles.css';
import { getLatestTrips } from '../../services/tripService';
import RecentTrips from './RecentTrips/RecentTrips';

const Vehicles = (props) => {

    const { setLoading } = props;

    const [userVehicles, setUserVehicles] = useState([]);
    const [recentUserTrips, setRecentUserTrips] = useState([]);

    console.log(userVehicles)
    console.log(recentUserTrips)

    useEffect(() => {
        (async () => {
            try {
                const vehicles = await getUserVehicles();
                const trips = await getLatestTrips(3);
                setUserVehicles(userVehicles => vehicles);
                setRecentUserTrips(recentUserTrips => trips);
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
                <div className="user-recent-services-table">
                    <NavLink to="/Vehicle/Create">Add vehicle</NavLink>
                    <RecentTrips recentTrips={recentUserTrips} />
                    <div>test</div>
                </div>
                <div className="user-vehicles"> {userVehicles.map(uv => <VehicleCard key={uv.id} vehicleData={uv} />)}</div>
            </div>  
            
        </section>
    );
}

export default IsLoadingHOC(Vehicles);