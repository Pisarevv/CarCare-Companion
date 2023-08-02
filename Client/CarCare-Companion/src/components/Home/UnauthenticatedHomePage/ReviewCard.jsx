import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faStar } from '@fortawesome/free-solid-svg-icons';
import './ReviewCard.css';


const ReviewCard = ({reviewInfo}) => {
    const {userFirstName, description, starsRating} = reviewInfo;
    const starsCountArray = [...Array(starsRating).keys()];
    return (
        <div className="review-card">
            <div className="name">{userFirstName}</div>
            <div className="stars">{starsCountArray.map(x => (<FontAwesomeIcon key = {x} icon={faStar}/>))}</div>
            <div className="description">{description}</div>
        </div>
    )
}

export default ReviewCard;