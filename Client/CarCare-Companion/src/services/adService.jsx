/**
 * adSerrvice 
 * ---------------------
 * The service responsible for the CRUD operations of the ads.
 * ---------------------- 
**/

import * as api from "./api"

export async function getAllCarouselAds(){
    return await api.get("/Home");
}
       

