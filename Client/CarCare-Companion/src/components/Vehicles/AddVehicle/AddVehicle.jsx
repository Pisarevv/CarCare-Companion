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
 * - vehicleImage (object): This object contains the vehicle image.
 * - vehicleImageError (object): Object containing an error based on the vehicleImage validity.
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
 *  Function that creates the vehicle if all of the properties are valid.
 *  If the request is successful it redirects to the user vehiclles collection.
 * -----------------
 * 
 * - ErrorHandler
 *  This is a custom function that handles errors thrown by the REST api  
 *  and based on the error shows the user notifications.
 * -----------------
**/

import { useEffect, useReducer, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";

import { NotificationHandler } from "../../../utils/NotificationHandler";

import useAxiosPrivate from "../../../hooks/useAxiosPrivate";
import IsLoadingHOC from '../../Common/IsLoadingHoc';

import userVehicleReducer from "../../../reducers/userVehicleReducer";
import dataURLtoFile from "../../../utils/URLtoFileConverter";

import './AddVehicle.css'



const ValidationErrors = {
  emptyInput: "This field cannot be empty",
  inputNotNumber: "This field accepts only valid numbers",
  yearNotValid: `The start year has to be past 1900 and before ${new Date().getFullYear()}`,
  invalidFileFormat: "The file format is not supported",
  fileSizeTooBig: "Please select a file under 2 MB"
}

const ValidationRegexes = {
  //Validates that the year is an integer 
  yearRegex: new RegExp(/^[0-9]*$/),
  mileageRegex: new RegExp(/^[0-9]*$/)
}


const AddVehicle = (props) => {

  const navigate = useNavigate();

  const axiosPrivate = useAxiosPrivate();
  const axiosPrivateFile = useAxiosPrivate();
  const { setLoading } = props;

  const [vehicleImage, setVehicleImage] = useState(null);
  const [vehicleImageError, setVehicleImageError] = useState("");

  const [fuelTypes, setFuelTypes] = useState([]);
  const [vehicleTypes, setVehicleTypes] = useState([]);
  
  const [stepOneFinished, setStepOneFinished] = useState(false);

  const [userVehicle, dispatch] = useReducer(userVehicleReducer, {
    make: "",
    mileage: "",
    fuelTypeId: "",
    model: "",
    vehicleTypeId: "",
    year: "",

    makeError: "",
    mileageError: "",
    fuelTypeIdError: "",
    modelError: "",
    vehicleTypeIdError: "",
    yearError: ""
  })

  useEffect(() => {
    let isMounted = true;
    const controller = new AbortController();

    const getDetails = async () => {
      try {
        const requests = [
          axiosPrivate.get('/Vehicles/FuelTypes', {
            signal: controller.signal
          }),
          axiosPrivate.get('/Vehicles/Types', {
            signal: controller.signal
          })
        ]
        Promise.all(requests)
        .then(responses => {

          const fuelTypesResult = responses[0].data;
          const vehicleTypesResult = responses[1].data;

          dispatch({ type: `SET_FUELTYPEID`, payload: fuelTypesResult[0].id })
          dispatch({ type: `SET_VEHICLETYPEID`, payload: vehicleTypesResult[0].id })
          if(isMounted){
            setFuelTypes(fuelTypes => fuelTypesResult);
            setVehicleTypes(vehicleTypes => vehicleTypesResult);
          }        
        })
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
      controller.abort();
    }
  }, [])

  console.log(fuelTypes);
  console.log(vehicleTypes);

  //Event handlers
  const handleImageUpload = (e) => {
    const file = e.target.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
      const vehicleImageInput = reader.result;
      setVehicleImage(vehicleImage => vehicleImageInput);
    };

    if (file) {
      if (!validateImageFile(file) || !validateImageFileSize(file)) {
        setVehicleImage(vehicleImage => null);
        return;
      }
      reader.readAsDataURL(file);
    }

  }

  const previousViewHandler = () => {
    setStepOneFinished(stepOneFinished => false);
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

  const validateSelectFields = (target, value) => {
    if (!value) {
      dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
      return false;
    }
    return true;
  }

  const validateNumberFields = (target, value) => {
    if (target === "mileage") {
      if (!ValidationRegexes.yearRegex.test(value) || value.trim() === "") {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
        return false;
      }
      if (!ValidationRegexes.mileageRegex.test(value) || value.trim() === "") {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
        return false;
      }
      return true;
    }
    if (target === "year") {
      if (!ValidationRegexes.yearRegex.test(value) || value.trim() === "") {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
        return false;
      }
      if (Number(value) < 1900 || Number(value) > Number(new Date().getFullYear())) {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.yearNotValid });
        return false;
      }
      return true;
    }
  }

  const validateImageFile = (file) => {
    const fileType = file.type;
    if (fileType.startsWith('image/')) {
      setVehicleImageError("");
      return true;
    } else {
      setVehicleImageError(ValidationErrors.invalidFileFormat);
      return false;
    }
  }

  const validateImageFileSize = (file) => {
    const fileSizeInMb = Math.round((file.size / 1024));
    if (fileSizeInMb < 2048) {
      setVehicleImageError("");
      return true;
    }
    setVehicleImageError(ValidationErrors.fileSizeTooBig);
    return false;
  }

  const onStepOneFinished = (e) => {
    e.preventDefault()
    let isMakeValid = validateTextFields("make", userVehicle.make);
    let isModelValid = validateTextFields("model", userVehicle.model);
    let isFuelTypeValid = validateSelectFields("fuelTypeId", userVehicle.fuelTypeId);
    let isMileageValid = validateNumberFields("mileage", userVehicle.mileage);
    let isVehicleTypeValid = validateSelectFields("vehicletypeId", userVehicle.vehicleTypeId);
    let isYearValid = validateNumberFields("year", userVehicle.year);

    if (isMakeValid && isMileageValid &&
      isFuelTypeValid && isModelValid &&
      isVehicleTypeValid && isYearValid
    ) {
      setStepOneFinished(stepOneFinished => true)
    }
    else {
      setStepOneFinished(stepOneFinished => false)
    }

  }

  const onVehicleAdd = async (e) => {
    e.preventDefault();
    try {
      let isMakeValid = validateTextFields("make", userVehicle.make);
      let isModelValid = validateTextFields("model", userVehicle.model);
      let isFuelTypeValid = validateSelectFields("fuelTypeId", userVehicle.fuelTypeId);
      let isMileageValid = validateNumberFields("mileage", userVehicle.mileage);
      let isVehicleTypeValid = validateSelectFields("vehicletypeId", userVehicle.vehicleTypeId);
      let isYearValid = validateNumberFields("year", userVehicle.year);

      let vehicleId = "";

      if (isMakeValid && isMileageValid &&
        isFuelTypeValid && isModelValid &&
        isVehicleTypeValid && isYearValid
      ) {
        const { make, model, mileage, year, fuelTypeId, vehicleTypeId } = userVehicle;
        const vehicleIdResponse = await axiosPrivate.post("/Vehicles",{ make, model, mileage, year, fuelTypeId, vehicleTypeId });
        vehicleId = vehicleIdResponse.data;
      }

      else {
        throw "Invalid input fields"
      }

      if (vehicleImage && !vehicleImageError) {

        const file = new FormData();
        file.append("file", dataURLtoFile(vehicleImage, "inputImage"));
        await axiosPrivateFile.post("/Vehicles/ImageUpload",file, {
          headers : {
            "VehicleId" : `${vehicleId}`,
            "Content-Type": "multipart/form-data" 
          }
        })
      }

      navigate('/MyVehicles');
    }

    catch (error) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      NotificationHandler(error)
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
                  <form onSubmit={onStepOneFinished}>
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
                        <select className="select-group" name="fueltypeId" onChange={onInputChange}>
                          {fuelTypes.map(ft => <option key={ft.id} value={ft.id}>{ft.name}</option>)}
                        </select>
                      </div>
                      {userVehicle.fuelTypeId && <p className="invalid-field" >{userVehicle.fuelTypeIdError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Mileage:</label>
                      <input className="form-control" type="text" placeholder="Mileage" name="mileage" value={userVehicle.mileage} onChange={onInputChange} />
                      {userVehicle.mileageError && <p className="invalid-field" >{userVehicle.mileageError}</p>}
                    </div>
                    <div className="input-group input-group-lg">
                      <label>Vehicle type:</label>
                      <div className="form-control select">
                        <select className="select-group" name="vehicleTypeId" onChange={onInputChange}>
                          {vehicleTypes.map(vt => <option key={vt.id} value={vt.id}>{vt.name}</option>)}
                        </select>
                      </div>
                      {userVehicle.vehicleTypeIdError && <p className="invalid-field">{userVehicle.vehicleTypeIdError}</p>}
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
                <div className="step-two-container">
                  <h2>Vehicle image</h2>
                  <form onSubmit={onVehicleAdd}>
                    <div className="input-group input-group-lg img-group">
                      <label>Image: (Optional)</label>
                      {vehicleImage && <img className="vehicle-image" src={vehicleImage} alt="Selected" />}
                      <label className="select-img-button" htmlFor="file" >Select Image</label>
                      <input className="form-control" type="file" accept="image/*" id="file" onChange={handleImageUpload} />
                      {vehicleImageError && <p className="invalid-field">{vehicleImageError}</p>}
                    </div>
                    <div className="action-controls">
                      <button className="float" onClick={previousViewHandler}>Previous</button>
                      <button type="submit" className="float">{vehicleImage ? "Finish" : "Skip and finish"}</button>
                    </div>
                  </form>
                </div>
            }
          </div>
        </div>
      </div>
    </section>
  );



}


export default IsLoadingHOC(AddVehicle);

