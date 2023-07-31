import { useEffect, useState } from 'react';

import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

import LatestTripsCard from './LatestTripsCard';

import IsLoadingHOC from '../../../Common/IsLoadingHoc';

import './LatestTrips.css'

const LatestTrips = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [recentUserTrips, setRecentUserTrips] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecentTrips = async () => {
            try {
                const response = await axiosPrivate.get(`/Trips/Last/${6}`, {
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
            isMounted && controller.abort()
        }
    }, [])


    return (
        <div className="home-latest-trips-container">
            <div className="home-latest-trips-header">Latest trips</div>
            <div className="home-latest-trips-list">
                {
                    recentUserTrips.length > 0
                        ?
                        recentUserTrips.map(rt => <LatestTripsCard key = {rt.id} details = {rt}/> )
                        :
                        <div>You haven't added any trip records yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(LatestTrips);