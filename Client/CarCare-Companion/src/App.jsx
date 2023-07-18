import { Routes, Route } from 'react-router-dom';
import Navigation  from './components/Navigation/Navigation';

import { AuthProvider } from './contexts/AuthContext';

import Home from './components/Home/Home';
import Register from './components/Register/Register';
import Login from './components/Login/Login';
import Logout from './components/Logout/Logout';
import Vehicles from './components/Vehicles/Vehicles';
import AddVehicle from './components/Vehicles/AddVehicle/AddVehicle';


import VehicleDetails from './components/Vehicles/VehicleDetails/VehicleDetails';
import AddTrip from './components/Trips/AddTrip/AddTrip';
import Trips from './components/Trips/Trips';
import VehicleDelete from './components/Vehicles/DeleteVehicle/VehicleDelete';
import EditVehicle from './components/Vehicles/EditVehicle/EditVehicle';
import EditTrip from './components/Trips/EditTrip/EditTrip';
import DeleteTrip from './components/Trips/DeleteTrip/DeleteTrip';
import ServiceRecords from './components/ServiceRecords/ServiceRecords';

import './App.css'

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
          <Route path='/Trips/Edit/:id' element={<EditTrip/>}/>
          <Route path='/Trips/Delete/:id' element={<DeleteTrip/>}/>
          <Route path='/ServiceRecords' element={<ServiceRecords/>}/>
          </Routes>
        </main> 
      </div>
    </AuthProvider>
  )
}

export default App;
