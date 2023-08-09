import { useEffect, useState } from 'react';
import { NavLink, useLocation, useNavigate } from 'react-router-dom';

import { NotificationHandler } from '../../utils/NotificationHandler'

import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import IsLoadingHOC from '../Common/IsLoadingHoc';

import TripsStatistics from './TripsStatistics/TripsStatistics';
import UserTripCard from './UserTripCard';

import './Trips.css'


const Trips = (props) => {

    const axiosPrivate = useAxiosPrivate();

    const navigate = useNavigate();

    const location = useLocation();

    const [userTrips, setUserTrips] = useState([]);

    const { setLoading } = props;

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getTrips = async () => {
            try {
                const response = await axiosPrivate.get('/Trips', {
                    signal: controller.signal
                });
                isMounted && setUserTrips(userTrips => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
                setLoading(false);
            }
        }

        getTrips();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    return (
        <section className="trips-section">
            <div className="trips-container">
                <div className="trips-statistics">
                    <div className="add-trip-button"><NavLink to="/Trips/Add">Add trip</NavLink></div>
                    <div className='trip-records-statistics'> <TripsStatistics /></div>
                </div>
                <div className="trips-list-container ">{userTrips.map(ut => <UserTripCard key={ut.id} tripDetails={ut} />)}</div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(Trips);