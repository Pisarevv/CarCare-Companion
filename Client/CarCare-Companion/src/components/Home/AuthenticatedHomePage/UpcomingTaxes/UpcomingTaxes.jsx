import { useEffect, useState } from 'react';

import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

import IsLoadingHOC from '../../../Common/IsLoadingHoc'
import UpcomingTaxCard from './UpcomingTaxCard';


import './UpcomingTaxes.css'

const UpcomingTaxes = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [upcomingTaxes, setUpcomingTaxes] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getUpcomingTaxes = async () => {
            try {
                const response = await axiosPrivate.get(`/TaxRecords/Upcoming/${3}`, {
                    signal: controller.signal
                });
                isMounted && setUpcomingTaxes(recentUserTrips => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getUpcomingTaxes();

        return () => {
            isMounted = false;
            isMounted && controller.abort()
        }
    }, [])


    return (
        <div className="upcoming-taxes-container">
            <div className="upcoming-taxes-header">Upcoming taxes</div>
            <div className="upcoming-taxes-list">
                {
                    upcomingTaxes.length > 0
                        ?
                        upcomingTaxes.map(ut => <UpcomingTaxCard key = {ut.id} details = {ut}/> )
                        :
                        <div>You haven't added any trip records yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(UpcomingTaxes);