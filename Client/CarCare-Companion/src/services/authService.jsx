/**
 * authService 
 * ---------------------
 * The service responsible for the CRUD operations of the authentication.
 * ---------------------- 
**/

import * as api from "./api"

export const login = (email,password) => 
     api.post("/Login", {email,password}) 

export const register = (email,firstName,lastName,password,confirmPassword) => 
     api.post("/Register", {email,firstName,lastName,password,confirmPassword})

export const logout = () => 
     api.post("/Logout");