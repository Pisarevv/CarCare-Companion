// Required React imports.
import { useEffect, useState } from 'react';

// React-router imports for navigation and accessing route parameters.
import { Link } from 'react-router-dom';

// Custom hook for making authenticated Axios requests.
import useAxiosPrivate from '../../hooks/useAxiosPrivate';

// High-Order Component for loading state management.
import IsLoadingHOC from '../Common/IsLoadingHoc';

// Utility functions for notifications
import { NotificationHandler } from '../../utils/NotificationHandler';

//Child components
import TaxRecordCard from './TaxRecordCard';
import TaxRecordsStatistics from './TaxRecordsStatistics/TaxRecordsStatistics';

// Component-specific styles.
import './TaxRecords.css'

const TaxRecords = (props) => {

    // Destructuring the setLoading function from props
    const { setLoading } = props;

    // Using a custom hook to make private authenticated axios requests
    const axiosPrivate = useAxiosPrivate();

    // State to hold the list of tax records
    const [taxRecords, setTaxRecords] = useState([]);

    // Effect hook that runs once on component mount
    useEffect(() => {
        // A flag to check if the component is still mounted before updating its state
        let isMounted = true;
        // Initialization of AbortController for aborting fetch requests
        const controller = new AbortController();

        // Asynchronous function to fetch tax records from the server
        const getRecords = async () => {
            try {
                const response = await axiosPrivate.get('/TaxRecords', {
                    signal: controller.signal
                });
                // Setting tax records if component is still mounted
                isMounted && setTaxRecords(taxRecords => response.data);
            } catch (err) {
                // Handling any errors during the fetch and notifying the user
                NotificationHandler(err);
                // Redirecting to login page on error (ensure you have 'navigate' and 'location' in scope)
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
                // Setting the loading state to false after data fetch completes
                setLoading(false);
            }
        }

        // Call the asynchronous function
        getRecords();

        // Cleanup function to run when the component unmounts
        return () => {
            isMounted = false;
            // Abort the fetch request if component is no longer mounted
            isMounted && controller.abort();
        }
    }, []);  // Empty dependency array ensures this effect runs only once

    return (
        <section className='tax-records-section'>
            <div className='tax-records-container'>
                <div className="taxes-statistics">
                    {/* Link to add a new tax record */}
                    <Link id="add-tax-record" to="/TaxRecords/Add">Add tax record</Link>
                    {/* Component displaying tax records statistics */}
                    <div className='tax-records-statistics'><TaxRecordsStatistics/></div>
                </div>
                <div className="tax-records-list">
                    {
                        // Conditionally render tax records if available, otherwise display a default message
                        taxRecords.length > 0
                            ?
                            taxRecords.map(tr => <TaxRecordCard key={tr.id} taxRecordDetails={tr} />)
                            :
                            <div>You haven't added any tax records yet.</div>
                    }
                </div>
            </div>
        </section>
    )
}

// Wrapping the TaxRecords component with the IsLoadingHOC to display a loading spinner when needed
export default IsLoadingHOC(TaxRecords);
