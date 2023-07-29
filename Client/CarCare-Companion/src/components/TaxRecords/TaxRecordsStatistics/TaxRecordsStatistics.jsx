import { useEffect, useState } from "react";

import { useNavigate } from "react-router-dom";

import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import IsLoadingHOC from "../../Common/IsLoadingHoc";

import { NotificationHandler } from "../../../utils/NotificationHandler";

import './TaxRecordsStatistics.css'



const TaxRecordsStatistics = (props) => {

    const { setLoading } = props;

    const navigate = useNavigate();

    const axiosPrivate = useAxiosPrivate();

    const [taxRecordsCount, setTaxRecordsCount] = useState(null);
    const [taxRecordsCost, setTaxRecordsCost] = useState(null);


    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecordsStatistics = async () => {
            try {
                const requests = [
                    axiosPrivate.get('/TaxRecords/Count', {
                        signal: controller.signal
                    }),
                    axiosPrivate.get('/TaxRecords/Cost', {
                        signal: controller.signal
                    })
                ];
                Promise.all(requests)
                .then(responses => {
                    const taxRecordsCountResult = responses[0].data;
                    const taxRecordsCostResult = responses[1].data;

                    if(isMounted){
                        setTaxRecordsCount(taxRecordsCount => taxRecordsCountResult);
                        setTaxRecordsCost(taxRecordsCost => taxRecordsCostResult);
                    }
                })
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getRecordsStatistics();

        return () => {
            isMounted = false;
            controller.abort();
        }
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