import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons';
import './CarouselAdCard.css';
import { Link } from 'react-router-dom';


const CarouselAdCard = ({reviewInfo}) => {
    const {userFirstName, description, starsRating, id} = reviewInfo;
    const starsCountArray = [...Array(starsRating).keys()];
    return (
        <div className="review-card">
            <div className="name">{userFirstName}</div>
            <div className="stars">{starsCountArray.map(x => (<FontAwesomeIcon key = {x} icon={faStar}/>))}</div>
            <div className="description">{description}</div>
            <div className="card-buttons"><Link to = {`/Administrator/CarouselAds/Edit/${id}`}>Edit</Link><Link>Delete</Link></div>
        </div>
    )
}

export default CarouselAdCard;