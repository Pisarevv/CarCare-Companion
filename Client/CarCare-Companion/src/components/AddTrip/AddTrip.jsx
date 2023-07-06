import { NavLink, useNavigate } from 'react-router-dom'
import { useEffect, useReducer, useState } from 'react'

import tripReducer from '../../reducers/tripReducer'

import './AddTrip.css'
import { getUserVehicles } from '../../services/vehicleService'
import IsLoadingHOC from '../Common/IsLoadingHoc'
import { ErrorHandler } from '../../utils/ErrorHandler/ErrorHandler'
import { createTrip } from '../../services/tripService'

const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    fuelFields : "Both fields related to fuel have to be listed."
}

const ValidationRegexes = {
    //Validates that the fuel price and travelled distance is a floating point  
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),
}

const AddTrip = (props) => {

    const navigate = useNavigate();

    const { setLoading } = props;

    const [userVehicles, setUserVehicles] = useState([]);


    const [trip, dispatch] = useReducer(tripReducer, {
        startDestination: "",
        endDestination: "",
        travelledMileage: "",
        fuelPrice: "",
        usedFuel: "",
        vehicle: "",

        startDestinationError: "",
        endDestinationError: "",
        travelledMileageError: "",
        fuelPriceError: "",
        usedFuelError: "",
        vehicleError: ""

    });

    useEffect(() => {
        (async () => {
            try {
                let userVehicleResult = await getUserVehicles()
                setUserVehicles(userVehicles => userVehicleResult);
                dispatch({ type: `SET_VEHICLE`, payload: userVehicleResult[0].id })
                setLoading(false);
            }
            catch (error) {
                ErrorHandler(error);
                setLoading(false);
            }
        })()
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



    const onTripAdd = async (e) => {
        e.preventDefault();
        try {
            let isStartDestinationValid = validateTextFields("startDestination",trip.startDestination);
            let isEndDestinationValid = validateTextFields("endDestination",trip.endDestination);
            let isMileageValid = validateNumberFields("travelledMileage",trip.travelledMileage);
            let isVehicleValid = validateTextFields("vehicle",trip.vehicle);
            if (!trip.usedFuel || !trip.fuelPrice) {
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isVehicleValid) 
                {
                    const {startDestination,endDestination,travelledMileage,usedFuel,fuelPrice,vehicle} = trip;
                    await createTrip(startDestination,endDestination,travelledMileage,usedFuel,fuelPrice,vehicle);
                }
            }
            else {
                let isFuelPriceValid = validateNumberFields("fuelPrice",trip.fuelPrice);
                let isUsedFuelValid = validateNumberFields("usedFuel",trip.usedFuel);
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isFuelPriceValid &&
                    isUsedFuelValid && isVehicleValid)
                {
                    const {startDestination,endDestination,travelledMileage,usedFuel,fuelPrice,vehicle} = trip;
                    await createTrip(startDestination,endDestination,travelledMileage,usedFuel,fuelPrice,vehicle);
                }
            }

        }
        catch (error) {
            ErrorHandler(error);
            navigate('/Trips')
        }
    }

    return (
        <section className="add-trip">
            <div className="add-trip-container">
                <div className="add-trip-body">
                    <div className="add-trip-heading">
                        <h2>Trip details</h2>
                        <form onSubmit={onTripAdd}>
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
                                <input className="form-control" type="text" placeholder="Travelled mileage" name="travelledMileage" value={trip.travelledMileage} onChange={onInputChange} />
                                {trip.travelledMileage && <p className="invalid-field" >{trip.travelledMileageError}</p>}
                            </div>
                            <div className="input-group input-group-lg">
                                <label>Used fuel:</label>
                                <input className="form-control" type="text" placeholder="Used fuel" name="usedFuel" value={trip.usedFuel} onChange={onInputChange} />
                                {trip.usedFuelError && <p className="invalid-field" >{trip.usedFuelError}</p>}
                                {(trip.usedFuelError && !trip.fuelPrice ) && <p className="invalid-field" >{ValidationErrors.fuelFields}</p>}
                            </div>
                            <div className="input-group input-group-lg">
                                <label>Fuel price:</label>
                                <input className="form-control" type="text" placeholder="Fuel price" name="fuelPrice" value={trip.fuelPrice} onChange={onInputChange} />
                                {trip.typeError && <p className="invalid-field">{trip.typeError}</p>}
                                {(!trip.usedFuel && trip.fuelPrice) && <p className="invalid-field" >{ValidationErrors.fuelFields}</p>}
                            </div>
                            <div className="input-group input-group-lg">
                                <label>Vehicle:</label>
                                <div className="form-control select">
                                    <select className="select-group" name="vehicle" onChange={onInputChange}>
                                        {userVehicles.map(uv => <option key={uv.id} value={uv.id}>{`${uv.make} ${uv.model}`}</option>)}
                                    </select>
                                </div>
                                {trip.vehicle && <p className="invalid-field" >{trip.vehicleError}</p>}
                            </div>
                            <NavLink className="float" to={`/Trips`}>Cancel</NavLink>
                            <button type="submit" className="float">Add trip</button>
                        </form>
                    </div>
                </div>
            </div>
        </section>
    )
}

export default IsLoadingHOC(AddTrip);