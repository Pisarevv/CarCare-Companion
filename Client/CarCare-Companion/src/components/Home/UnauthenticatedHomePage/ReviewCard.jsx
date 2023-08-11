// FontAwesomeIcon component from the '@fortawesome/react-fontawesome' library 
// for rendering icon elements in React.
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'

// The specific star icon we'll be using to indicate rating.
import { faStar } from '@fortawesome/free-solid-svg-icons';

// CSS styles specific to this component.
import './ReviewCard.css';

const ReviewCard = ({reviewInfo}) => {

    // Destructuring the reviewInfo prop to extract relevant information.
    const {userFirstName, description, starsRating} = reviewInfo;

    // Create an array with a length equivalent to the stars rating.
    // For example, if starsRating is 5, starsCountArray will be [0, 1, 2, 3, 4].
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