import { useLocation, useNavigate, useParams } from 'react-router-dom'

import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';
import IsLoadingHOC from '../../../Common/IsLoadingHoc';
import { useEffect, useState } from 'react';
import { NotificationHandler } from '../../../../utils/NotificationHandler';

import './ApplicationUserDetails.css'

const ApplicationUserDetails = (props) => {

    const { setLoading } = props;

    const { id } = useParams();

    const location = useLocation();

    const navigate = useNavigate();

    const axiosPrivate = useAxiosPrivate()

    const [userDetails, setUserDetails] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getUserDetails = async () => {
            try {
                const response = await axiosPrivate.get(`/Users/ApplicationUsers/${id}`, {
                    signal: controller.signal
                });

                isMounted && setUserDetails(userDetails => response.data);
            } catch (error) {
                NotificationHandler(error);
                navigate(to = "/Users/ApplicationUsers", { state: { from: location }, replace: true });
            }
            finally {
                setLoading(false);
            }
        }

        getUserDetails();

        return () => {
            isMounted = false
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