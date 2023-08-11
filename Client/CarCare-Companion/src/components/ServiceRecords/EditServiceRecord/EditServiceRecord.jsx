// Importing necessary React hooks, React Router utilities, and utility functions.
import { useEffect, useReducer, useState } from 'react';
import { NavLink, useLocation, useNavigate, useParams } from 'react-router-dom';

// Reducer to handle service record updates.
import serviceRecordReducer from '../../../reducers/serviceRecordReducer';

// Utility to handle notifications.
import { NotificationHandler } from '../../../utils/NotificationHandler';

// Custom Axios hook for authenticated requests.
import useAxiosPrivate from '../../../hooks/useAxiosPrivate';

// A Higher-Order Component to display a loading spinner or the wrapped component.
import IsLoadingHOC from '../../Common/IsLoadingHoc';

// Utilities to format and validate date strings.
import StringToISODateString from '../../../utils/StringToISODateString';
import ISODateStringToString from '../../../utils/IsoDateStringToString';

// Utility to format numbers.
import DecimalSeparatorFormatter from '../../../utils/DecimalSeparatorFormatter';

// CSS styles specific to this component.
import './EditServiceRecord.css';

// Error messages to display when input validation fails.
const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    invalidDate: "The provided date is invalid - correct format example 25/03/2023"
};

// Regular expressions to validate the input format for various fields.
const ValidationRegexes = {
    // Validates that the fuel price and traveled distance is a floating point.
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),
    
    // Validates that the time format is dd/MM/yyyy.
    timeFormatRegex: new RegExp(/^(0[1-9]|[1-2]\d|3[0-1])\/(0[1-9]|1[0-2])\/(\d{4})$/)
};

/**
 * EditServiceRecord is a component that provides an interface for users to edit details
 * of a particular service record.
 * 
 * @param {Object} props - Props passed to the component.
 */
