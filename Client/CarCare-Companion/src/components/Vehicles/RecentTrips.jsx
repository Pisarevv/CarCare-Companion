import './RecentTrips.css'

const RecentTrips = () => {

    return (
        <div className="recent-trips-container">
            <div className="trips-header">Recent trips</div>
            <div className="trips-list">
                <div className="trip"> 
                    <div>Los angeles - California</div>
                    <div>Milage : 6000 miles</div>
                </div>
                <div className="trip"> 
                    <div>Los angeles - California</div>
                    <div>Milage : 6000 miles</div>
                </div>
                <div className="trip"> 
                    <div>Los angeles - California</div>
                    <div>Milage : 6000 miles</div>
                </div>
            </div>
        </div>
    )
}


export default RecentTrips;