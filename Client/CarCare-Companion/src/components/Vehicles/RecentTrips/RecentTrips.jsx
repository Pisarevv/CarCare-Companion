import { useEffect, useState } from 'react';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import RecentTripCard from './RecentTripCard';

import IsLoadingHOC from '../../Common/IsLoadingHoc';

import './RecentTrips.css'

const RecentTrips = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [recentUserTrips, setRecentUserTrips] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecentTrips = async () => {
            try {
                const response = await axiosPrivate.get(`/ServiceRecords/Last/${3}`, {
                    signal: controller.signal
                });
                isMounted && setRecentUserTrips(recentUserTrips => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getRecentTrips();

        return () => {
            isMounted = false;
            controller.abort();
        }
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