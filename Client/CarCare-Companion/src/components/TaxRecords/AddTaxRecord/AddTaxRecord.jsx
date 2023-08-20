// React hooks for side-effects, state management, and the reducer pattern
import { useEffect, useReducer, useState } from 'react';

// React-router-dom hooks for navigation
import { NavLink, useLocation, useNavigate } from 'react-router-dom';

// Reducer function for managing tax record state
import taxRecordReducer from '../../../reducers/taxRecordReducer';

// Higher-Order Component for showing a loading state
import IsLoadingHOC from '../../Common/IsLoadingHoc';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../hooks/useDeauthenticate';

// Custom Axios hook for making authenticated requests
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Helper utility to handle notifications (e.g., error messages)
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Utility functions for date and number formatting
import StringToISODateString from '../../../utils/StringToISODateString';
import DecimalSeparatorFormatter from '../../../utils/DecimalSeparatorFormatter';

// Component specific styles
import './AddTaxRecord.css';

// Pre-defined validation error messages for user input
const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    invalidDate: "The provided date is invalid - correct format example 25/03/2023"
};

// Regular Expressions for validating user input
const ValidationRegexes = {
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),  // Validates floating-point numbers
    timeFormatRegex: new RegExp(/^(0[1-9]|[1-2]\d|3[0-1])\/(0[1-9]|1[0-2])\/(\d{4})$/)  // Validates dates in the format dd/MM/yyyy
};

/**
 * AddTaxRecord component allows users to add a tax record.
 * This component fetches available vehicles for the user and 
 * provides a form to input details of a new tax record.
 *
 * @param {Object} props - Props passed to the component.
 */
const AddTaxRecord = (props) => {

    // Extract setLoading function from props to control loading state
    const { setLoading } = props;

    // Provides access to the current location (route) and navigation function.
    const location = useLocation();
    const navigate = useNavigate();

    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Axios instance for authenticated requests
    const axiosPrivate = useAxiosPrivate();

    // State to store the list of user vehicles
    const [userVehicles, setUserVehicles] = useState([]);

    // State to manage tax record details and related errors using the reducer pattern
    const [taxRecord, dispatch] = useReducer(taxRecordReducer, {
        title: "",
        validFrom: "",
        validTo: "",
        description: "",
        cost: "",
        vehicleId: "",
        titleError: "",
        validFromError: "",
        validToError: "",
        descriptionError: "",
        costError: "",
        vehicleIdError: ""
    });

    // useEffect hook to fetch user vehicles from the API on component mount
    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        // Fetches the list of vehicles associated with the user
        const getVehicles = async () => {
            try {
                const response = await axiosPrivate.get("/Vehicles", {
                    signal: controller.signal
                });

                if (isMounted && response.data.length > 0) {
                    setUserVehicles(response.data);
                    dispatch({ type: `SET_VEHICLEID`, payload: response.data[0].id }); // Set the first vehicle as default
                }
            }
            catch (err) {
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
                setLoading(false); // Turn off loading state
            }
        }

        getVehicles();

        // Clean-up function to abort any pending requests when component is unmounted
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, []);


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

    const validateDateFields = (target, value) => {
        if (value.trim() === "") {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
            return false;
        }

        if (!ValidationRegexes.timeFormatRegex.test(value)) {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.invalidDate });
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

    const ontaxRecordAdd = async (e) => {
        e.preventDefault();

        try {
            let isTitleValid = validateTextFields("title", taxRecord.title);
            let isValidFromValid = validateDateFields("validFrom", taxRecord.validFrom);
            let isValidToValid = validateDateFields("validTo", taxRecord.validTo);
            let isVehicleValid = validateTextFields("vehicleId", taxRecord.vehicleId);
            let isCostValid = validateNumberFields("cost", taxRecord.cost);

            if (isTitleValid && isValidFromValid &&
                isValidToValid && isVehicleValid &&
                isCostValid) {
                const { title, description, vehicleId } = taxRecord;
                const cost = DecimalSeparatorFormatter(taxRecord.cost);
                const validFrom = StringToISODateString(taxRecord.validFrom);
                const validTo = StringToISODateString(taxRecord.validTo);
                const response = await axiosPrivate.post("/TaxRecords", { title, description, validFrom, validTo, cost, vehicleId })
                navigate('/taxRecords');
                NotificationHandler("Success", "Sucessfully added tax record!", response.status);
            }

        }
        catch (error) {
            window.scrollTo({ top: 0, behavior: 'smooth' });
            const { title, status } = error.response.data;
            NotificationHandler("Warning", title, status);
        }
    }

    return (
        <section className="add-tax-record-section">
            <div className="add-tax-record-container">
                <div className="add-tax-record-body">
                    <div className="add-tax-record-heading">
                        <h2>Tax record details</h2>
                    </div>
                    <form onSubmit={ontaxRecordAdd}>
                        <div className="input-group input-group-lg">
                            <label>Title:</label>
                            <input className="form-control" type="text" placeholder="Enter the tax record title here" name="title" value={taxRecord.title} onChange={onInputChange} />
                            {taxRecord.titleError && <p className="invalid-field" >{taxRecord.titleError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Valid from:</label>
                            <input className="form-control" type="text" placeholder="DD/MM/YYYY" name="validFrom" value={taxRecord.validFrom} onChange={onInputChange} />
                            {taxRecord.validFromError && <p className="invalid-field">{taxRecord.validFromError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Valid to:</label>
                            <input className="form-control" type="text" placeholder="DD/MM/YYYY" name="validTo" value={taxRecord.validTo} onChange={onInputChange} />
                            {taxRecord.validToError && <p className="invalid-field" >{taxRecord.validToError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Vehicle:</label>
                            <div className="form-control select">
                                <select className="select-group" name="vehicleId" onChange={onInputChange}>
                                    {userVehicles.map(uv => <option key={uv.id} value={uv.id}>{`${uv.make} ${uv.model}`}</option>)}
                                </select>
                            </div>
                            {taxRecord.vehicleIdError && <p className="invalid-field" >{taxRecord.vehicleIdError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Cost:</label>
                            <input className="form-control" type="text" placeholder="Enter cost in levs here" name="cost" value={taxRecord.cost} onChange={onInputChange} />
                            {taxRecord.costError && <p className="invalid-field" >{taxRecord.costError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Description:</label>
                            <input className="form-control" type="text" placeholder="Enter description here" name="description" value={taxRecord.description} onChange={onInputChange} />
                            {taxRecord.descriptionError && <p className="invalid-field" >{taxRecord.descriptionError}</p>}
                        </div>
                        <NavLink className="float" to={`/taxRecords`}>Cancel</NavLink>
                        <button type="submit" className="float">Finish</button>
                    </form>

                </div>
            </div>
        </section>
    )
}

export default IsLoadingHOC(AddTaxRecord);