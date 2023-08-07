import { useEffect, useState } from 'react';

import { useParams } from 'react-router-dom';

import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

import RecentVehicleServiceCard from './RecentVehicleServiceCard';

import { NotificationHandler } from '../../../../utils/NotificationHandler';

import IsLoadingHOC from '../../../Common/IsLoadingHoc';

import './RecentVehicleServices.css'



const RecentVehicleServices = (props) => {
 
    const { setLoading } = props;

    const { id } = useParams();

    const axiosPrivate = useAxiosPrivate();
    const [recentServiceRecords, setRecentServiceRecords] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecentVehicleServices = async () => {
            try {
                const response = await axiosPrivate.get(`/ServiceRecords/${id}/Last/${3}`, {
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

        getRecentVehicleServices();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    
    return (
        <div className="vehicle-recent-services-container">
            <div className="vehicle-services-header">Recently added service records</div>
            <div className="vehicle-services-list">
                {
                    recentServiceRecords.length > 0
                        ?
                        recentServiceRecords.map(rs => <RecentVehicleServiceCard key = {rs.id} details = {rs}/> )
                        :
                        <div>You haven't added any service records for this vehicle yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(RecentVehicleServices);