import { useEffect, useReducer, useState } from 'react'

import { NavLink, useNavigate, useParams } from 'react-router-dom'

import tripReducer from '../../../reducers/tripReducer'

import useAxiosPrivate from '../../../hooks/useAxiosPrivate'
import IsLoadingHOC from '../../Common/IsLoadingHoc'

import { NotificationHandler } from '../../../utils/NotificationHandler'

import DecimalSeparatorFormatter from '../../../utils/DecimalSeparatorFormatter'

import './EditTrip.css'

const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    fuelFields: "Both fields related to fuel have to be listed."
}

const ValidationRegexes = {
    //Validates that the fuel price and travelled distance is a floating point  
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),
}

const EditTrip = (props) => {

    const navigate = useNavigate();

    const axiosPrivate = useAxiosPrivate()

    const { setLoading } = props;

    const { id } = useParams();

    const [userVehicles, setUserVehicles] = useState([]);

    const [stepOneFinished, setStepOneFinished] = useState(false);

    const [trip, dispatch] = useReducer(tripReducer, {
        startDestination: "",
        endDestination: "",
        mileageTravelled: "",
        fuelPrice: "",
        usedFuel: "",
        vehicleId: "",

        startDestinationError: "",
        endDestinationError: "",
        mileageTravelledError: "",
        fuelPriceError: "",
        usedFuelError: "",
        vehicleIdError: ""

    });

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getTripDetails = async () => {
            try {
                const requests = [
                    axiosPrivate.get('/Vehicles', {
                        signal: controller.signal
                    }),
                    axiosPrivate.get(`/Trips/Details/${id}`, {
                        signal: controller.signal
                    })
                ]

                Promise.all(requests)
                .then(responses => {
                    const userVehiclesResult = responses[0].data;
                    const userTripDetails = responses[1].data;

                    setTripInitialDetails(userTripDetails);
                    dispatch({ type: `SET_VEHICLEID`, payload: userVehiclesResult[0].id })
                    isMounted && setUserVehicles(userVehicles => userVehiclesResult);
                })             
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getTripDetails();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])


    const setTripInitialDetails = (userTripDetails) => {
        for (const property in userTripDetails) {
          if(userTripDetails[property] == null){
            userTripDetails[property] = "";
          }
          dispatch({ type: `SET_${(property).toUpperCase()}`, payload: userTripDetails[property] })
        }
      }

    const onInputChange = (e) => {
        dispatch({ type: `SET_${(e.target.name).toUpperCase()}`, payload: e.target.value })
    }

    const validateTextFields = (target, value) => {
        if (value.trim() === "") {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
            return false;
        }
        return true;
    }

    const validateNumberFields = (target, value) => {

        if (value === "") {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
            return false;
        }

        if (!ValidationRegexes.floatNumbersRegex.test(value)) {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
            return false;
        }
        return true;
    }

    const onStepOneFinished = (e) => {
        e.preventDefault()
        let isStartDestinationValid = validateTextFields("startDestination", trip.startDestination);
        let isEndDestinationValid = validateTextFields("endDestination", trip.endDestination);
        let isMileageValid = validateNumberFields("mileageTravelled", trip.mileageTravelled);
        let isvehicleIdValid = validateTextFields("vehicleId", trip.vehicleId);

        if (isStartDestinationValid && isEndDestinationValid &&
            isMileageValid && isvehicleIdValid) {
            setStepOneFinished(stepOneFinished => true)
        }
        else {
            setStepOneFinished(stepOneFinished => false)
        }

    }

    const previousViewHandler = () => {
        setStepOneFinished(stepOneFinished => false);
    }


    const onTripAdd = async (e) => {
        e.preventDefault();
        try {
            let isStartDestinationValid = validateTextFields("startDestination", trip.startDestination);
            let isEndDestinationValid = validateTextFields("endDestination", trip.endDestination);
            let isMileageValid = validateNumberFields("mileageTravelled", trip.mileageTravelled);
            let isvehicleIdValid = validateTextFields("vehicleId", trip.vehicleId);
            if (!trip.usedFuel && !trip.fuelPrice) {
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isvehicleIdValid) {
                    const { startDestination, endDestination, vehicleId } = trip;
                    const mileageTravelled = DecimalSeparatorFormatter(trip.mileageTravelled);
                    const usedFuel = null;
                    const fuelPrice = null;
                    const response = await axiosPrivate.patch(`/Trips/Edit/${id}`,{startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId});
                    navigate("/Trips");
                    NotificationHandler("Success", "Sucessfully added trip record!", response.status);
                }
            }
            else {
                let isFuelPriceValid = validateNumberFields("fuelPrice", trip.fuelPrice);
                let isUsedFuelValid = validateNumberFields("usedFuel", trip.usedFuel);
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isFuelPriceValid &&
                    isUsedFuelValid && isvehicleIdValid) {
                    const { startDestination, endDestination, vehicleId } = trip;
                    const mileageTravelled = DecimalSeparatorFormatter(trip.mileageTravelled);
                    const usedFuel = DecimalSeparatorFormatter(trip.usedFuel);
                    const fuelPrice = DecimalSeparatorFormatter(trip.fuelPrice);
                    const response = await axiosPrivate.patch(`/Trips/Edit/${id}`,{startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId});
                    navigate("/Trips");
                    NotificationHandler("Success", "Sucessfully edited trip record!", response.status);
                }
            }

        }
        catch (error) {
            window.scrollTo({ top: 0, behavior: 'smooth' });
            const { title, status } = error.response.data;
            NotificationHandler("Warning", title, status);
        }
    }

    return (
        <section className="add-trip-section">
            <div className="add-trip-container">
                <div className="add-trip-body">

                    {
                        stepOneFinished == false
                            ?
                            <>
                                <div className="add-trip-heading">
                                    <h2>Edit trip</h2>
                                </div>
                                <form onSubmit={onStepOneFinished}>
                                    <div className="input-group input-group-lg">
                                        <label>Start destination:</label>
                                        <input className="form-control" type="text" placeholder="Start destination" name="startDestination" value={trip.startDestination} onChange={onInputChange} />
                                        {trip.startDestinationError && <p className="invalid-field" >{trip.startDestinationError}</p>}
                                    </div>
                                    <div className="input-group input-group-lg">
                                        <label>End destination:</label>
                                        <input className="form-control" type="text" placeholder="End destination" name="endDestination" value={trip.endDestination} onChange={onInputChange} />
                                        {trip.endDestinationError && <p className="invalid-field">{trip.endDestinationError}</p>}
                                    </div>
                                    <div className="input-group input-group-lg">
                                        <label>Travelled mileage:</label>
                                        <input className="form-control" type="text" placeholder="Travelled mileage" name="mileageTravelled" value={trip.mileageTravelled} onChange={onInputChange} />
                                        {trip.mileageTravelledError && <p className="invalid-field" >{trip.mileageTravelledError}</p>}
                                    </div>
                                    <div className="input-group input-group-lg">
                                        <label>Vehicle:</label>
                                        <div className="form-control select">
                                            <select className="select-group" name="vehicleId" value={trip.vehicleId} onChange={onInputChange}>
                                                {userVehicles.map(uv => <option key={uv.id} value={uv.id}>{`${uv.make} ${uv.model}`}</option>)}
                                            </select>
                                        </div>
                                        {trip.vehicleId && <p className="invalid-field" >{trip.vehicleIdError}</p>}
                                    </div>
                                    <NavLink className="float" to={`/Trips`}>Cancel</NavLink>
                                    <button type="submit" className="float">Next step</button>
                                </form>
                            </>
                            :
                            <>
                                <div className="add-trip-heading">
                                    <h2>Fuel details</h2>
                                </div>
                                <form onSubmit={onTripAdd}>
                                    <div className="input-group input-group-lg">
                                        <label>Used fuel:</label>
                                        <input className="form-control" type="text" placeholder="Used fuel" name="usedFuel" value={trip.usedFuel} onChange={onInputChange} />
                                        {trip.usedFuelError && <p className="invalid-field" >{trip.usedFuelError}</p>}
                                        {(!trip.usedFuel && trip.fuelPrice) && <p className="warning-field" >{ValidationErrors.fuelFields}</p>}
                                    </div>
                                    <div className="input-group input-group-lg">
                                        <label>Fuel price per liter:</label>
                                        <input className="form-control" type="text" placeholder="Fuel price" name="fuelPrice" value={trip.fuelPrice} onChange={onInputChange} />
                                        {trip.fuelPrice && <p className="invalid-field">{trip.fuelPriceError}</p>}
                                        {(trip.usedFuel && !trip.fuelPrice) && <p className="warning-field" >{ValidationErrors.fuelFields}</p>}
                                    </div>

                                    <button className="float" onClick={previousViewHandler}>Previous</button>
                                    <button type="submit" className="float">{trip.usedFuel && trip.fuelPrice ? "Finish" : "Skip and finish"}</button>
                                </form>
                            </>
                    }

                </div>
            </div>
        </section>
    )
}

export default IsLoadingHOC(EditTrip);