import { useEffect, useState } from "react";

import { getUserTripsCost, getUserTripsCount } from "../../services/tripService";

import IsLoadingHOC from "../Common/IsLoadingHoc";

import './TripsStatistics.css'

const TripsStatistics = (props) => {

    const { setLoading } = props;

    const [userTripsCount, setUserTripsCount] = useState(null);
    const [userTripsCost, setUserTripsCost] = useState(null);

    useEffect(() => {
        (async () => {
            try {
                let userTripsCountResult = await getUserTripsCount();
                let userTripsCostResult = await getUserTripsCost();

                setUserTripsCount(userTripsCost => userTripsCountResult);
                setUserTripsCost(userTripsCost => userTripsCostResult);

                console.log(userTripsCountResult);
                console.log(userTripsCostResult);

                setLoading(false);
            } catch (error) {
                NotificationHandler(error);
                setLoading(false);
            }
        })()
    }, [])


    return (

        <div className="trips-statistics-list">
            <h1>Overview:</h1>
            <div className="trips-count">You have completed {userTripsCount} so far.</div>
            <div className="trips-cost">The total cost of your trips is: {userTripsCost} lv.</div>
            <div className="trip-statistics-border"></div>
        </div>   
        
    )
}


export default IsLoadingHOC(TripsStatistics);