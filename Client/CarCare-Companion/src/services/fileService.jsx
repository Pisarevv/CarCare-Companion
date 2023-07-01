/**
 * adSerrvice 
 * ---------------------
 * The service responsible for the CRUD operations of the ads.
 * ---------------------- 
**/

import * as api from "./api"

export async function uploadImage(image, contentType){
    var result = await api.post("/File", image, contentType);
    return result;
}
       

