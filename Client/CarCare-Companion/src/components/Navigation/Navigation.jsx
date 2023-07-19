/**
 * Navigation Component
 * ---------------------
 * This component displays the website header. It has two states
 * - For guest (unauthorized users):
 *   The user can navigate trough Home page
 *   When the user is not logged in a Register and Login links are available.
 * - For logged in users (authorized users):
 *   This user can navigate to the following links:
 *   - MyVehicles
 *   - Trips
 *   - ServiceRecords
 *   - TaxRecords
 *   - Profile
 *   There is no Login or Register links. Instead there is a Logout link.
 * ---------------------- 
 * 
 * Contexts:
 * ----------------
 * - useAuthContext
 *  In this component this context provides the "isAuthenticated" variable.
 *  The purpose of this variable is to determine if the user is authorized or not 
 *  and based on that to render different navigation links.
 * -----------------
**/

import { Link  } from 'react-router-dom'

import { useAuthContext } from '../../contexts/AuthContext';


import './Navigation.css'

const Navigation = () => {

    const { isAuthenticated } = useAuthContext();
 
    return (
        <header>
            {/* <img className="logo" src="/images/fhlogo.png" alt="image" /> */}
            <nav>
                <ul className="nav_links">
                    <li><Link to="/">Home</Link></li>
                   
                
                </ul>
            </nav>
            <ul className="nav_links">
                {
                        isAuthenticated
                        ?
                        <>
                         <li><Link to="/MyVehicles">My vehicles</Link></li>
                         <li><Link to="/Trips">Trips manager</Link></li>
                         <li><Link to="/ServiceRecords">Services manager</Link></li>
                         <li><Link to="/TaxRecords">Taxes manager</Link></li>
                         <li><Link to="/Profile">Profile</Link></li>      
                         <li><Link to="/Logout">Logout</Link></li>                   
                        </>
                        :
                        <>
                         <li><Link to="/Register">Register</Link></li>
                         <li><Link to="/Login">Login</Link></li>
                        </>
                }

            </ul>

        </header>
    )
}

export default Navigation;