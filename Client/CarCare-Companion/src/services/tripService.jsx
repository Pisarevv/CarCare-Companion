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

export async function createTrip(startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId){
    var result = await api.post("/Trips", {startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId});
    return result;
}

export async function getTripsCount(){
    var result = await api.get("/Trips/Count");
}

export async function getTripsCost(){
    var result = await api.get("/Trips/Cost");
}

