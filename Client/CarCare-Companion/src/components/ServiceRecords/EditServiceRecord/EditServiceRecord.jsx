import { useEffect, useReducer, useState } from 'react'

import { NavLink, useLocation, useNavigate, useParams } from 'react-router-dom'

import serviceRecordReducer from '../../../reducers/serviceRecordReducer'

import { NotificationHandler } from '../../../utils/NotificationHandler'

import StringToISODateString from '../../../utils/StringToISODateString'
import ISODateStringToString from '../../../utils/IsoDateStringToString'

import useAxiosPrivate from '../../../hooks/useAxiosPrivate'
import IsLoadingHOC from '../../Common/IsLoadingHoc'

import './EditServiceRecord.css'

const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    invalidDate: "The provided date is invalid - correct format example 25/03/2023"
}

const ValidationRegexes = {
    //Validates that the fuel price and travelled distance is a floating point  
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),

    //Validates that the time format is dd/MM/yyyy
    timeFormatRegex: new RegExp(/^(0[1-9]|[1-2]\d|3[0-1])\/(0[1-9]|1[0-2])\/(\d{4})$/)

}

const EditServiceRecord = (props) => {

    const navigate = useNavigate();

    const location = useLocation();

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const { id } = useParams();

    const [userVehicles, setUserVehicles] = useState([]);

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

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

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

                        if (isMounted) {
                            setUserVehicles(userVehicles => userVehiclesResult);
                            setServiceRecordInitialDetails(serviceRecordDetails)

                            dispatch({ type: `SET_VEHICLEID`, payload: serviceRecordDetails.vehicleId })
                            dispatch({ type: `SET_PERFORMEDON`, payload: ISODateStringToString.ddmmyyyy(serviceRecordDetails.performedOn) })
                        }
                    });

            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
                setLoading(false);
            }
        }

        getDetails();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])


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
                const { title, description, mileage, cost, vehicleId } = serviceRecord;
                const performedOn = StringToISODateString(serviceRecord.performedOn);
                await axiosPrivate.patch(`/ServiceRecords/Edit/${id}`, {title, description, mileage, cost, vehicleId, performedOn})
                navigate('/ServiceRecords')
            }

        }
        catch (error) {
            NotificationHandler(error);
            navigate('/ServiceRecords')
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