import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';

import { getAllUserTrips, getUserTripsCost, getUserTripsCount } from '../../services/tripService';

import UserTripCard from './UserTripCard';

import { ErrorHandler } from '../../utils/ErrorHandler/ErrorHandler';

import IsLoadingHOC from '../Common/IsLoadingHoc';

import './Trips.css'

const Trips = (props) => {

    const [userTrips, setUserTrips] = useState([]);
    const [userTripsCount, setUserTripsCount] = useState(null);
    const [userTripsCost, setUserTripsCost] = useState(null);

    const { setLoading } = props;

    useEffect(() => {
        (async () => {
            try {
                let userTripsResult = await getAllUserTrips();
                let userTripsCountResult = await getUserTripsCount();
                let userTripsCostResult = await getUserTripsCost();
                
                setUserTrips(userTrips => userTripsResult);
                setUserTripsCount(userTripsCost => userTripsCountResult);
                setUserTripsCost(userTripsCost => userTripsCostResult);

                console.log(userTripsCountResult);
                console.log(userTripsCostResult);
               
                setLoading(false);
            } catch (error) {
              ErrorHandler(error);
              setLoading(false);
            }
        })()
    }, [])

    return (
        <section className="trips-section">
            <div className="trips-container">
                <div className="add-trip-button"><NavLink to="/Trips/Add">Add trip</NavLink> </div>
                <div className="div2"> 1</div>
                <div className="trips-list">{userTrips.map(ut => <UserTripCard key={ut.id} tripDetails={ut} />)}</div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(Trips);