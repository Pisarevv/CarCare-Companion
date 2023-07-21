/**
 * taxRecordsService 
 * ---------------------
 * The service responsible for the CRUD operations of the tax records.
 * ---------------------- 
**/

import * as api from './api'


export async function getAllTaxRecords(){
    var result = await api.get("/TaxRecords");
    return result;
}

export async function createTaxRecord(title, description, validFrom, validTo, cost, vehicleId){
    var result = await api.post("/TaxRecords",{title, description, validFrom, validTo,  cost, vehicleId});
    return result;
}

export async function getTaxRecordDetails(recordId){
    return await api.get(`/TaxRecords/Details/${recordId}`);
   
}

export async function editTaxRecord(title, description, validFrom, validTo, cost, vehicleId, recordId){
    return await api.patch(`/TaxRecords/Edit/${recordId}`);
}