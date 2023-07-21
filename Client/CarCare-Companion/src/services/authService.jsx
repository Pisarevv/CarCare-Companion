/**
 * authService 
 * ---------------------
 * The service responsible for the CRUD operations of the authentication.
 * ---------------------- 
**/

import * as api from "./api"

export async function login (email, password){
   return  api.post("/Login", { email, password });
}
    

export async function register(email, firstName, lastName, password, confirmPassword){
   return api.post("/Register", { email, firstName, lastName, password, confirmPassword});
}
     

// export const logout = () =>
//      api.post("/Logout");