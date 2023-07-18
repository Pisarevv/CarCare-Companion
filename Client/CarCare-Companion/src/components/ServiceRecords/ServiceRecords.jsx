import './ServiceRecords.css'

import IsLoadingHOC from '../Common/IsLoadingHoc';
import { useEffect, useState } from 'react';
import ServiceRecordCard from './ServiceRecordCard';
import { NotificationHandler } from '../../utils/NotificationHandler';

const ServiceRecords = (props) => {
 
    const { setLoading } = props;

    const [serviceRecords, setServiceRecords] = useState([]);

    useEffect(() => {
        (async() => {
            try {
                setLoading(false);
            } 
            catch (error) {
              NotificationHandler(error);
              setLoading(false);   
            }
        })()
    }, [])

    return (
        <section className='service-records-section'>
            <div className='service-records-container'>
                <div className="services-statistics"> </div>
                <div className="service-records-list">
                    { 
                    serviceRecords.length > 0 
                    ? 
                    serviceRecords.map(sr => <ServiceRecordCard/>)
                    :
                    <div>You haven't added any service records yet.</div>
                    }
                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(ServiceRecords);