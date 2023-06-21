/**
 * authService 
 * ---------------------
 * The service responsible for the CRUD operations of the authentication.
 * ---------------------- 
**/

import * as api from "./api"

export const login = (email,password) => 
     api.post("/users/login", {email,password}) 

export const register =(email,password) => 
     api.post("/users/register", {email,password})

export const logout = () => 
     api.post("/users/logout");