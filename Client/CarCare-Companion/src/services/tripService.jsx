/**
 * tripServce 
 * ---------------------
 * The service responsible for the CRUD operations of the trips.
 * ---------------------- 
**/

import * as api from "./api"

export async function getAllUserTrips(){
    var result = await api.get("/Trips");
    return result;
}

export async function getLatestTrips(count){
    var result = await api.get(`/Trips/Last/${count}`)
    return result;
}

export async function createTrip(startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId){
    var result = await api.post("/Trips", {startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId});
    return result;
}

export async function getUserTripsCount(){
    var result = await api.get("/Trips/Count");
    return result;
}

export async function getUserTripsCost(){
    var result = await api.get("/Trips/Cost");
    return result;
}

