/**
 * useLocalStorage 
 * ---------------------
 * Custom hook that allows data to be stored
 * in the browser localStorage and be modified easily.
 * On initializing it recieves a default value that is 
 * associated with the given key.
 * ---------------------- 
 * 
 * States:
 * ----------------------
 * - value (object): This state allows any type of data to be stored.
 * ---------------------
 * 
 * Functions: 
 * - setLocalStorageValue - function that changes the stored value 
 *  in the browser localStorage and update the state of the hook
**/


import { useState } from "react";

const useLocalStorage = (key, defaultValue) => {

    const [value, setValue] = useState(() => {
        const storedData = localStorage.getItem(key);

        return storedData ? JSON.parse(storedData) : defaultValue;
    });

    const setLocalStorageValue = (value) => {
        localStorage.setItem(key, JSON.stringify(value));
        setValue(value);    
    };

    return [value, setLocalStorageValue]


}

export default useLocalStorage;