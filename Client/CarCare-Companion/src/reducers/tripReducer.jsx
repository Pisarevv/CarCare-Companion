/**
 * userProductReducer 
 * ---------------------
 * Reducer used in create and edit user trips storing their state.
 * ---------------------- 
**/


const tripReducer = (state, action) => {
    switch (action.type) {
        case "SET_STARTDESTINATION":
            return { ...state, startDestination: action.payload, startDestinationError: "" };
        case "SET_ENDDESTINATION":
            return { ...state, endDestination: action.payload, endDestinationError: "" };
        case "SET_TRAVELLEDMILEAGE":
            return { ...state, travelledMileage: action.payload, travelledMileageError: "" };
        case "SET_USEDFUEL":
            return { ...state, usedFuel: action.payload, usedFuelError: "" };
        case "SET_FUELPRICE":
            return { ...state, fuelPrice: action.payload, fuelPriceError: "" };
        case "SET_VEHICLE":
            return {...state, vehicle : action.payload, vehicleError: ""};
        case "SET_STARTDESTINATION_ERROR":
            return { ...state, startDestinationError: action.payload };
        case "SET_ENDDESTINATION_ERROR":
            return { ...state, endDestinationError: action.payload };
        case "SET_TRAVELLEDMILEAGE_ERROR":
            return { ...state, travelledMileageError: action.payload };
        case "SET_USEDFUEL_ERROR":
            return { ...state, usedFuelError: action.payload };
        case "SET_FUELPRICE_ERROR":
            return { ...state, fuelPriceError: action.payload };
        case "SET_VEHICLE_ERROR":
            return {...state, vehicleError : action.payload};
        default:    
            return state;
        }
}

export default tripReducer;