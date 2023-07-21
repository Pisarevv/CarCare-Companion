/**
 * tripServce 
 * ---------------------
 * The service responsible for the CRUD operations of the trips.
 * ---------------------- 
**/

import * as api from "./api"

export async function getAllUserTrips(){
    return  await api.get("/Trips");
}

export async function getLatestTrips(count){
    return await api.get(`/Trips/Last/${count}`)
}

export async function createTrip(startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId){
    return await api.post("/Trips", {startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId});
}

export async function editTrip(startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId, tripId){
    return await api.patch(`/Trips/Edit/${tripId}`, {startDestination,endDestination,mileageTravelled,usedFuel,fuelPrice,vehicleId});
}

export async function getTripDetails(tripId){
    return await api.get(`/Trips/Details/${tripId}`);
}

export async function getUserTripsCount(){
    return await api.get("/Trips/Count");
}

export async function getUserTripsCost(){
    return await api.get("/Trips/Cost");
}

export async function deleteTrip(tripId){
    return await api.delete(`/Trips/Delete/${tripId}`);
}
