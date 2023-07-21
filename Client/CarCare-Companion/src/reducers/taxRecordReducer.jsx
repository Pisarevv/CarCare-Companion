/**
 * taxRecordReducer  
 * ---------------------
 * Reducer used in create and edit tax record for storing state.
 * ---------------------- 
**/


const taxRecordReducer = (state, action) => {
    switch (action.type) {
        case "SET_TITLE":
            return { ...state, title: action.payload, titleError: "" };
        case "SET_VALIDFROM":
            return { ...state, validFrom: action.payload, validFromError: "" };
        case "SET_VALIDTO":
            return { ...state, validTo: action.payload, validToError: "" };
        case "SET_DESCRIPTION": 
            return { ...state, description: action.payload, descriptionError: "" };
        case "SET_COST":
            return { ...state, cost: action.payload, costError: "" };
        case "SET_VEHICLEID":
            return {...state, vehicleId : action.payload, vehicleError: ""};
        case "SET_TITLE_ERROR":
            return { ...state, titleError: action.payload };
        case "SET_VALIDFROM_ERROR":
            return { ...state, validFromError: action.payload };
        case "SET_VALIDTO_ERROR":
                return { ...state, validToError: action.payload };
        case "SET_DESCRIPTION_ERROR":
            return { ...state, descriptionError: action.payload };     
        case "SET_COST_ERROR":
            return { ...state, costError: action.payload };
        case "SET_VEHICLEID_ERROR":
            return {...state, vehicleIdError : action.payload};
        default:    
            return state;
        }
}

export default taxRecordReducer;