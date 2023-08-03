import { Link } from 'react-router-dom';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons';

import './CarouselAdCard.css';

const CarouselAdCard = ({reviewInfo}) => {
    const {userFirstName, description, starsRating, id} = reviewInfo;
    const starsCountArray = [...Array(starsRating).keys()];
    return (
        <div className="review-card">
            <div className="name">{userFirstName}</div>
            <div className="stars">{starsCountArray.map(x => (<FontAwesomeIcon key = {x} icon={faStar}/>))}</div>
            <div className="description">{description}</div>
            <div className="ad-card-buttons"><Link to = {`/Administrator/CarouselAds/Edit/${id}`}>Edit</Link></div>
        </div>
    )
}

export default CarouselAdCard;