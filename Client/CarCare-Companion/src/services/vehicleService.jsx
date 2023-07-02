import * as api from './api';


export async function getFuelTypes(){
    var result = await api.get("/FuelTypes")
    return result;
}

export async function getVehicleTypes(){
    var result = await api.get("/VehicleTypes")
    return result;
}