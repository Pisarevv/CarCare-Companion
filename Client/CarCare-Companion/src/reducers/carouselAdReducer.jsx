/**
 * carouselAdReducer  
 * ---------------------
 * Reducer used in create and edit carousel ads for storing state.
 * ---------------------- 
**/

const carouselAdReducer = (state, action) => {
    switch (action.type) {
        case "SET_USERFIRSTNAME":
            return { ...state, userFirstName: action.payload, userFirstNameError: "" };
        case "SET_STARSRATING":
            return { ...state, starsRating: action.payload, starsRatingError: "" };
        case "SET_DESCRIPTION": 
            return { ...state, description: action.payload, descriptionError: "" };
        case "SET_USERFIRSTNAME_ERROR":
            return { ...state, userFirstNameError: action.payload };
        case "SET_STARSRATING_ERROR":
            return { ...state, starsRatingError: action.payload };
        case "SET_DESCRIPTION_ERROR":
            return { ...state, descriptionError: action.payload };     
        default:    
            return state;
        }
}

export default carouselAdReducer;