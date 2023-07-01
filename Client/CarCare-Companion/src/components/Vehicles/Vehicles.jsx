import { NavLink, useNavigate } from 'react-router-dom';
import './Vehicles.css';

const Vehicles = () => {

    const navigate = useNavigate();

    return (
        <section className="vehicles">
            <div className="vehicle-container">
                <div className="create-button"> <NavLink to="/Vehicle/Create">Add vehicle</NavLink></div>
                <div className="div2"> test2</div>
                <div className="div3"> test3</div>
                <div className="div4"> test4</div>
                <div className="div5"> test5</div>
            </div>
        </section>
    );
}

export default Vehicles;