// React hooks for side-effects, state management and reducer pattern
import { useEffect, useReducer, useState } from 'react';

// React-router-dom hooks for navigation and accessing route parameters
import { NavLink, useLocation, useParams } from 'react-router-dom';

// Reducer function for tax record state management
import taxRecordReducer from '../../../reducers/taxRecordReducer';

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../hooks/useDeauthenticate';

// Custom Axios hook for making private (authenticated) requests
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// Higher-Order Component for loading state
import IsLoadingHOC from '../../Common/IsLoadingHoc';

// Notification handling utility
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Utility functions for date string manipulation and formatting
import StringToISODateString from '../../../utils/StringToISODateString';
import ISODateStringToString from '../../../utils/IsoDateStringToString';
import DecimalSeparatorFormatter from '../../../utils/DecimalSeparatorFormatter';

// Component specific styles
import './EditTaxRecord.css';

// Object containing error messages for validation
const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    invalidDate: "The provided date is invalid - correct format example 25/03/2023"
};

// Regexes for float numbers and date validation
const ValidationRegexes = {
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),
    timeFormatRegex: new RegExp(/^(0[1-9]|[1-2]\d|3[0-1])\/(0[1-9]|1[0-2])\/(\d{4})$/)
};

/**
 * EditTaxRecord component allows users to edit a specific tax record.
 * The record details are fetched from an API and can be updated.
 *
 * @param {Object} props - Props passed to the component.
 */
const EditTaxRecord = (props) => {

    // Provides access to the current location (route).
    const location = useLocation();
   
    //Use custom hook to get logUseOut function
    const logUserOut = useDeauthenticate();

    // Extract tax record ID from the URL
    const { id } = useParams();

    // setLoading function from props, used by HOC
    const { setLoading } = props;

    // Axios instance for authenticated requests
    const axiosPrivate = useAxiosPrivate();

    // State for user vehicles
    const [userVehicles, setUserVehicles] = useState([]);

    // State for tax record and its related errors using reducer pattern
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

    // useEffect hook to fetch vehicles and tax record details on component mount
    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getVehicles = async () => {
            try {
                const requests = [
                    axiosPrivate.get("/Vehicles", {
                        signal: controller.signal
                    }),
                    axiosPrivate.get(`/TaxRecords/Details/${id}`, {
                        signal: controller.signal
                    })
                ];

                Promise.all(requests)
                    .then(responses => {
                        const userVehicleResult = responses[0].data;
                        const taxRecordDetails = responses[1].data;

                        if (isMounted) {
                            setUserVehicles(userVehicleResult);
                            setTaxRecordInitialDetails(taxRecordDetails);
                        }
                    })
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
                setLoading(false);
            }
        }

        getVehicles();

        // Clean-up function to abort any pending requests
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, []);

    // Function to set the initial details of the tax record fetched
    const setTaxRecordInitialDetails = (vehicleDetails) => {
        for (const property in vehicleDetails) {
            if (property === "validFrom" || property === "validTo") {
                dispatch({ 
                    type: `SET_${(property).toUpperCase()}`,
                    payload: ISODateStringToString.ddmmyyyy(vehicleDetails[property])
                });
                continue;
            }
            dispatch({
                type: `SET_${(property).toUpperCase()}`,
                payload: vehicleDetails[property]
            });
        }
    }

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

        if (value == "") {
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
            let isVehicleValid = validateTextFields("vehicle", taxRecord.vehicleId);
            let isCostValid = validateNumberFields("cost", taxRecord.cost);

            if (isTitleValid && isValidFromValid &&
                isValidToValid && isVehicleValid &&
                isCostValid) {
                const { title, description, vehicleId } = taxRecord;
                const cost = DecimalSeparatorFormatter(taxRecord.cost);
                const validFrom = StringToISODateString(taxRecord.validFrom);
                const validTo = StringToISODateString(taxRecord.validTo);
                const response = await axiosPrivate.patch(`/TaxRecords/Edit/${id}`, { title, description, validTo, validFrom, cost, vehicleId });
                navigate('/taxRecords')
                NotificationHandler("Success", "Sucessfully edited tax record!", response.status);
            }

        }
        catch (error) {
            window.scrollTo({ top: 0, behavior: 'smooth' });
            const { title, status } = error.response.data;
            NotificationHandler("Warning", title, status);
        }
    }

    return (
        <section className="edit-tax-record-section">
            <div className="edit-tax-record-container">
                <div className="edit-tax-record-body">
                    <div className="edit-tax-record-heading">
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

export default IsLoadingHOC(EditTaxRecord);