const EditServiceRecord = (props) => {
    // React Router's navigate function to programmatically change routes.
    const navigate = useNavigate();

    // Provides access to the current location (route).
    const location = useLocation();

    // Extracting the setLoading function from the props.
    const { setLoading } = props;

    // Custom hook for making authenticated Axios requests.
    const axiosPrivate = useAxiosPrivate();

    // Fetching the 'id' parameter from the URL.
    const { id } = useParams();

    // State to hold the list of user vehicles.
    const [userVehicles, setUserVehicles] = useState([]);

    // useReducer to manage the state of the service record and its associated validation errors.
    const [serviceRecord, dispatch] = useReducer(serviceRecordReducer, {
        title: "",
        performedOn: "",
        description: "",
        mileage: "",
        cost: "",
        vehicleId: "",
        titleError: "",
        performedOnError: "",
        descriptionError: "",
        mileageError: "",
        costError: "",
        vehicleIdError: ""
    });

    // Effect hook to fetch the list of user vehicles and the details of the service record to be edited.
    useEffect(() => {
        // To ensure that we don't set the state of an unmounted component.
        let isMounted = true;

        // To handle aborted requests.
        const controller = new AbortController();

        // Function to fetch user vehicles and service record details.
        const getDetails = async () => {
            try {
                const requests = [
                    axiosPrivate.get('/Vehicles', {
                        signal: controller.signal
                    }),
                    axiosPrivate.get(`/ServiceRecords/Details/${id}`, {
                        signal: controller.signal
                    })
                ];

                Promise.all(requests)
                    .then(responses => {
                        const userVehiclesResult = responses[0].data;
                        const serviceRecordDetails = responses[1].data;

                        // If the component is still mounted, update the states.
                        if (isMounted) {
                            setUserVehicles(userVehiclesResult);
                            setServiceRecordInitialDetails(serviceRecordDetails);
                            dispatch({ type: 'SET_VEHICLEID', payload: serviceRecordDetails.vehicleId });
                            dispatch({ type: 'SET_PERFORMEDON', payload: ISODateStringToString.ddmmyyyy(serviceRecordDetails.performedOn) });
                        }
                    });
            } catch (err) {
                // On error, show a notification and redirect to the login page.
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            } finally {
                // Stop showing the loading spinner.
                setLoading(false);
            }
        }

        getDetails();

        // Cleanup function to abort the requests if the component is unmounted.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, []);

     // Event handlers, validations, and utility functions.
    const setServiceRecordInitialDetails = (vehicleIdDetails) => {
        for (const property in vehicleIdDetails) {
            if (property == "performedOn") {
                continue;
            }
            dispatch({ type: `SET_${(property).toUpperCase()}`, payload: vehicleIdDetails[property] })
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

        if (value === "") {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
            return false;
        }
        if (!ValidationRegexes.floatNumbersRegex.test(value)) {
            dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
            return false;
        }
        return true;
    }

    const onServiceRecordEdit = async (e) => {
        e.preventDefault();

        try {
            let isTitleValid = validateTextFields("title", serviceRecord.title);
            let isPerformedOnValid = validateDateFields("performedOn", serviceRecord.performedOn);
            let isMileageValid = validateNumberFields("mileage", serviceRecord.mileage);
            let isvehicleIdValid = validateTextFields("vehicleId", serviceRecord.vehicleId);
            let isCostValid = validateNumberFields("cost", serviceRecord.cost);

            if (isTitleValid && isPerformedOnValid &&
                isMileageValid && isvehicleIdValid &&
                isCostValid) {
                const { title, description, vehicleId } = serviceRecord;
                const mileage = DecimalSeparatorFormatter(serviceRecord.mileage);
                const cost = DecimalSeparatorFormatter(serviceRecord.cost);
                const performedOn = StringToISODateString(serviceRecord.performedOn);
                const response = await axiosPrivate.patch(`/ServiceRecords/Edit/${id}`, {title, description, mileage, cost, vehicleId, performedOn})
                navigate('/ServiceRecords')
                NotificationHandler("Success", "Sucessfully edited service record!",response.status);
            }

        }
        catch (error) {
            window.scrollTo({ top: 0, behavior: 'smooth' });
            const {title, status} = error.response.data;
            NotificationHandler("Warning",title,status);
        }
    }

    return (
        <section className="edit-service-record-section">
            <div className="edit-service-record-container">
                <div className="edit-service-record-body">
                    <div className="edit-service-record-heading">
                        <h2>Edit record details</h2>
                    </div>
                    <form onSubmit={onServiceRecordEdit}>
                        <div className="input-group input-group-lg">
                            <label>Title:</label>
                            <input className="form-control" type="text" placeholder="Enter the service record title here" name="title" value={serviceRecord.title} onChange={onInputChange} />
                            {serviceRecord.titleError && <p className="invalid-field" >{serviceRecord.titleError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Date of performing:</label>
                            <input className="form-control" type="text" placeholder="DD/MM/YYYY" name="performedOn" value={serviceRecord.performedOn} onChange={onInputChange} />
                            {serviceRecord.performedOnError && <p className="invalid-field">{serviceRecord.performedOnError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Mileage:</label>
                            <input className="form-control" type="text" placeholder="Mileage in kilometers" name="mileage" value={serviceRecord.mileage} onChange={onInputChange} />
                            {serviceRecord.mileageError && <p className="invalid-field" >{serviceRecord.mileageError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Vehicle:</label>
                            <div className="form-control select">
                                <select className="select-group" name="vehicleId" onChange={onInputChange}>
                                    {userVehicles.map(uv => <option key={uv.id} value={uv.id}>{`${uv.make} ${uv.model}`}</option>)}
                                </select>
                            </div>
                            {serviceRecord.vehicleId && <p className="invalid-field" >{serviceRecord.vehicleIdError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Cost:</label>
                            <input className="form-control" type="text" placeholder="Enter cost in levs here" name="cost" value={serviceRecord.cost} onChange={onInputChange} />
                            {serviceRecord.costError && <p className="invalid-field" >{serviceRecord.costError}</p>}
                        </div>
                        <div className="input-group input-group-lg">
                            <label>Description:</label>
                            <input className="form-control" type="text" placeholder="Enter description here" name="description" value={serviceRecord.description} onChange={onInputChange} />
                            {serviceRecord.descriptionError && <p className="invalid-field" >{serviceRecord.descriptionError}</p>}
                        </div>
                        <NavLink className="float" to={`/ServiceRecords`}>Cancel</NavLink>
                        <button type="submit" className="float">Finish</button>
                    </form>

                </div>
            </div>
        </section>
    )
}

export default IsLoadingHOC(EditServiceRecord);