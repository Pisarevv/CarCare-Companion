// Importing necessary hooks and components from React and react-router-dom
import { NavLink, useLocation, useNavigate } from 'react-router-dom'
import { useEffect, useReducer, useState } from 'react'

// Reducer for handling the trip state
import tripReducer from '../../../reducers/tripReducer'

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../hooks/useDeauthenticate';

// Custom hook for authenticated axios requests
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";

// Higher order component to display a loading spinner
import IsLoadingHOC from '../../Common/IsLoadingHoc'

// Utility to handle notifications
import { NotificationHandler } from '../../../utils/NotificationHandler'

// Utility to format decimals with separators
import DecimalSeparatorFormatter from '../../../utils/DecimalSeparatorFormatter';

// Component specific styles
import './AddTrip.css'

// Static validation error messages
const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    fuelFields: "Both fields related to fuel have to be listed."
}

// Regular expressions for validation
const ValidationRegexes = {
    // Validates that the fuel price and travelled distance are floating points
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),
}

const AddTrip = (props) => {

    // Provides access to the current location (route) and navigation function.
    const location = useLocation();
    const navigate = useNavigate();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Initializing axios with authentication
    const axiosPrivate = useAxiosPrivate();

    // Destructuring the setLoading function from props
    const { setLoading } = props;

    // State to hold the list of user vehicles
    const [userVehicles, setUserVehicles] = useState([]);

    // State to determine if the first step of the form is complete
    const [stepOneFinished, setStepOneFinished] = useState(false);

    // Using the reducer to manage the state of the trip details
    const [trip, dispatch] = useReducer(tripReducer, {
        startDestination: "",
        endDestination: "",
        mileageTravelled: "",
        fuelPrice: "",
        usedFuel: "",
        vehicleId: "",

        // Error states for each input field
        startDestinationError: "",
        endDestinationError: "",
        mileageTravelledError: "",
        fuelPriceError: "",
        usedFuelError: "",
        vehicleIdError: ""
    });

    // Effect hook that runs once on component mount
    useEffect(() => {
        // A flag to check if the component is still mounted before updating its state
        let isMounted = true;
        // Initialization of AbortController for aborting fetch requests
        const controller = new AbortController();

        // Asynchronous function to fetch user vehicle IDs from the server
        const getUservehicleIds = async () => {
            try {
                const response = await axiosPrivate.get('/Vehicles', {
                    signal: controller.signal
                });

                // Setting user vehicles if component is still mounted
                if(isMounted) {
                    if(response.data.length > 0) {
                        setUserVehicles(userVehicles => response.data);
                        dispatch({ type: `SET_VEHICLEID`, payload: response.data[0].id })
                    }
                }
            } catch (err) {
                // Handle error and redirect to login in case of an error.
                if(err.response.status == 401){
                    // On error, show a notification and redirect to the login page.
                   NotificationHandler("Something went wrong","Plese log in again", 400);
                   logUserOut(location);
               }   
                const { title, status } = error.response.data;
                NotificationHandler("Warning", title, status);
            }
            finally {
                // Setting the loading state to false after data fetch completes
                setLoading(false);
            }
        }

        // Call the asynchronous function
        getUservehicleIds();

        // Cleanup function to run when the component unmounts
        return () => {
            isMounted = false;
            // Abort the fetch request if component is no longer mounted
            isMounted && controller.abort();
        }
    }, []) 

  // Event handlers, validations, and utility functions.
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

        if (value.trim() === "") {
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
            if (!trip.usedFuel && !trip.fuelPrice) {
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isVehicleIdValid) {
                    const { startDestination, endDestination, vehicleId } = trip;
                    const mileageTravelled = DecimalSeparatorFormatter(trip.mileageTravelled);
                    const usedFuel = null;
                    const fuelPrice = null; 
                    const response = await axiosPrivate.post("/Trips", { startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId});
                    navigate("/Trips");
                    NotificationHandler("Success", "Sucessfully added trip record!", response.status);
                }
            }
            else {
                let isFuelPriceValid = validateNumberFields("fuelPrice", trip.fuelPrice);
                let isUsedFuelValid = validateNumberFields("usedFuel", trip.usedFuel);
                if (isStartDestinationValid && isEndDestinationValid &&
                    isMileageValid && isFuelPriceValid &&
                    isUsedFuelValid && isVehicleIdValid) 
                {
                    const { startDestination, endDestination, vehicleId } = trip;
                    const mileageTravelled = DecimalSeparatorFormatter(trip.mileageTravelled);
                    const usedFuel = DecimalSeparatorFormatter(trip.usedFuel);
                    const fuelPrice = DecimalSeparatorFormatter(trip.fuelPrice);
                    const response = await axiosPrivate.post("/Trips", { startDestination, endDestination, mileageTravelled, usedFuel, fuelPrice, vehicleId});
                    navigate("/Trips");
                    NotificationHandler("Success", "Sucessfully added trip record!", response.status);
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
                                        <label>Vehicle:</label>
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
                                        {(!trip.usedFuel && trip.fuelPrice) && <p className="warning-field" >{ValidationErrors.fuelFields}</p>}
                                    </div>
                                    <div className="input-group input-group-lg">
                                        <label>Fuel price per liter:</label>
                                        <input className="form-control" type="text" placeholder="Fuel price" name="fuelPrice" value={trip.fuelPrice} onChange={onInputChange} />
                                        {trip.fuelPriceError && <p className="invalid-field">{trip.fuelPriceError}</p>}
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

export default IsLoadingHOC(AddTrip);