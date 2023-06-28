/**
 * IsLoadingHOC 
 * ---------------------
 * This function is used to wrap a component
 * that needs a loading animation.
 * It wraps the passed component and 
 * attaches the setLoadingState function.
 * ---------------------- 
 * 
 *  States:
 * ----------------------
 * - isLoading (bool): On this state depends
 *   if the loading animaton is rendered or not
 * 
 * Functions:
 * ---------------------
 * - setLoadingState 
 *  This function passed to and triggered by the wrapped components
 *  which changes the state of isLoading.
 * ---------------------
**/

import { useState } from "react"

import LoadingComponent from "./LoadingComponent";


const IsLoadingHOC = (WrappedComponent) => {
    function HOC(props) {
        const [isLoading, setLoading] = useState(true);

        const setLoadingState = (isComponentLoading) => {
            setLoading(isComponentLoading);
        };

        return (
            <>
                {isLoading && <LoadingComponent />}
                <WrappedComponent {...props} setLoading={setLoadingState} />
            </>
        );
    };

    return HOC;
}

export default IsLoadingHOC;