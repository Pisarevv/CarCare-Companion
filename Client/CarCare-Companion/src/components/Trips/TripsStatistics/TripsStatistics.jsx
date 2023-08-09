import { useEffect, useState } from "react";

import IsLoadingHOC from "../../Common/IsLoadingHoc";

import { NotificationHandler } from "../../../utils/NotificationHandler";

import './TripsStatistics.css'
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";

const TripsStatistics = (props) => {

    const axiosPrivate = useAxiosPrivate();

    const { setLoading } = props;

    const [userTripsCount, setUserTripsCount] = useState(null);
    const [userTripsCost, setUserTripsCost] = useState(null);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();
    
        const getTripStatistics = async () => {
          try {
            const requests = [
              axiosPrivate.get('/Trips/Count', {
                signal: controller.signal
              }),
              axiosPrivate.get('/Trips/Cost', {
                signal: controller.signal
              })
            ]
            Promise.all(requests)
            .then(responses => {
    
              const tripsCount = responses[0].data;
              const tripsCost = responses[1].data;

              if(isMounted){
                setUserTripsCount(userTripsCount => tripsCount);
                setUserTripsCost(userTripsCost => tripsCost);
              }        
            })
          } catch (err) {
            NotificationHandler(err);
            navigate('/login', { state: { from: location }, replace: true });
          }
          finally {
            setLoading(false);
          }
        }
    
        getTripStatistics();
    
        return () => {
          isMounted = false;
          isMounted && controller.abort();
        }
      }, [])
    

    return (

        <div className="trips-statistics-list">
            <h1>Overview:</h1>
            <div className="trip-statistics-border"></div>
            <div className="trips-count">You have completed {userTripsCount} so far.</div>
            <div className="trips-cost">The total cost of your trips is: {userTripsCost} lv.</div>
        </div>   
        
    )
}


export default IsLoadingHOC(TripsStatistics);