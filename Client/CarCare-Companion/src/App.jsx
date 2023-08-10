import { Routes, Route } from 'react-router-dom';
import Navigation from './components/Navigation/Navigation';

import { AuthProvider } from './contexts/AuthContext';

import AuthenticatedHomePage from './components/Home/AuthenticatedHomePage/AuthenticatedHomePage';
import UnauthenticatedHomePage from './components/Home/UnauthenticatedHomePage/UnauthenticatedHomePage';
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
import AddServiceRecord from './components/ServiceRecords/AddServiceRecord/AddServiceRecord';
import EditServiceRecord from './components/ServiceRecords/EditServiceRecord/EditServiceRecord';
import DeleteServiceRecord from './components/ServiceRecords/DeleteServiceRecord/DeleteServiceRecord';
import TaxRecords from './components/TaxRecords/TaxRecords';
import AddTaxRecord from './components/TaxRecords/AddTaxRecord/AddTaxRecord';
import EditTaxRecord from './components/TaxRecords/EditTaxRecord/EditTaxRecord';
import DeleteTaxRecord from './components/TaxRecords/DeleteTaxRecord/DeleteTaxRecord';
import PrivateGuard from './components/Common/PrivateGuard';
import PersistLogin from './components/Common/PersistLogin';
import ApplicationUsers from './components/Admin/ApplicationUsers/ApplicationUsers';
import ApplicationUserDetails from './components/Admin/ApplicationUsers/ApplicationUserDetails/ApplicationUserDetails';
import AdminDashboard from './components/Admin/AdminDashboard/AdminDashboard';

import './App.css'
import CarouselAds from './components/Admin/Ads/CarouselAds/CarouselAds';
import EditCarouselAd from './components/Admin/Ads/CarouselAds/EditCarouselAd/EditCarouselAd';



function App() {
  return (
    <AuthProvider>
      <div className='App'>
        <Navigation />
        <main>

          <Routes>
            <Route element={<PersistLogin />}>
              <Route path='/' element={<UnauthenticatedHomePage />} />
              <Route path='/Logout' element={<Logout />} />


              <Route element={<PrivateGuard allowedRoles={["User", "Administrator"]} />}>
                <Route path='/Home' element={<AuthenticatedHomePage />} />
                <Route path='/MyVehicles' element={<Vehicles />} />
                <Route path='/Vehicle/Create' element={<AddVehicle />} />
                <Route path='/Vehicle/Edit/:id' element={<EditVehicle />} />
                <Route path='/Vehicle/Details/:id' element={<VehicleDetails />} />
                <Route path='/Vehicle/Delete/:id' element={<VehicleDelete />} />
                <Route path='/Trips' element={<Trips />} />
                <Route path='/Trips/Add' element={<AddTrip />} />
                <Route path='/Trips/Edit/:id' element={<EditTrip />} />
                <Route path='/Trips/Delete/:id' element={<DeleteTrip />} />
                <Route path='/ServiceRecords' element={<ServiceRecords />} />
                <Route path='/ServiceRecords/Add' element={<AddServiceRecord />} />
                <Route path='/ServiceRecords/Edit/:id' element={<EditServiceRecord />} />
                <Route path='/ServiceRecords/Delete/:id' element={<DeleteServiceRecord />} />
                <Route path='/TaxRecords' element={<TaxRecords />} />
                <Route path='/TaxRecords/Add' element={<AddTaxRecord />} />
                <Route path='/TaxRecords/Edit/:id' element={<EditTaxRecord />} />
                <Route path='/TaxRecords/Delete/:id' element={<DeleteTaxRecord />} />
              </Route>

              <Route element={<PrivateGuard allowedRoles={["Administrator"]} />}>
                <Route path='/AdministratorDashboard/' element={<AdminDashboard />} />
                <Route path='/Administrator/ApplicationUsers' element={<ApplicationUsers />} />
                <Route path='/Administrator/ApplicationUsers/:id' element={<ApplicationUserDetails />} />
                <Route path='/Administrator/CarouselAds' element={<CarouselAds />} />
                <Route path='/Administrator/CarouselAds/Edit/:id' element={<EditCarouselAd />} />

              </Route>
            </Route>
            <Route path='/Register' element={<Register />} />
            <Route path='/Login' element={<Login />} />
          </Routes>
        </main>
      </div>
    </AuthProvider>
  )
}

export default App;
