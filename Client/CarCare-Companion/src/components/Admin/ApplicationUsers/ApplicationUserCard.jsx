// React Router's Link component for declarative navigation.
import { Link } from 'react-router-dom';

// CSS styles specific to this component.
import './ApplicationUserCard.css';

const ApplicationUserCard = ({ details }) => {

    // Extracting relevant details from the passed props.
    const { userId, username } = details;

    // Rendering a card representation of an application user.
    return (
        <div className="application-user-card">
            <div>{userId}</div>
            <div>{username}</div>
            <div>
                {/* Link to view more details of the application user. */}
                <Link to={`/Administrator/ApplicationUsers/${userId}`}>Details</Link>
            </div>
        </div>
    )
}

export default ApplicationUserCard;
