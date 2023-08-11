// React's hooks for managing state and side-effects.
import { useEffect, useState } from 'react';

// React-router's Link component for creating navigation links.
import { Link } from 'react-router-dom';

// Helper utility for handling notifications.
import { NotificationHandler } from '../../utils/NotificationHandler';

// Custom hook for making authenticated Axios requests.
import useAxiosPrivate from '../../hooks/useAxiosPrivate';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../Common/IsLoadingHoc';

// Child components responsible for displaying individual service record and related statistics.
import ServiceRecordCard from './ServiceRecordCard';
import ServiceRecordsStatistics from './ServiceRecordStatistics/ServiceRecordsStatistics';

// CSS styles specific to this component.
import './ServiceRecords.css';

/**
 * ServiceRecords component displays a list of service records and associated statistics.
 * Users can also add new service records from this view.
 */
const ServiceRecords = (props) => {

    // Destructure the setLoading function from props (likely passed from IsLoadingHOC).
    const { setLoading } = props;

    // Custom hook to get an Axios instance for authenticated requests.
    const axiosPrivate = useAxiosPrivate();

    // State to hold the service records fetched from the backend.
    const [serviceRecords, setServiceRecords] = useState([]);

    useEffect(() => {
        // Flag to track if the component is still mounted to prevent state updates on an unmounted component.
        let isMounted = true;
        const controller = new AbortController();

        const getServiceRecords = async () => {
            try {
                const response = await axiosPrivate.get('/ServiceRecords', {
                    signal: controller.signal  // Used for cancelling the request if needed.
                });
                // Only update state if the component is still mounted.
                isMounted && setServiceRecords(response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });  // Redirect to login on error.
            } finally {
                // Always set loading to false at the end.
                setLoading(false);
            }
        }

        // Fetch the service records.
        getServiceRecords();

        return () => {
            // Clean up function. If the component is unmounted, abort any ongoing request.
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    return (
        <section className='service-records-section'>
            <div className='service-records-container'>
                <div className="services-statistics">
                    {/* Link to add a new service record. */}
                    <Link id="add-record" to="/ServiceRecords/Add">Add service record</Link>
                    {/* Component to display service record statistics. */}
                    <div className='service-records-statistics'><ServiceRecordsStatistics /></div>
                </div>
                <div className="service-records-list">
                    {
                        // Display service records if any, otherwise show a message.
                        serviceRecords.length > 0
                            ? serviceRecords.map(sr => <ServiceRecordCard key={sr.id} serviceRecordDetails={sr} />)
                            : <div>You haven't added any service records yet.</div>
                    }
                </div>
            </div>
        </section>
    )
}

// Export the component wrapped in the IsLoadingHOC, which will show a loading state until setLoading is set to false.
export default IsLoadingHOC(ServiceRecords);
