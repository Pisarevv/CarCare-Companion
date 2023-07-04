import { NavLink, useNavigate } from 'react-router-dom';

import './Vehicles.css';
import { useEffect, useState } from 'react';
import { getUserVehicles } from '../../services/vehicleService';
import VehicleCard from './VehicleCard';

const Vehicles = () => {

    const navigate = useNavigate();

    const [userVehicles, setUserVehicles] = useState([]);


    useEffect(() => {
        (async() => {
           const vehicles = await getUserVehicles();
           setUserVehicles(userVehicles => vehicles);
           console.log(vehicles);
        })()
    },[])


    return (
        <section className="vehicles">
            <div className="vehicle-container">
                <div className="create-button"> <NavLink to="/Vehicle/Create">Add vehicle</NavLink></div>
                <div className="div2"> test2</div>
                <div className="div3"> test3</div>
                <div className="div4"> test4</div>
                <div className="user-vehicles"> {userVehicles.map(uv => <VehicleCard key ={uv.id} vehicleData = {uv}/>)}</div>
            </div>
        </section>
    );
}

export default Vehicles;