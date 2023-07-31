import { useEffect, useState } from "react";

import { useNavigate } from "react-router-dom";

import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";
import IsLoadingHOC from "../../../Common/IsLoadingHoc";

import { NotificationHandler } from "../../../../utils/NotificationHandler";

import './ServiceRecordsStatistics.css'


const ServiceRecordsStatistics = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const navigate = useNavigate();

    const [serviceRecordsCount, setServiceRecordsCount] = useState(null);
    const [serviceRecordsCost, setServiceRecordsCost] = useState(null);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getServiceRecordsStatistics = async () => {
            try {
                const requests = [
                    axiosPrivate.get('/ServiceRecords/Count', {
                        signal: controller.signal
                    }),
                    axiosPrivate.get('/ServiceRecords/Cost', {
                        signal: controller.signal
                    })
                ];

                Promise.all(requests)
                .then(responses => {
                    const serviceRecordsCountResult = responses[0].data;
                    const serviceRecordsCostResult = responses[1].data;

                    if(isMounted){
                        setServiceRecordsCount(serviceRecordsCount => serviceRecordsCountResult);
                        setServiceRecordsCost(serviceRecordsCost => serviceRecordsCostResult);
                    }
                });

            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getServiceRecordsStatistics();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])



    return (

        <div className="home-page-service-record-statistics-list">
            <h4>Service records:</h4>
            <div className="home-page-service-record-statistics-border"></div>
            <div className="home-page-service-records-count">You have added {serviceRecordsCount} service
            {serviceRecordsCount == 1 ? " record" : " records" } so far.</div>
            <div className="home-page-service-records-cost">The total cost of your service records is: {serviceRecordsCost} lv.</div>
          
        </div>   
        
    )
}


export default IsLoadingHOC(ServiceRecordsStatistics);