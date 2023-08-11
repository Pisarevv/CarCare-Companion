// React's hooks for managing side-effects and state.
import { useEffect, useState } from 'react';

// Custom hook to make authenticated Axios requests.
import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

// Higher-order component (HOC) that wraps another component to show a loading state.
import IsLoadingHOC from '../../../Common/IsLoadingHoc'

// Component for handling notifications.
import { NotificationHandler } from '../../../../utils/NotificationHandler';

// Child component to display individual carousel ad cards.
import CarouselAdCard from './CarouselAdCard';

// CSS styles specific to this component.
import './CarouselAds.css'

const CarouselAds = (props) => {

    // Extract the setLoading function from the props, which controls the loading state.
    const { setLoading } = props;

    // Instantiate the useAxiosPrivate hook to get an instance of Axios with authentication headers.
    const axiosPrivate = useAxiosPrivate();

    // State to hold the list of carousel ads.
    const [carouselAds, setCarouselAds] = useState([]);

    // UseEffect hook to fetch the carousel ads when the component mounts.
    useEffect(() => {
        // Flag to ensure asynchronous tasks don't update state after the component is unmounted.
        let isMounted = true;

        // Create an AbortController to cancel the fetch request in case of unmounting.
        const controller = new AbortController();

        const loadCarouselAds = async () => {
            try {
                // Fetch the carousel ads.
                const response = await axiosPrivate.get("/Ads/CarouselAds", {
                    signal: controller.signal
                });

                // If the component is still mounted, update the state.
                isMounted && setCarouselAds(carouselAds => response.data);
            } catch (error) {
                // Handle any error that arises during the fetch operation using NotificationHandler.
                NotificationHandler(error);
            } finally {
                // Stop showing the loading state.
                setLoading(false);
            }
        }

        // Call the function to fetch the carousel ads.
        loadCarouselAds();

        // Cleanup function: run this if the component unmounts.
        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])

    return (
        <section className='carouselAds-container'>
            <div className="review-container">
                <div className="cards">
                    <div className="cards-inner">
                        <div className="cards-border"></div>
                        <div className="cards-row">
                            {carouselAds.map(ca => (
                                <div className="card-col" key={ca.id}>
                                    <CarouselAdCard reviewInfo={ca} />
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </section>
    )
}

// Wrap the CarouselAds component with IsLoadingHOC to show loading states when necessary.
export default IsLoadingHOC(CarouselAds);
