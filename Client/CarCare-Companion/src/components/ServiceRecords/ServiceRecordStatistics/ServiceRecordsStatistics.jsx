import { useEffect, useState } from "react";

import { getAllServiceRecordsCost, getAllServiceRecordsCount } from "../../../services/serviceRecordsService";

import IsLoadingHOC from "../../Common/IsLoadingHoc";

import { NotificationHandler } from "../../../utils/NotificationHandler";

import './ServiceRecordsStatistics.css'



const ServiceRecordsStatistics = (props) => {

    const { setLoading } = props;

    const [serviceRecordsCount, setServiceRecordsCount] = useState(null);
    const [serviceRecordsCost, setServiceRecordsCost] = useState(null);

    useEffect(() => {
        (async () => {
            try {
                let serviceRecordsCountResult = await getAllServiceRecordsCount();
                let serviceRecordsCostResult = await getAllServiceRecordsCost();

                setServiceRecordsCount(serviceRecordsCount => serviceRecordsCountResult);
                setServiceRecordsCost(serviceRecordsCost => serviceRecordsCostResult);

                setLoading(false);
            } catch (error) {
                NotificationHandler(error);
                setLoading(false);
            }
        })()
    }, [])


    return (

        <div className="service-record-statistics-list">
            <h1>Overview:</h1>
            <div className="service-record-statistics-border"></div>
            <div className="service-records-count">You have added {serviceRecordsCount} 
            {serviceRecordsCount == 1 ? " record" : " records" } so far.</div>
            <div className="service-records-cost">The total cost of your service records is: {serviceRecordsCost} lv.</div>
          
        </div>   
        
    )
}


export default IsLoadingHOC(ServiceRecordsStatistics);