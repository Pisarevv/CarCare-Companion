import { useEffect, useReducer } from 'react'

import { NavLink, useNavigate, useParams } from 'react-router-dom'

import carouselAdReducer from '../../../../../reducers/carouselAdReducer'

import useAxiosPrivate from '../../../../../hooks/useAxiosPrivate'
import IsLoadingHOC from '../../.././../Common/IsLoadingHoc'

import { NotificationHandler } from '../../../../../utils/NotificationHandler'


import './EditCarouselAd.css'



const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only numbers between 1 and 5",
}

const ValidationRegexes = {
    //Validates that the stars rating is an integer between 1 and 5
    floatNumbersRegex: new RegExp(/^[1-5]$/),
}

const EditCarouselAd = (props) => {

    const navigate = useNavigate();

    const { id } = useParams();

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [carouselAd, dispatch] = useReducer(carouselAdReducer, {
        userFirstName: "",
        starsRating: "",
        description: "",

        userFirstNameError: "",
        starsRatingError: "",
        descriptionError: "",
    });

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getCarouselAd = async () => {
            try {
                const request = await axiosPrivate.get(`/Ads/CarouselAds/Details/${id}`,{
                    signal : controller.signal
                });

                isMounted && setCarouselAdInitialDetails(request.data);
            } 
            catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
               setLoading(false);
            }
        }

        getCarouselAd();

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