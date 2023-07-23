import { useEffect, useState } from 'react';

import { getLatestTrips } from '../../../services/tripService';

import RecentTripCard from './RecentTripCard';

import IsLoadingHOC from '../../Common/IsLoadingHoc';

import './RecentTrips.css'
const RecentTrips = (props) => {

    const { setLoading } = props;

    const [recentUserTrips, setRecentUserTrips] = useState([]);

    useEffect(() => {
        (async () => {
            try {
                const trips = await getLatestTrips(3);
                setRecentUserTrips(recentUserTrips => trips);
                setLoading(false);
            }
            catch (error) {
                NotificationHandler(error)
                setLoading(false);
            }
        })()
    }, [])

    return (
        <div className="recent-trips-container">
            <div className="trips-header">Recent trips</div>
            <div className="trips-list">
                {
                    recentUserTrips.length > 0
                        ?
                        recentUserTrips.map(rt => <RecentTripCard key = {rt.id} details = {rt}/> )
                        :
                        <div>You haven't added any trip records yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(RecentTrips);