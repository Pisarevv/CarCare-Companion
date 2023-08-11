// React Router hooks for obtaining router state and navigation.
import { useLocation, useNavigate, useParams } from 'react-router-dom';

// React's hooks for managing side-effects and state.
import { useEffect, useState } from 'react';

// Custom hook to make authenticated Axios requests.
import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../../../Common/IsLoadingHoc';

// Utility for showing notifications on errors or other events.
import { NotificationHandler } from '../../../../utils/NotificationHandler';

// CSS styles specific to this component.
import './ApplicationUserDetails.css';

const ApplicationUserDetails = (props) => {

    // Extract the setLoading function from the props, which controls the loading state.
    const { setLoading } = props;

    // Obtain the 'id' parameter from the route.
    const { id } = useParams();

    // Hook to get access to the current location (URL) state.
    const location = useLocation();

    // Hook to programmatically navigate to other routes.
    const navigate = useNavigate();

    // Instantiate the useAxiosPrivate hook to get an instance of Axios with authentication headers.
    const axiosPrivate = useAxiosPrivate();

    // State to hold the details of the application user.
    const [userDetails, setUserDetails] = useState([]);

    // UseEffect hook to fetch the user details when the component mounts.
    useEffect(() => {

        // Flag to ensure asynchronous tasks don't update state after the component is unmounted.
        let isMounted = true;

        // Create an AbortController to cancel the fetch request in case of unmounting.
        const controller = new AbortController();

        const getUserDetails = async () => {
            try {

                // Fetch the details of the application user.
                const response = await axiosPrivate.get(`/Users/ApplicationUsers/${id}`, {
                    signal: controller.signal
                });

                // If the component is still mounted, update the state.
                isMounted && setUserDetails(userDetails => response.data);
            } catch (error) {

                // Handle any error that arises during the fetch operation.
                NotificationHandler(error);

                // Redirect to the list of application users.
                navigate(to = "/Users/ApplicationUsers", { state: { from: location }, replace: true });
            }
            finally {

                // Stop showing the loading state.
                setLoading(false);
            }
        }

        // Call the function to fetch the user details.
        getUserDetails();

        // Cleanup function: run this if the component unmounts.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, []);


    const onUserRoleChange = async (e) => {
        e.preventDefault();
        if (!userDetails.isAdmin) {
           try {
            const result = await axiosPrivate.patch(`Users/ApplicationUsers/AddAdmin/${id}`);
            navigate("/Administrator/ApplicationUsers");
           } catch (error) {
            NotificationHandler(error);
           }
        }
        else if (userDetails.isAdmin) {
            try {
            const result = await axiosPrivate.patch(`Users/ApplicationUsers/RemoveAdmin/${id}`);
            navigate("/Administrator/ApplicationUsers");
            } catch (error) {
              NotificationHandler(error);
            }
        }
    }

    return (
        <div className="user-details-container">
            <div className="user-details-card">
                <div className="user-details-row">
                    <label>Id</label>
                    <div>{userDetails.userId}</div>
                </div>
                <div className="user-details-row">
                    <label>Username</label>
                    <div>{userDetails.username}</div>
                </div>
                <div className="user-details-row">
                    <label>First name</label>
                    <div>{userDetails.firstName}</div>
                </div>
                <div className="user-details-row">
                    <label>Last name</label>
                    <div>{userDetails.lastName}</div>
                </div>
                <div className="user-details-row">
                    <label>Vehicles count</label>
                    <div>{userDetails.vehiclesCount}</div>
                </div>
                <div className="user-details-row">
                    <label>Trips count</label>
                    <div>{userDetails.tripsCount}</div>
                </div>
                <div className="user-details-row">
                    <label>Service records count:</label>
                    <div>{userDetails.serviceRecordsCount}</div>
                </div>
                <div className="user-details-row">
                    <label>Tax records count:</label>
                    <div>{userDetails.taxRecordsCount}</div>
                </div>
                <div className="user-details-row">
                    <label>Role</label>
                    <div>{userDetails.isAdmin ? "Admin" : "User"}</div>
                </div>
                <div className="user-details-row">
                    <button onClick={onUserRoleChange} className="admin-button">{userDetails.isAdmin ? "Remove Admin" : "Make Admin"}</button>
                </div>

            </div>
        </div>
    )
}

export default IsLoadingHOC(ApplicationUserDetails)