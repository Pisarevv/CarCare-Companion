import { useEffect, useState } from 'react';

import { getLatestServiceRecords } from '../../../services/serviceRecordsService';

import RecentServiceCard from './RecentServiceCard';

import { NotificationHandler } from '../../../utils/NotificationHandler';

import IsLoadingHOC from '../../Common/IsLoadingHoc';

import './RecentServices.css'


const RecentServices = (props) => {

    const { setLoading } = props;

    const [recentServiceRecords, setRecentServiceRecords] = useState([]);

    useEffect(() => {
        (async () => {
            try {
                const serviceRecords = await getLatestServiceRecords(3);
                setRecentServiceRecords(recentServiceRecords => serviceRecords);
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