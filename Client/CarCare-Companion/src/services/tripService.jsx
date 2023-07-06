/**
 * tripServce 
 * ---------------------
 * The service responsible for the CRUD operations of the trips.
 * ---------------------- 
**/

import * as api from "./api"

export async function createTrip(startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId){
    var result = await api.post("/CreateTrip", {startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId});
    return result;
}