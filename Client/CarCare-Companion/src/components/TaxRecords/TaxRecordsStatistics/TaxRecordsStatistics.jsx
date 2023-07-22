import { useEffect, useState } from "react";

import { getAllTaxRecordsCost, getAllTaxRecordsCount } from "../../../services/taxRecordsService";

import IsLoadingHOC from "../../Common/IsLoadingHoc";

import { NotificationHandler } from "../../../utils/NotificationHandler";

import './TaxRecordsStatistics.css'


const TaxRecordsStatistics = (props) => {

    const { setLoading } = props;

    const [taxRecordsCount, setTaxRecordsCount] = useState(null);
    const [taxRecordsCost, setTaxRecordsCost] = useState(null);

    useEffect(() => {
        (async () => {
            try {
                let taxRecordsCountResult = await getAllTaxRecordsCount();
                let taxRecordsCostResult = await getAllTaxRecordsCost();

                setTaxRecordsCount(userTripsCost => taxRecordsCountResult);
                setTaxRecordsCost(userTripsCost => taxRecordsCostResult);  

                setLoading(false);
            } catch (error) {
                NotificationHandler(error);
                setLoading(false);
            }
        })()
    }, [])


    return (

        <div className="tax-record-statistics-list">
            <h1>Overview:</h1>
            <div className="tax-record-statistics-border"></div>
            <div className="taxRecords-count">You have added {taxRecordsCount} 
            {taxRecordsCount == 1 ? " record" : " records" } so far.</div>
            <div className="taxRecors-cost">The total cost of your tax records is: {taxRecordsCost} lv.</div>
          
        </div>   
        
    )
}


export default IsLoadingHOC(TaxRecordsStatistics);