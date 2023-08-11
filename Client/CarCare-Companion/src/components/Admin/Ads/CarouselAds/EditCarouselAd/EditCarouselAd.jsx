// React's hooks for managing side-effects and state.
import { useEffect, useReducer } from 'react';

// React Router hooks and components for navigation and retrieving route parameters.
import { NavLink, useNavigate, useParams } from 'react-router-dom';

// Reducer function specific to the carousel ad's operations.
import carouselAdReducer from '../../../../../reducers/carouselAdReducer';

// Custom hook to make authenticated Axios requests.
import useAxiosPrivate from '../../../../../hooks/useAxiosPrivate';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../../.././../Common/IsLoadingHoc';

// Utility to handle notifications.
import { NotificationHandler } from '../../../../../utils/NotificationHandler';

// CSS styles specific to this component.
import './EditCarouselAd.css';

// Validation constants and regular expressions.
const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only numbers between 1 and 5",
};

const ValidationRegexes = {
    // Validates that the stars rating is an integer between 1 and 5.
    floatNumbersRegex: new RegExp(/^[1-5]$/),
};

const EditCarouselAd = (props) => {

    // Hooks to navigate programmatically and retrieve route parameters.
    const navigate = useNavigate();
    const { id } = useParams();

    // Extract the setLoading function from the props, which controls the loading state.
    const { setLoading } = props;

    // Instantiate the useAxiosPrivate hook to get an instance of Axios with authentication headers.
    const axiosPrivate = useAxiosPrivate();

    // Using useReducer to manage state related to the carousel ad.
    const [carouselAd, dispatch] = useReducer(carouselAdReducer, {
        userFirstName: "",
        starsRating: "",
        description: "",
        userFirstNameError: "",
        starsRatingError: "",
        descriptionError: "",
    });

    // UseEffect hook to fetch the carousel ad details when the component mounts.
    useEffect(() => {
        // Flag to ensure asynchronous tasks don't update state after the component is unmounted.
        let isMounted = true;

        // Create an AbortController to cancel the fetch request in case of unmounting.
        const controller = new AbortController();

        const getCarouselAd = async () => {
            try {
                const request = await axiosPrivate.get(`/Ads/CarouselAds/Details/${id}`, {
                    signal: controller.signal
                });

                // If the component is still mounted, update the state.
                isMounted && setCarouselAdInitialDetails(request.data);
            } 
            catch (err) {
                // Handle any error that arises during the fetch operation.
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
               setLoading(false);
            }
        }

        // Call the function to fetch the carousel ad details.
        getCarouselAd();

        // Cleanup function: run this if the component unmounts.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    },[])

    const setCarouselAdInitialDetails = (vehicleDetails) => {
        for (const property in vehicleDetails) {
          dispatch({ type: `SET_${(property).toUpperCase()}`, payload: vehicleDetails[property] })
        }
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

        if (!ValidationRegexes.floatNumbersRegex.test(value) || value == "") {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
            return false;
        }
        return true;
    }

    const onCarouselAdEdit = async (e) => {
        e.preventDefault();

        try {
            let isFirstNameValid = validateTextFields("userFirstName", carouselAd.userFirstName);
            let isStarsRatingValid = validateNumberFields("starsRating", carouselAd.starsRating);
            let isdescriptionValid = validateTextFields("description", carouselAd.description);

            if (isFirstNameValid && isStarsRatingValid && isdescriptionValid)
            {
                const {userFirstName, description, starsRating} = carouselAd;
                await axiosPrivate.patch(`Ads/CarouselAds/Edit/${id}`, {userFirstName, description, starsRating});
                navigate('/Administrator/CarouselAds');
            } 

        }
        catch (error) {
            NotificationHandler(error);
        }
    }

    return (
        <section className="edit-carousel-ad-section">
            <div className="edit-carousel-ad-container">
                <div className="edit-carousel-ad-body">
                    <div className="edit-carousel-ad-heading">
                        <h2>Carousel ad details</h2>
                    </div>
                    <form onSubmit={onCarouselAdEdit}>
                        <div className="input-group input-group-lg">
                            <label>User first name:</label>
                            <input className="form-control" type="text" placeholder="Enter user first name here" name="userFirstName" value={carouselAd.userFirstName} onChange={onInputChange} />
                            {carouselAd.userFirstNameError && <p className="invalid-field" >{carouselAd.userFirstNameError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Stars rating:</label>
                            <input className="form-control" type="text" placeholder="Between 1 and 5" name="starsRating" value={carouselAd.starsRating} onChange={onInputChange} />
                            {carouselAd.starsRatingError && <p className="invalid-field">{carouselAd.starsRatingError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Description:</label>
                            <input className="form-control" type="text" placeholder="Enter description here" name="description" value={carouselAd.description} onChange={onInputChange} />
                            {carouselAd.descriptionError && <p className="invalid-field" >{carouselAd.descriptionError}</p>}
                        </div>
                        <NavLink className="float" to={`/Administrator/CarouselAds`}>Cancel</NavLink>
                        <button type="submit" className="float">Finish</button>
                    </form>

                </div>
            </div>
        </section>
    )
}

export default IsLoadingHOC(EditCarouselAd);