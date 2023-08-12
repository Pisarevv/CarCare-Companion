// The Link component from react-router-dom to navigate between routes.
import { Link } from 'react-router-dom';

// Icons and utilities from the FontAwesome library.
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faStar } from '@fortawesome/free-solid-svg-icons';

// CSS styles specific to this component.
import './CarouselAdCard.css';

const CarouselAdCard = ({reviewInfo}) => {

    // Extract individual fields from the passed reviewInfo prop.
    const { userFirstName, description, starsRating, id } = reviewInfo;

    // Create an array of the given length (starsRating) to easily map and render the star icons.
    const starsCountArray = [...Array(starsRating).keys()];

    return (
        <div className="review-card">
            <div className="name">{userFirstName}</div>
            <div className="stars">
                {
                    starsCountArray.map(x => (<FontAwesomeIcon key={x} icon={faStar}/>))
                }
            </div>
            <div className="description">{description}</div>
            
            <div className="ad-card-buttons">
                <Link to={`/Administrator/CarouselAds/Edit/${id}`}>Edit</Link>
            </div>
        </div>
    )
}

export default CarouselAdCard;
