/**
 * AddVehicle Component
 * ---------------------
 * This component displays a form that is available for registered user.
 * The user can add a vehicle to his collection of vehicles.
 * If all fields are valid the vehicle is created and added.
 * ---------------------- 
 * 
 * States:
 * ----------------------
 * - userVehicle (object): This object contains the properties of the product
 *   and errors that can occur on them. The userVehicle is controlled by a reducer.
 * ---------------
 * 
 * Functions:
 * -----------------
 * - onInputChange:
 *  Generic function updating the userVehicle of a property.
 * - validateTextFields:
 *  Function that validates that a field is not blank.
 *  There is a possibility to add different validation.
 * - validateNumberFields:
 *  Function that validates fields that a field is not blank and contains digits only.
 *  There is a possibility to add different validation.
 * - onVehicleAdd:
 *  Function that creates the product if all of the properties are valid.
 *  If the request is successful it redirects to the user products listing page - ("/recylce").
 * -----------------
 * 
 * - ErrorHandler
 *  This is a custom function that handles errors thrown by the REST api  
 *  and based on the error shows the user notifications.
 * -----------------
**/

import { useEffect, useReducer, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";

import { ErrorHandler } from "../../utils/ErrorHandler/ErrorHandler";

import { uploadImage } from "../../services/fileService";

import IsLoadingHOC from '../Common/IsLoadingHoc';

import userVehicleReducer from "../../reducers/userVehicleReducer";
import dataURLtoFile from "../../utils/ErrorHandler/URLtoFileConverter";

import './AddVehicle.css'
import { getFuelTypes, getVehicleTypes } from "../../services/vehicleService";



const ValidationErrors = {
  emptyInput: "This field cannot be empty",
  inputNotNumber: "This field accepts only valid numbers"
}

const ValidationRegexes = {
  //Validates that the year is an integer 
  yearRegex: new RegExp(/^[0-9]*$/)
}


const AddVehicle = (props) => {

  const navigate = useNavigate();

  const { setLoading } = props;

  const [vehicleImage, setVehicleImage] = useState(null);
  const [vehicleId, setVehicleId] = useState("");

  const [fuelTypes, setFuelTypes] = useState([]);
  const [vehicleTypes, setVehicleTypes] = useState([]);
  const [stepOneFinished, setStepOneFinished] = useState(false);

  const [userVehicle, dispatch] = useReducer(userVehicleReducer, {
    make: "",
    mileage: "",
    fuelType: "",
    model: "",
    type: "",
    year: "",

    makeError: "",
    mileageError: "",
    fuelTypeError: "",
    modelError: "",
    typeError: "",
    yearError: ""
  })

  useEffect(() => {
    (async () => {
      try {
        var fuelTypesResult = await getFuelTypes();
        var vehicleTypesResult = await getVehicleTypes();
        setFuelTypes(fuelTypes => fuelTypesResult);
        setVehicleTypes(vehicleTypes => vehicleTypesResult);

        setLoading(false);
      }
      catch (error) {
        ErrorHandler(error);
        setLoading(false);
      }

    })()
  }, []);


  //Event handlers
  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    const reader = new FileReader();

    reader.onload = () => {
      const imageDataUrl = reader.result;
      setVehicleImage(imageDataUrl);
    };

    if (file) {
      reader.readAsDataURL(file);
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
    if (target === "price") {
      if (!ValidationRegexes.priceRegex.test(value) || value.trim() === "") {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
        return false;
      }
      return true;
    }
    if (target === "year") {
      if (!ValidationRegexes.yearRegex.test(value) || value.trim() === "") {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
        return false;
      }
      return true;
    }
  }

  const onVehicleAdd = async (e) => {
    e.preventDefault();
    try {
      let isMakeValid = validateTextFields("make", userVehicle.make);
      let isMileageValid = validateTextFields("mileage", userVehicle.mileage);
      let isFuelTypeValid = validateTextFields("fuelType", userVehicle.fuelType);
      let isModelValid = validateTextFields("model", userVehicle.model);
      let isTypeValid = validateTextFields("type", userVehicle.type);
      let isYearValid = validateNumberFields("year", userVehicle.year);

      if (isMakeValid && isMileageValid &&
        isFuelTypeValid && isModelValid &&
        isTypeValid && isYearValid
      ) {
        let { make, mileage, fuelType, model, type, year } = userVehicle;
        // const formData = new FormData();
        // formData.append('file', dataURLtoFile(vehicleImage, "test"));

        // await uploadImage(formData, "multipart/form-data");
        var response = await createUserVehicle({ make, mileage, fuelType, model, price, type, year });
        setVehicleId(vehicleId => response);
        setStepOneFinished(true);
        // navigate("/recycle/page/1");

      }
      else {
        throw "Invalid input fields"
      }
    } catch (error) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      ErrorHandler(error)
    }
  }

  const onImageAdd = async (e) => {
    e.preventDefault();
    try {
      const formData = new FormData();
      formData.append('file', dataURLtoFile(vehicleImage, "test"));
      await uploadImage(formData, "multipart/form-data");

    } catch (error) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      ErrorHandler(error)
    }
  }


  return (
    <section className="add-vehicle">
      <div className="add-container">
        <div className="add-vehicle-body">
          <div className="add-vehicle-heading">
            {
              stepOneFinished == false
                ?
                <>
                  <h2>Vehicle details</h2>
                  <form onSubmit={onVehicleAdd}>
                    <div className="input-group input-group-lg">
                      <label>Make:</label>
                      <input className="form-control" type="text" placeholder="Vehicle make" name="make" value={userVehicle.make} onChange={onInputChange} />
                      {userVehicle.makeError && <p className="invalid-field" >{userVehicle.makeError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Model:</label>
                      <input className="form-control" type="text" placeholder="Model" name="model" value={userVehicle.model} onChange={onInputChange} />
                      {userVehicle.modelError && <p className="invalid-field">{userVehicle.modelError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Fuel type:</label>
                      <div className="form-control select">
                        <select className="select-group" name="fueltype" onChange={onInputChange}>
                          {fuelTypes.map(ft => <option key={ft.id} value={ft.id}>{ft.name}</option>)}
                        </select>
                      </div>
                      {userVehicle.fuelType && <p className="invalid-field" >{userVehicle.fuelTypeError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Mileage:</label>
                      <input className="form-control" type="text" placeholder="Mileage" name="mileage" value={userVehicle.mileage} onChange={onInputChange} />
                      {userVehicle.mileageError && <p className="invalid-field" >{userVehicle.mileageError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Vehicle type:</label>
                      <div className="form-control select">
                        <select className="select-group" name="type" onChange={onInputChange}>
                          {vehicleTypes.map(ft => <option key={ft.id} value={ft.id}>{ft.name}</option>)}
                        </select>
                      </div>
                      {userVehicle.typeError && <p className="invalid-field">{userVehicle.typeError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Year:</label>
                      <input className="form-control" type="text" placeholder="Year" name="year" value={userVehicle.year} onChange={onInputChange} />
                      {userVehicle.yearError && <p className="invalid-field">{userVehicle.yearError}</p>}
                    </div>
                    <NavLink className="float" to={`/`}>Cancel</NavLink>
                    <button type="submit" className="float">Next step</button>
                  </form>
                </>
                :
                <>
                  <h2>Vehicle image</h2>
                  <form onSubmit={onImageAdd}>
                    <div className="input-group input-group-lg img-group">
                      <label>Image: (Optional)</label>
                      {vehicleImage && <img className="vehicle-image" src={vehicleImage} alt="Selected" />}
                      <label className="select-img-button" htmlFor="file" >Select Image</label>
                      <input className="form-control" type="file" accept="image/*" id="file" onChange={handleImageUpload} />
                    </div>
                    <NavLink className="float" to={`/recycle/page/1`}>Cancel</NavLink>
                    <button type="submit" className="float">Add vehicle</button>
                  </form>
                </>
            }
          </div>
        </div>
      </div>
    </section>
  );



}


export default IsLoadingHOC(AddVehicle);

