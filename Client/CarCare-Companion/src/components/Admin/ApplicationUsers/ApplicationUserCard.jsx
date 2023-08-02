import { Link } from 'react-router-dom';
import './ApplicationUserCard.css';

const ApplicationUserCard = ({details}) => {
    const {userId,username} = details;

    return (
        <div className="application-user-card">
            <div>{userId}</div>
            <div>{username}</div>
            <div><Link to = {`/Administrator/ApplicationUsers/${userId}`}>Details</Link></div>
        </div>
    )
}

export default ApplicationUserCard;

