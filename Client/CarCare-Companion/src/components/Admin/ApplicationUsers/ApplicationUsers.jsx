// React's hooks for managing side-effects and state.
import { useEffect, useState } from 'react';

// React Router hooks for navigation and accessing the current location.
import { useLocation, useNavigate } from 'react-router-dom';

// Custom hook to make authenticated Axios requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../../Common/IsLoadingHoc';

// Utility to handle notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Child component to display individual application user records.
import ApplicationUserCard from './ApplicationUserCard';

// CSS styles specific to this component.
import './ApplicationUsers.css'

const ApplicationUsers = (props) => {

    // React Router hooks for programmatically navigating and accessing the current route's location.
    const navigate = useNavigate();
    const location = useLocation();

    // Extract the setLoading function from the props, which controls the loading state.
    const { setLoading } = props;

    // Instantiate the useAxiosPrivate hook to get an instance of Axios with authentication headers.
    const axiosPrivate = useAxiosPrivate();

    // State to hold the list of application users.
    const [applicationUsers, setApplicationUsers] = useState([]);

    // UseEffect hook to fetch the application users when the component mounts.
    useEffect(() => {
        // Flag to ensure asynchronous tasks don't update state after the component is unmounted.
        let isMounted = true;

        // Create an AbortController to cancel the fetch request in case of unmounting.
        const controller = new AbortController();

        const getApplcationUsers = async () => {
            try {
                // Fetch the list of application users.
                const response = await axiosPrivate.get('/Users/ApplicationUsers', {
                    signal: controller.signal
                });

                // Log the fetched data for debugging purposes. Consider removing this in production.
                console.log(response.data);

                // If the component is still mounted, update the state.
                isMounted && setApplicationUsers(users => response.data);
            } 
            catch (error) {
                // Handle any error that arises during the fetch operation.
                NotificationHandler(error);
                navigate('/Administrator', { state: { from: location }, replace: true });
            }
            finally {
                // Stop showing the loading state.
                setLoading(false);
            }
        }

        // Call the function to fetch the application users.
        getApplcationUsers();

        // Cleanup function: run this if the component unmounts.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    // Rendering the list of application users inside a section.
    return (
        <section className="application-users-section">
            <div className="application-users-container">
                {applicationUsers.map(au => 
                    <ApplicationUserCard key={au.id} details={au} />
                )}
            </div>
        </section>
    )
}

export default IsLoadingHOC(ApplicationUsers);
