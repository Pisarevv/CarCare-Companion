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
        case "SET_VEHICLETYPEID":
            return { ...state, vehicleTypeId: action.payload, vehicleTypeError: "" };
        case "SET_MILEAGE":
            return { ...state, mileage: action.payload, mileageError: "" };
        case "SET_FUELTYPEID":
            return { ...state, fuelTypeId: action.payload, fuelTypeError: "" };
        case "SET_YEAR":
            return { ...state, year: action.payload, yearError: "" };
        case "SET_MAKE_ERROR":
            return { ...state, makeError: action.payload };
        case "SET_MODEL_ERROR":
            return { ...state, modelError: action.payload };
        case "SET_VEHICLETYPEID_ERROR":
            return { ...state, vehicleTypeIdError: action.payload};
        case "SET_MILEAGE_ERROR":
            return { ...state, mileageError: action.payload };
        case "SET_FUELTYPEID_ERROR":
            return { ...state, fuelTypeIdError: action.payload };
        case "SET_YEAR_ERROR":
            return { ...state, yearError: action.payload };
        default:    
            return state;
        }
}

export default userVehicleReducer;