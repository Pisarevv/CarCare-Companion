// React's hooks for managing side-effects and state.
import { useEffect, useState } from 'react';

// Custom hook to make authenticated Axios requests.
import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../../../Common/IsLoadingHoc'

// Child component to display individual upcoming tax records.
import UpcomingTaxCard from './UpcomingTaxCard';

// Helper utility for handling notifications.
import { NotificationHandler } from '../../../../utils/NotificationHandler';

// CSS styles specific to this component.
import './UpcomingTaxes.css'


const UpcomingTaxes = (props) => {

    // Extract the setLoading function from the props, which controls the loading state.
    const { setLoading } = props;

    // Instantiate the useAxiosPrivate hook to get an instance of Axios with authentication headers.
    const axiosPrivate = useAxiosPrivate();

    // State to hold the list of upcoming taxes.
    const [upcomingTaxes, setUpcomingTaxes] = useState([]);

    // UseEffect hook to fetch the upcoming taxes when the component mounts.
    useEffect(() => {
        // Flag to ensure asynchronous tasks don't update state after the component is unmounted.
        let isMounted = true;

        // Create an AbortController to cancel the fetch request in case of unmounting.
        const controller = new AbortController();

        const getUpcomingTaxes = async () => {
            try {
                // Fetch the next three upcoming taxes.
                const response = await axiosPrivate.get(`/TaxRecords/Upcoming/${3}`, {
                    signal: controller.signal
                });

                // If the component is still mounted, update the state.
                isMounted && setUpcomingTaxes(recentUserTrips => response.data);
            } catch (err) {
                // Handle any error that arises during the fetch operation.

                NotificationHandler(err);
                // The navigate function and location object are not defined in the provided code.
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
                // Stop showing the loading state.
                setLoading(false);
            }
        }

        // Call the function to fetch the upcoming taxes.
        getUpcomingTaxes();

        // Cleanup function: run this if the component unmounts.
        return () => {
            isMounted = false;
            isMounted && controller.abort()
        }
    }, [])



    return (
        <div className="upcoming-taxes-container">
            <div className="upcoming-taxes-header">Upcoming taxes</div>
            <div className="upcoming-taxes-list">
                {
                    upcomingTaxes.length > 0
                        ?
                        upcomingTaxes.map(ut => <UpcomingTaxCard key = {ut.id} details = {ut}/> )
                        :
                        <div>You haven't added any trip records yet.</div>
                }
            </div>
        </div>
    )
}


export default IsLoadingHOC(UpcomingTaxes);