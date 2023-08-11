import { useState } from "react";

/**
 * useLocalStorage Hook
 *
 * A custom hook to manage state and local storage together.
 * When the state is set using the returned setter function, 
 * the value is also stored in local storage.
 * 
 * Parameters:
 * - key (string): The key under which data should be stored in local storage.
 * - defaultValue (any): A default value to be returned if there's no existing value in local storage.
 * 
 * Returns:
 * - An array with the current value from the state and the setter function.
 * 
 * Usage:
 * const [data, setData] = useLocalStorage("myDataKey", {});
 */
const useLocalStorage = (key, defaultValue) => {

    // Initialize state with value from local storage or default value.
    const [value, setValue] = useState(() => {
        const storedData = localStorage.getItem(key);
        return storedData ? JSON.parse(storedData) : defaultValue;
    });

    /**
     * setLocalStorageValue Function
     * 
     * Updates the local storage with the new value and also updates the state.
     * 
     * Parameters:
     * - value (any): The new value to be set in the local storage and state.
     */
    const setLocalStorageValue = (value) => {
        localStorage.setItem(key, JSON.stringify(value));
        setValue(value);    
    };

    return [value, setLocalStorageValue];
}

export default useLocalStorage;
