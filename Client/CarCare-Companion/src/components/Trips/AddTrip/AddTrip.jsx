import { NavLink, useNavigate } from 'react-router-dom'
import { useEffect, useReducer, useState } from 'react'

import tripReducer from '../../../reducers/tripReducer'

import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import IsLoadingHOC from '../../Common/IsLoadingHoc'

import { NotificationHandler } from '../../../utils/NotificationHandler'

import './AddTrip.css'

const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    fuelFields: "Both fields related to fuel have to be listed."
}

const ValidationRegexes = {
    //Validates that the fuel price and travelled distance is a floating point  
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),
}

const AddTrip = (props) => {

    const navigate = useNavigate();

    const axiosPrivate = useAxiosPrivate();

    const { setLoading } = props;

    const [userVehicles, setUservehicles] = useState([]);

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

        const getUservehicleIds = async () => {
            try {
                const response = await axiosPrivate.get('/Vehicles', {
                    signal: controller.signal
                });

                dispatch({ type: `SET_VEHICLEID`, payload: response.data[0].id })

                isMounted && setUservehicles(uservehicleIds => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getUservehicleIds();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

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

        if (!ValidationRegexes.floatNumbersRegex.test(value) || value.trim() === "") {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
            return false;
        }
        return true;
    }

    const onStepOneFinished = (e) => {
        e.preventDefault()
        let isStartDestinationValid = validateTextFields("startDestination", trip.startDestination);
        let isEndDestinationValid = validateTextFields("endDestination", trip.endDestination);
        let isMileageValid = validateNumberFields("mileageTravelled", trip.mileageTravelled);
        let isVehicleIdValid = validateTextFields("vehicleId", trip.vehicleId);

        if (isStartDestinationValid && isEndDestinationValid &&
            isMileageValid && isVehicleIdValid) {
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
            let isVehicleIdValid = validateTextFields("vehicleId", trip.vehicleId);
            if (!trip.usedFuel || !trip.fuelPrice) {
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isVehicleIdValid) {
                    const { startDestination, endDestination, mileageTravelled, vehicleId } = trip;
                    const usedFuel = null;
                    const fuelPrice = null;
                    await axiosPrivate.post("/Trips", { startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId})
                }
            }
            else {
                let isFuelPriceValid = validateNumberFields("fuelPrice", trip.fuelPrice);
                let isUsedFuelValid = validateNumberFields("usedFuel", trip.usedFuel);
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isFuelPriceValid &&
                    isUsedFuelValid && isVehicleIdValid) {
                    const { startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId } = trip;
                    await axiosPrivate.post("/Trips", { startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId})
                }
            }

            navigate("/Trips")

        }
        catch (error) {
            NotificationHandler(error);
            navigate('/Trips')
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
                                    <h2>Trip details</h2>
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
                                        <label>vehicleId:</label>
                                        <div className="form-control select">
                                            <select className="select-group" name="vehicleId" onChange={onInputChange}>
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
                                    <h2>Fuel details (Optional)</h2>
                                </div>
                                <form onSubmit={onTripAdd}>
                                    <div className="input-group input-group-lg">
                                        <label>Used fuel:</label>
                                        <input className="form-control" type="text" placeholder="Used fuel" name="usedFuel" value={trip.usedFuel} onChange={onInputChange} />
                                        {trip.usedFuelError && <p className="invalid-field" >{trip.usedFuelError}</p>}
                                        {(trip.usedFuel && !trip.fuelPrice) && <p className="warning-field" >{ValidationErrors.fuelFields}</p>}
                                    </div>
                                    <div className="input-group input-group-lg">
                                        <label>Fuel price per liter:</label>
                                        <input className="form-control" type="text" placeholder="Fuel price" name="fuelPrice" value={trip.fuelPrice} onChange={onInputChange} />
                                        {trip.typeError && <p className="invalid-field">{trip.typeError}</p>}
                                        {(!trip.usedFuel && trip.fuelPrice) && <p className="warning-field" >{ValidationErrors.fuelFields}</p>}
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

export default IsLoadingHOC(AddTrip);