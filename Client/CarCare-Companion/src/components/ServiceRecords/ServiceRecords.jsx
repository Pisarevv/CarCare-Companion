import { useEffect, useState } from 'react';

import { Link } from 'react-router-dom';

import { NotificationHandler } from '../../utils/NotificationHandler';

import { getAllServiceRecords } from '../../services/serviceRecordsService';

import IsLoadingHOC from '../Common/IsLoadingHoc';

import ServiceRecordCard from './ServiceRecordCard';
import ServiceRecordsStatistics from './ServiceRecordStatistics/ServiceRecordsStatistics';

import './ServiceRecords.css'


const ServiceRecords = (props) => {

    const { setLoading } = props;

    const [serviceRecords, setServiceRecords] = useState([]);

    useEffect(() => {
        (async () => {
            try {
                let serviceRecordsResult = await getAllServiceRecords();
                console.log(serviceRecordsResult);
                setServiceRecords(serviceRecords => serviceRecordsResult);
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
                <div className="services-statistics">
                    <Link id = "add-record" to="/ServiceRecords/Add">Add service record</Link>
                    <div className='service-records-statistics'><ServiceRecordsStatistics/></div>
                </div>
                <div className="service-records-list">
                    {
                        serviceRecords.length > 0
                            ?
                            serviceRecords.map(sr => <ServiceRecordCard key={sr.id} serviceRecordDetails={sr} />)
                            :
                            <div>You haven't added any service records yet.</div>
                    }
                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(ServiceRecords);