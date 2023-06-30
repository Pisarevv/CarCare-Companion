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
          <Route path='/Vehicles' element={<Vehicles/>}/>
          <Route path='/Vehicle/Create' element={<AddVehicle/>}/>
          </Routes>
        </main> 
      </div>
    </AuthProvider>
  )
}

export default App;
