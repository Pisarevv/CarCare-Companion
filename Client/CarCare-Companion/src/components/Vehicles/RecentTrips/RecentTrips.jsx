import RecentTripCard from './RecentTripCard';
import './RecentTrips.css'

const RecentTrips = ({recentTrips}) => {

    return (
        <div className="recent-trips-container">
            <div className="trips-header">Recent trips</div>
            <div className="trips-list">
                {recentTrips && recentTrips.map(rt => <RecentTripCard key = {rt.id} details = {rt}/>)}
            </div>
        </div>
    )
}


export default RecentTrips;