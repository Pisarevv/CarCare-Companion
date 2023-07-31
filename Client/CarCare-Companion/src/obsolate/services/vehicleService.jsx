import * as api from './api';
import * as fileApi from './fileApi';

export async function getFuelTypes(){
    return await api.get("/Vehicles/FuelTypes");
}

export async function getVehicleTypes(){
    return await api.get("/Vehicles/Types");
}

export async function createVehicle(make,model,mileage,year,fuelTypeId, vehicleTypeId){
    return await axiosApi.post("/Vehicles",{make,model,mileage,year,fuelTypeId, vehicleTypeId});
}

export async function editVehicle(make,model,mileage,year,fuelTypeId, vehicleTypeId, vehicleId){
    return await api.patch(`/Vehicles/Edit/${vehicleId}`,{make,model,mileage,year,fuelTypeId, vehicleTypeId});
}

export async function uploadVehicleImage(formData,vehicleId){
    return await fileApi.post("/Vehicles/ImageUpload",formData,vehicleId);
}

export async function getUserVehicles(){
    return await axiosApi.get("/Vehicles");
}

export async function getVehicleDetails(vehicleId){
    return await api.get(`/Vehicles/Details/${vehicleId}`);
}

export async function getVehicleEditDetails(vehicleId){
    return await api.get(`/Vehicles/Edit/${vehicleId}`);
}

export async function deleteVehicle(vehicleId){
    return await api.delete(`/Vehicles/Delete/${vehicleId}`);
}
