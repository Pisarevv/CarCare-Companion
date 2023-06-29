import { NavLink, useNavigate } from 'react-router-dom';
import './Vehicles.css';

const Vehicles = () => {

    const navigate = useNavigate();

    return (
        <section className="vehicles">
            <div className="vehicle-container">
                <div class="create-button"> <NavLink to="/register">Add vehicle</NavLink></div>
                <div class="div2"> test2</div>
                <div class="div3"> test3</div>
                <div class="div4"> test4</div>
                <div class="div5"> test5</div>
            </div>
        </section>
    );
}

export default Vehicles;