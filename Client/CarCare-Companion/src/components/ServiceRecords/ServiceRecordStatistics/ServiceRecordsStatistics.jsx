// Importing necessary React hooks and React Router utilities.
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

// Custom Axios hook for authenticated requests.
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";

// A Higher-Order Component to display a loading spinner or the wrapped component.
import IsLoadingHOC from "../../Common/IsLoadingHoc";

// Utility to handle notifications.
import { NotificationHandler } from "../../../utils/NotificationHandler";

// CSS styles specific to this component.
import './ServiceRecordsStatistics.css'

/**
 * ServiceRecordsStatistics is a component to display statistical data related to service records, 
 * such as the count of records and their total cost.
 * 
 * @param {Object} props - Props passed to the component.
 */
const ServiceRecordsStatistics = (props) => {

    // Destructuring the setLoading function from the props.
    const { setLoading } = props;

    // Custom hook for making authenticated Axios requests.
    const axiosPrivate = useAxiosPrivate();

    // React Router's navigate function to programmatically change routes.
    const navigate = useNavigate();

    // State variables to hold the count and cost of the service records.
    const [serviceRecordsCount, setServiceRecordsCount] = useState(null);
    const [serviceRecordsCost, setServiceRecordsCost] = useState(null);

    // Effect hook to fetch statistical data about service records.
    useEffect(() => {
        // To ensure that we don't set the state of an unmounted component.
        let isMounted = true;
        // To handle aborted requests.
        const controller = new AbortController();

        // Function to fetch the statistical data.
        const getServiceRecordsStatistics = async () => {
            try {
                // Axios requests to fetch the count and cost.
                const requests = [
                    axiosPrivate.get('/ServiceRecords/Count', {
                        signal: controller.signal
                    }),
                    axiosPrivate.get('/ServiceRecords/Cost', {
                        signal: controller.signal
                    })
                ];

                // Handling multiple promises at once.
                Promise.all(requests)
                .then(responses => {
                    const serviceRecordsCountResult = responses[0].data;
                    const serviceRecordsCostResult = responses[1].data;

                    // If the component is still mounted, set the states.
                    if(isMounted){
                        setServiceRecordsCount(serviceRecordsCount => serviceRecordsCountResult);
                        setServiceRecordsCost(serviceRecordsCost => serviceRecordsCostResult);
                    }
                });

            } catch (err) {
                // On error, show a notification and redirect to the login page.
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            } finally {
                // Stop showing the loading spinner.
                setLoading(false);
            }
        }

        // Calling the function.
        getServiceRecordsStatistics();

        // Cleanup function to abort the requests if the component is unmounted.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    // JSX for the component.
    return (
        <div className="service-record-statistics-list">
            <h1>Overview:</h1>
            <div className="service-record-statistics-border"></div>
            <div className="service-records-count">
                You have added {serviceRecordsCount} 
                {/* Conditionally rendering the word 'record' based on the count. */}
                {serviceRecordsCount == 1 ? " record" : " records" } so far.
            </div>
            <div className="service-records-cost">
                The total cost of your service records is: {serviceRecordsCost} lv.
            </div>
        </div>   
    )
}

// Wrapping the component in the Higher-Order Component.
export default IsLoadingHOC(ServiceRecordsStatistics);
