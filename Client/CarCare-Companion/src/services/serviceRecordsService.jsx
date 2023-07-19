/**
 * serviceRecordsService 
 * ---------------------
 * The service responsible for the CRUD operations of the record services.
 * ---------------------- 
**/

import * as api from "./api"


export async function createServiceRecord(title, description, mileage, cost, vehicleId, performedOn){
    var result = await api.post("/ServiceRecords", {title, description, mileage, cost, vehicleId, performedOn});
    return result;
}

export async function getAllServiceRecords(){
    var result = await api.get("/ServiceRecords");
    return result;
}

export async function getServiceRecordsDetails(recordId){
    var result = await api.get(`/ServiceRecords/Details/${recordId}`);
    return result;
}

export async function editServiceRecord(title, description, mileage, cost, vehicleId, performedOn, recordId){
    var result = await api.patch(`/ServiceRecords/Edit/${recordId}`, {title, description, mileage, cost, vehicleId, performedOn});
    return result;
}

export async function deleteServiceRecord(recordId){
    var result = await api.delete(`/ServiceRecords/Delete/${recordId}`);
    return result;
}
