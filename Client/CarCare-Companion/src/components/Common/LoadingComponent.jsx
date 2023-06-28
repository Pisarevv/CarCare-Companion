/**
 * LoadingComponent components
 * ---------------------
 * This components returns a loading animation with three dots.
 * ---------------------
**/

import { ThreeDots } from 'react-loader-spinner';

const LoadingComponent = () => {
    return (
        <ThreeDots
            height="80"
            width="80"
            radius="9"
            color="#4fa94d"
            ariaLabel="three-dots-loading"
            wrapperStyle={{
                position: "fixed",
                left: "47%",
            }}
             visible={true}
        />
    );
}

export default LoadingComponent;