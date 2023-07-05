import * as api from './api';
import * as fileApi from './fileApi';

export async function getFuelTypes(){
    var result = await api.get("/FuelTypes")
    return result;
}

export async function getVehicleTypes(){
    var result = await api.get("/VehicleTypes")
    return result;
}

export async function createVehicle(make,model,mileage,year,fuelTypeId, vehicleTypeId, ownerId){
    var result = await api.post("/VehicleCreate",{make,model,mileage,year,fuelTypeId, vehicleTypeId, ownerId})
    return result;
}

export async function uploadVehicleImage(formData,vehicleId){
    var result = await fileApi.post("/VehicleImageUpload",formData,vehicleId);
    return result;
}

export async function getUserVehicles(){
    var result = await api.get("/UserVehicles");
    return result;
}

export async function getVehicleDetails(vehicleId){
    var result = await api.get(`/VehicleDetails/${vehicleId}`);
    return result;
}