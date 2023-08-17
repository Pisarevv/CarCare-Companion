// Importing necessary hooks and modules from React and React Router.
import { useEffect, useReducer, useState } from "react";
import { NavLink, useLocation } from "react-router-dom";

// Utilities
import { NotificationHandler } from "../../../utils/NotificationHandler";
import dataURLtoFile from "../../../utils/URLtoFileConverter";
import DecimalSeparatorFormatter from "../../../utils/DecimalSeparatorFormatter";

//Custom hook for deauthentication of the user
import useDeauthenticate from '../../../hooks/useDeauthenticate';

// Custom hook for authenticated API requests.
import useAxiosPrivate from "../../../hooks/useAxiosPrivate";

// Higher Order Component to show loading status.
import IsLoadingHOC from '../../Common/IsLoadingHoc';

// Reducer for managing the vehicle's state.
import userVehicleReducer from "../../../reducers/userVehicleReducer";

// Component-specific styling.
import './AddVehicle.css'


// Static validation messages and regex patterns.
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
  mileageRegex: new RegExp(/^\d+(?:[.,]\d+)?$/)
}

// Component for adding a new vehicle.
const AddVehicle = (props) => {

  // Provides access to the current location (route).
  const location = useLocation();

  //Use custom hook to get logUseOut function
  const logUserOut = useDeauthenticate();

  // Hooks for making authorized API requests.
  const axiosPrivate = useAxiosPrivate();
  const axiosPrivateFile = useAxiosPrivate();

  // Destructure the setLoading function from props, which controls the loading state.
  const { setLoading } = props;

  // State variables for the vehicle image and any associated error.
  const [vehicleImage, setVehicleImage] = useState(null);
  const [vehicleImageError, setVehicleImageError] = useState("");

  // State variables for storing available fuel types and vehicle types.
  const [fuelTypes, setFuelTypes] = useState([]);
  const [vehicleTypes, setVehicleTypes] = useState([]);

  // State to track whether the first step of adding a vehicle has been completed.
  const [stepOneFinished, setStepOneFinished] = useState(false);

  // Use the `userVehicleReducer` to manage the state of user's vehicle and related errors.
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
  });

  // useEffect hook that runs when the component mounts.
  useEffect(() => {
    // Flag to check if the component is still mounted when updating state.
    let isMounted = true;

    // Create an instance of AbortController to potentially cancel fetch requests.
    const controller = new AbortController();

    // Asynchronous function to fetch necessary details for the vehicle addition form.
    const getDetails = async () => {
      try {
        // Perform parallel API requests to fetch fuel types and vehicle types.
        const requests = [
          axiosPrivate.get('/Vehicles/FuelTypes', { signal: controller.signal }),
          axiosPrivate.get('/Vehicles/Types', { signal: controller.signal })
        ];

        Promise.all(requests)
          .then(responses => {
            const fuelTypesResult = responses[0].data;
            const vehicleTypesResult = responses[1].data;

            // Dispatch actions to set default selected fuel type and vehicle type.
            dispatch({ type: `SET_FUELTYPEID`, payload: fuelTypesResult[0].id });
            dispatch({ type: `SET_VEHICLETYPEID`, payload: vehicleTypesResult[0].id });

            // Update state variables with fetched data if the component is still mounted.
            if (isMounted) {
              setFuelTypes(fuelTypesResult);
              setVehicleTypes(vehicleTypesResult);
            }
          });
      } catch (err) {
        // Handle error and redirect to login in case of an error.
        if (err.response.status == 401) {
          // On error, show a notification and redirect to the login page.
          NotificationHandler("Something went wrong", "Plese log in again", 400);
          logUserOut(location);
        }
        const { title, status } = error.response.data;
        NotificationHandler("Warning", title, status);
      } finally {
        // Stop the loading state regardless of success or error.
        setLoading(false);
      }
    };

    // Invoke the `getDetails` function.
    getDetails();

    // Effect cleanup function.
    return () => {
      isMounted = false;
      controller.abort();
    }
  }, []);


  // Event handlers, validations, and utility functions.
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
      if (value.trim() === "") {
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
      if (value.trim() === "") {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.emptyInput });
        return false;
      }
      if (!ValidationRegexes.yearRegex.test(value)) {
        dispatch({ type: `SET_${target.toUpperCase()}_ERROR`, payload: ValidationErrors.inputNotNumber });
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
      let response = "";

      if (isMakeValid && isMileageValid &&
        isFuelTypeValid && isModelValid &&
        isVehicleTypeValid && isYearValid
      ) {
        const { make, model, year, fuelTypeId, vehicleTypeId } = userVehicle;
        const mileage = DecimalSeparatorFormatter(userVehicle.mileage);
        const requestResponse = await axiosPrivate.post("/Vehicles", { make, model, mileage, year, fuelTypeId, vehicleTypeId });
        vehicleId = requestResponse.data.id;
        response = requestResponse;
      }

      else {
        throw "Invalid input fields"
      }

      if (vehicleImage && !vehicleImageError) {

        const file = new FormData();
        file.append("file", dataURLtoFile(vehicleImage, "inputImage"));
        await axiosPrivateFile.post("/Vehicles/ImageUpload", file, {
          headers: {
            "VehicleId": `${vehicleId}`,
            "Content-Type": "multipart/form-data"
          }
        })
      }
      navigate('/MyVehicles');
      NotificationHandler("Success", "Sucessfully added vehicle!", response.status);
    }

    catch (error) {
      window.scrollTo({ top: 0, behavior: 'smooth' });
      const { title, status } = error.response.data;
      NotificationHandler("Warning", title, status);
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

