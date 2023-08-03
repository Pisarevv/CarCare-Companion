import { useEffect, useReducer, useState } from 'react'

import { NavLink, useNavigate } from 'react-router-dom'

import taxRecordReducer from '../../../reducers/taxRecordReducer'

import IsLoadingHOC from '../../Common/IsLoadingHoc'
import useAxiosPrivate from '../../../hooks/useAxiosPrivate'

import { NotificationHandler } from '../../../utils/NotificationHandler'

import StringToISODateString from '../../../utils/StringToISODateString'
import DecimalSeparatorFormatter from '../../../utils/DecimalSeparatorFormatter'

import './AddTaxRecord.css'

const ValidationErrors = {
    emptyInput: "This field cannot be empty",
    inputNotNumber: "This field accepts only valid numbers",
    invalidDate : "The provided date is invalid - correct format example 25/03/2023"
}

const ValidationRegexes = {
    //Validates that the fuel price and travelled distance is a floating point  
    floatNumbersRegex: new RegExp(/^\d+(?:[.,]\d+)?$/),

    //Validates that the time format is dd/MM/yyyy
    timeFormatRegex: new RegExp(/^(0[1-9]|[1-2]\d|3[0-1])\/(0[1-9]|1[0-2])\/(\d{4})$/)

}

const AddTaxRecord = (props) => {

    const navigate = useNavigate();

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [userVehicles, setUserVehicles] = useState([]);

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

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getVehicles = async () => {
            try {
                const response = await axiosPrivate.get("/Vehicles",{
                    signal : controller.signal
                });

                if(isMounted){
                    if(response.data.length > 0){
                        setUserVehicles(userVehicles => response.data);
                        dispatch({ type: `SET_VEHICLEID`, payload: response.data[0].id })
                    }
                }
            } 
            catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally {
               setLoading(false);
            }
        }

        getVehicles();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    },[])


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
                isCostValid)
            {
                const { title, description, vehicleId } = taxRecord;
                const cost = DecimalSeparatorFormatter(taxRecord.cost);
                const validFrom = StringToISODateString(taxRecord.validFrom);
                const validTo = StringToISODateString(taxRecord.validTo);
                await axiosPrivate.post("/TaxRecords", {title, description, validFrom, validTo, cost, vehicleId})
                navigate('/taxRecords')
            } 

        }
        catch (error) {
            NotificationHandler(error);
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