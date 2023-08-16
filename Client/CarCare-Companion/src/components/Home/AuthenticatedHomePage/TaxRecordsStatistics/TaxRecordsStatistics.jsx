// React hooks for side-effects and state management
import { useEffect, useState } from "react";

//React-router hook for managing location
import { useLocation } from 'react-router-dom';


// Custom Axios hook for making private (authenticated) requests
import useAxiosPrivate from "../../../../hooks/useAxiosPrivate";

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../../hooks/useDeauthenticate';

// Higher-Order Component to handle loading state
import IsLoadingHOC from "../../../Common/IsLoadingHoc";

// Utility to handle and show notifications
import { NotificationHandler } from "../../../../utils/NotificationHandler";

// Component specific styles
import './TaxRecordsStatistics.css'

/**
 * TaxRecordsStatistics component is responsible for displaying statistics 
 * related to tax records, including the total count and total cost.
 *
 * @param {Object} props - Props passed to the component.
 */
const TaxRecordsStatistics = (props) => {

    // Destructure setLoading from props, used to set the loading state in the HOC
    const { setLoading } = props;

    // React-router hook for location management.
    const location = useLocation();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Axios instance for authenticated requests
    const axiosPrivate = useAxiosPrivate();

    // State variables for the tax record count and total cost
    const [taxRecordsCount, setTaxRecordsCount] = useState(null);
    const [taxRecordsCost, setTaxRecordsCost] = useState(null);

    // UseEffect hook to fetch tax records statistics when the component mounts
    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecordsStatistics = async () => {
            try {
                // Simultaneous Axios requests for count and total cost of tax records
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

                    if(isMounted) {
                        setTaxRecordsCount(taxRecordsCountResult);
                        setTaxRecordsCost(taxRecordsCostResult);
                    }
                })
            } catch (err) {
                if(err.response.status == 401){
                    // On error, show a notification and redirect to the login page.
                   NotificationHandler("Something went wrong","Plese log in again", 400);
                   logUserOut(location);
               }   
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status); 
            }
            finally {
                setLoading(false);
            }
        }

        getRecordsStatistics();

        // Clean-up function to abort requests and avoid updating state after unmounting
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])


    return (

        <div className="home-page-tax-record-statistics-list">
            <h4>Tax records:</h4>
            <div className="home-page-tax-record-statistics-border"></div>
            <div className="home-page-taxRecords-count">You have added {taxRecordsCount} 
            {taxRecordsCount == 1 ? " record" : " records" } so far.</div>
            <div className="home-page-taxRecors-cost">The total cost of your tax records is: {taxRecordsCost} lv.</div>
          
        </div>   
        
    )
}


export default IsLoadingHOC(TaxRecordsStatistics);