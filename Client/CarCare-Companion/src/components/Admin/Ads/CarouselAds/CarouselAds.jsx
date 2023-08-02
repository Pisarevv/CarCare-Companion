import { useEffect, useState } from 'react';
import IsLoadingHOC from '../../../Common/IsLoadingHoc';
import './CarouselAds.css'
import { useNavigate } from 'react-router-dom';
import { NotificationHandler } from '../../../../utils/NotificationHandler';
import CarouselAdCard from './CarouselAdCard';
import useAxiosPrivate from '../../../../hooks/useAxiosPrivate';

const CarouselAds = (props) => {

    const [carouselAds, setCarouselAds] = useState([]);

    const { setLoading } = props;
    const navigate = useNavigate();

    const axiosPrivate = useAxiosPrivate();

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const loadCarouselAds = async () => {
           try {
            const response = await axiosPrivate.get("/Ads/CarouselAds",{
                signal : controller.signal
            });
            isMounted && setCarouselAds(carouselAds => response.data);
           } catch (error) {
            NotificationHandler(error);
           }
           finally{
            setLoading(false);
           }
        }

        loadCarouselAds();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    },[])

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


export default IsLoadingHOC(CarouselAds);