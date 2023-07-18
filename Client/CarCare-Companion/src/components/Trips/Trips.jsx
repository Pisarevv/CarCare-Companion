import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';

import { getAllUserTrips} from '../../services/tripService';

import UserTripCard from './UserTripCard';

import { NotificationHandler } from '../../utils/NotificationHandler'

import IsLoadingHOC from '../Common/IsLoadingHoc';

import './Trips.css'
import TripsStatistics from './TripsStatistics/TripsStatistics';

const Trips = (props) => {

    const [userTrips, setUserTrips] = useState([]);
   
    const { setLoading } = props;

    useEffect(() => {
        (async () => {
            try {
                let userTripsResult = await getAllUserTrips();               
                setUserTrips(userTrips => userTripsResult);            
                setLoading(false);
            } catch (error) {
              NotificationHandler(error);
              setLoading(false);
            }
        })()
    }, [])

    return (
        <section className="trips-section">
            <div className="trips-container">
                <div className="add-trip-button"><NavLink to="/Trips/Add">Add trip</NavLink> </div>
                <div className="trips-statistics"><TripsStatistics/></div>
                <div className="trips-list">{userTrips.map(ut => <UserTripCard key={ut.id} tripDetails={ut} />)}</div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(Trips);