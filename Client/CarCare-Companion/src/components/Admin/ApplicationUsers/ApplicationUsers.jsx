import { useEffect, useState } from 'react'

import { useLocation, useNavigate } from 'react-router-dom';

import useAxiosPrivate from '../../../hooks/useAxiosPrivate';
import IsLoadingHOC from '../../Common/IsLoadingHoc';

import { NotificationHandler } from '../../../utils/NotificationHandler';

import ApplicationUserCard from './ApplicationUserCard';

import './ApplicationUsers.css'

const ApplicationUsers = (props) => {
 
    const navigate = useNavigate();

    const location = useLocation();

    const {setLoading} = props;

    const axiosPrivate = useAxiosPrivate();

    const [applicationUsers,setApplicationUsers] = useState([]);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getApplcationUsers = async () => {
           try {
             const response = await axiosPrivate.get('/Admin/ApplicationUsers', {
                signal : controller.signal
            });

            console.log(response.data);

            isMounted && setApplicationUsers(applicationUsers => response.data);

           } catch (error) {
            NotificationHandler(error);
            navigate('/Admin', { state: { from: location }, replace: true });
           }finally{
            setLoading(false);
           }

        }

        getApplcationUsers();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    },[])


    return(
        <section className="application-users-section">
        <div className="application-users-container">
          {applicationUsers.map(au => <ApplicationUserCard key = {au.id}  details={au}/>)}
        </div>
    </section>
    )

}

export default IsLoadingHOC(ApplicationUsers);