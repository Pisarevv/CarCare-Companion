/**
 * Navigation Component
 * ---------------------
 * This component displays the website header. It has two states
 * - For guest (unauthorized users):
 *   The user can navigate trough Home page, Products page(where the store products are) 
 *   and Recycle page(where the user products listings are).
 *   When the user is not logged in a Register and Login links are available.
 * - For logged in users (authorized users):
 *   This user can navigate to all the links the guest can but there is no
 *   Login or Register links. Instead there is a Logout link.
 * ---------------------- 
 * 
 * Contexts:
 * ----------------
 * - useAuthContext
 *  In this component this context provides the "isAuthenticated" variable.
 *  The purpose of this variable is to determine if the user is authorized or not 
 *  and based on that to render different navigation links.
 *  
 *  - useCartContext
 *  In this component this context provides the "cart" array.
 *  The purpose of this array is to get its length and set the user cart products count on the icon of the cart.
 * -----------------
**/

import { NavLink } from 'react-router-dom'

import { useAuthContext } from '../../contexts/AuthContext';


import './Navigation.css'

const Navigation = () => {

    const { isAuthenticated } = useAuthContext();
 
    return (
        <header>
            {/* <img className="logo" src="/images/fhlogo.png" alt="image" /> */}
            <nav>
                <ul className="nav_links">
                    <li><NavLink to="/">Home</NavLink></li>
                    <li><NavLink to="/products/page/1">Forum</NavLink></li>
                    <li><NavLink to="/recycle/page/1">My vehicles</NavLink></li>
                </ul>
            </nav>
            <ul className="nav_links">
                {
                        isAuthenticated
                        ?
                        <><li><NavLink to="/logout">Logout</NavLink></li>
                           <li><NavLink to="/myListings">Profile</NavLink></li>                         
                        </>
                        :
                        <><li><NavLink to="/register">Register</NavLink></li>
                          <li><NavLink to="/login">Login</NavLink></li></>
                }

            </ul>

        </header>
    )
}

export default Navigation;