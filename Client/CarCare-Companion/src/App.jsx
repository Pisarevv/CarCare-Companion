import { Routes, Route } from 'react-router-dom';
import Navigation  from './components/Navigation/Navigation';

import { AuthProvider } from './contexts/AuthContext';

import Home from './components/Home/Home';
import Register from './components/Register/Register';
import Login from './components/Login/Login';
import Logout from './components/Logout/Logout';
import Vehicles from './components/Vehicles/Vehicles';
import AddVehicle from './components/AddVehicle/AddVehicle';

import './App.css'
import VehicleDetails from './components/VehicleDetails/VehicleDetails';
import AddTrip from './components/AddTrip/AddTrip';
import Trips from './components/Trips/Trips';
import VehicleDelete from './components/VehicleDetails/VehicleDelete';
import EditVehicle from './components/EditVehicle/EditVehicle';


function App() {
  return(
    <AuthProvider>
      <div className='App'>
        <Navigation/>
        <main>
          <Routes>
          <Route path='/' element={<Home/>}/>
          <Route path='/Register' element={<Register/>}/>
          <Route path='/Login' element={<Login/>}/>
          <Route path='/Logout' element={<Logout/>}/>
          <Route path='/MyVehicles' element={<Vehicles/>}/>
          <Route path='/Vehicle/Create' element={<AddVehicle/>}/>
          <Route path='/Vehicle/Edit/:id' element={<EditVehicle/>}/>
          <Route path='/Vehicle/Details/:id' element={<VehicleDetails/>}/>
          <Route path='/Vehicle/Delete/:id' element={<VehicleDelete/>}/>
          <Route path='/Trips' element={<Trips/>}/>
          <Route path='/Trips/Add' element={<AddTrip/>}/>
          </Routes>
        </main> 
      </div>
    </AuthProvider>
  )
}

export default App;
