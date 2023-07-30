import { useEffect, useState } from 'react';

import { Link } from 'react-router-dom';

import { NotificationHandler } from '../../utils/NotificationHandler';

import useAxiosPrivate from '../../hooks/useAxiosPrivate';
import IsLoadingHOC from '../Common/IsLoadingHoc';

import ServiceRecordCard from './ServiceRecordCard';
import ServiceRecordsStatistics from './ServiceRecordStatistics/ServiceRecordsStatistics';

import './ServiceRecords.css'



const ServiceRecords = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [serviceRecords, setServiceRecords] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getServiceRecords = async () => {
            try {
                const response = await axiosPrivate.get('/ServiceRecords', {
                    signal: controller.signal
                });
                isMounted && setServiceRecords(serviceRecords => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getServiceRecords();

        return () => {
            isMounted = false;
            controller.abort();
        }
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