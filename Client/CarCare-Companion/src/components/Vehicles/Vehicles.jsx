import { useEffect, useState } from 'react';
import { NavLink, useLocation, useNavigate } from 'react-router-dom';

import VehicleCard from './VehicleCard';
import RecentTrips from './RecentTrips/RecentTrips';
import RecentServices from './RecentServices/RecentServices';

import { NotificationHandler } from '../../utils/NotificationHandler'

import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import IsLoadingHOC from '../Common/IsLoadingHoc';

import './Vehicles.css';



const Vehicles = (props) => {

    const axiosPrivate = useAxiosPrivate();

    const { setLoading } = props;

    const [userVehicles, setUserVehicles] = useState([]);
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getUserVehicles = async () => {
            try {
                const response = await axiosPrivate.get('/Vehicles', {
                    signal: controller.signal
                });
                isMounted && setUserVehicles(response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getUserVehicles();

        return () => {
            isMounted = false;
            controller.abort();
        }
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