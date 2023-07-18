/**
 * serviceRecordReducer  
 * ---------------------
 * Reducer used in create and edit service record storing their state.
 * ---------------------- 
**/


const serviceRecordReducer = (state, action) => {
    switch (action.type) {
        case "SET_TITLE":
            return { ...state, title: action.payload, titleError: "" };
        case "SET_PERFORMEDON":
            return { ...state, performedOn: action.payload, performedOnError: "" };
        case "SET_DESCRIPTION": 
            return { ...state, description: action.payload, descriptionError: "" };
        case "SET_MILEAGE":
            return { ...state, mileage: action.payload, mileageError: "" };
        case "SET_COST":
            return { ...state, cost: action.payload, costError: "" };
        case "SET_VEHICLE":
            return {...state, vehicle : action.payload, vehicleError: ""};
        case "SET_TITLE_ERROR":
            return { ...state, titleError: action.payload };
        case "SET_PERFORMEDON_ERROR":
            return { ...state, performedOnError: action.payload };
        case "SET_DESCRIPTION_ERROR":
            return { ...state, descriptionError: action.payload };
        case "SET_MILEAGE_ERROR":
            return { ...state, mileageError: action.payload };
        case "SET_COST_ERROR":
            return { ...state, costError: action.payload };
        case "SET_VEHICLE_ERROR":
            return {...state, vehicleError : action.payload};
        default:    
            return state;
        }
}

export default serviceRecordReducer;