import { useEffect, useState } from 'react';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

import RecentServiceCard from './RecentServiceCard';

import { NotificationHandler } from '../../../utils/NotificationHandler';

import IsLoadingHOC from '../../Common/IsLoadingHoc';

import './RecentServices.css'



const RecentServices = (props) => {
 
    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();
    const [recentServiceRecords, setRecentServiceRecords] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecentServices = async () => {
            try {
                const response = await axiosPrivate.get(`/ServiceRecords/Last/${3}`, {
                    signal: controller.signal
                });
                isMounted && setRecentServiceRecords(recentServiceRecords => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getRecentServices();

        return () => {
            isMounted = false;
            controller.abort();
        }
    }, [])

    
    return (
        <div className="recent-trips-container">
            <div className="trips-header">Recently added service records</div>
            <div className="trips-list">
                {
                    recentServiceRecords.length > 0
                        ?
                        recentServiceRecords.map(rs => <RecentServiceCard key = {rs.id} details = {rs}/> )
                        :
                        <div>You haven't added any service records yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(RecentServices);