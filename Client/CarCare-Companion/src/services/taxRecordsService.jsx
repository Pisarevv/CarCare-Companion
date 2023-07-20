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

export async function createTaxRecord(title, description, validFrom, validTo , cost, vehicleId){
    var result = await api.post("/TaxRecords");
    return result;
}