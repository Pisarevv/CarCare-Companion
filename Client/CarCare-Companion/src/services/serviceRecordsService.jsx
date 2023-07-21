/**
 * serviceRecordsService 
 * ---------------------
 * The service responsible for the CRUD operations of the record services.
 * ---------------------- 
**/

import * as api from "./api"


export async function createServiceRecord(title, description, mileage, cost, vehicleId, performedOn){
    return await api.post("/ServiceRecords", {title, description, mileage, cost, vehicleId, performedOn});
}

export async function getAllServiceRecords(){
    return await api.get("/ServiceRecords");
}

export async function getServiceRecordsDetails(recordId){
    return await api.get(`/ServiceRecords/Details/${recordId}`);
}

export async function editServiceRecord(title, description, mileage, cost, vehicleId, performedOn, recordId){
    return await api.patch(`/ServiceRecords/Edit/${recordId}`, {title, description, mileage, cost, vehicleId, performedOn});
}

export async function deleteServiceRecord(recordId){
    return await api.delete(`/ServiceRecords/Delete/${recordId}`);
}

