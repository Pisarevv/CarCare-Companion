import * as api from './api';
import * as fileApi from './fileApi';

export async function getFuelTypes(){
    var result = await api.get("/Vehicles/FuelTypes")
    return result;
}

export async function getVehicleTypes(){
    var result = await api.get("/Vehicles/Types")
    return result;
}

export async function createVehicle(make,model,mileage,year,fuelTypeId, vehicleTypeId){
    var result = await api.post("/Vehicles",{make,model,mileage,year,fuelTypeId, vehicleTypeId})
    return result;
}

export async function uploadVehicleImage(formData,vehicleId){
    var result = await fileApi.post("/Vehicles/ImageUpload",formData,vehicleId);
    return result;
}

export async function getUserVehicles(){
    var result = await api.get("/Vehicles");
    return result;
}

export async function getVehicleDetails(vehicleId){
    var result = await api.get(`/Vehicles/Details/${vehicleId}`);
    return result;
}

export async function deleteVehicle(vehicleId){
    var result = await api.post(`/Vehicles/Delete/${vehicleId}`);
}