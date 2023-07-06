import { NavLink } from 'react-router-dom';
import './Trips.css'

const Trips = () => {

    return (
        <section className="trips-section">
            <div className="trips-container">
                <div className="div1"><NavLink to="/Trips/Add">Add trip</NavLink> </div>
                <div className="div2"> </div>
                <div className="div3"> </div>
            </div>
        </section>
    )
}

export default Trips;