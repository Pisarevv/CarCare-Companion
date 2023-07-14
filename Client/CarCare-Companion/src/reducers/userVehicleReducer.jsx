/**
 * userVehicleReducer 
 * ---------------------
 * Reducer used in create and edit user vehicles storing their state.
 * ---------------------- 
**/

const userVehicleReducer = (state, action) => {
    switch (action.type) {
        case "SET_MAKE":
            return { ...state, make: action.payload, makeError: "" };
        case "SET_MODEL":
            return { ...state, model: action.payload, modelError: "" };
        case "SET_VEHICLETYPE":
            return { ...state, vehicleType: action.payload, vehicleTypeError: "" };
        case "SET_MILEAGE":
            return { ...state, mileage: action.payload, mileageError: "" };
        case "SET_FUELTYPE":
            return { ...state, fuelType: action.payload, fuelTypeError: "" };
        case "SET_YEAR":
            return { ...state, year: action.payload, yearError: "" };
        case "SET_MAKE_ERROR":
            return { ...state, makeError: action.payload };
        case "SET_MODEL_ERROR":
            return { ...state, modelError: action.payload };
        case "SET_VEHICLETYPE_ERROR":
            return { ...state, vehicleTypeError: action.payload};
        case "SET_MILEAGE_ERROR":
            return { ...state, mileageError: action.payload };
        case "SET_FUELTYPE_ERROR":
            return { ...state, fuelTypeError: action.payload };
        case "SET_YEAR_ERROR":
            return { ...state, yearError: action.payload };
        default:    
            return state;
        }
}

export default userVehicleReducer;