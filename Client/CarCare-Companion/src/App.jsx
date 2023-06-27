import { Routes, Route } from 'react-router-dom';
import Navigation  from './components/Navigation/Navigation';

import { AuthProvider } from './contexts/AuthContext';

// import Home from './components/Home/Home';
import Register from './components/Register/Register';
import Login from './components/Login/Login';
import Logout from './components/Logout/Logout';

import './App.css'

function App() {
  return(
    <AuthProvider>
      <div className='App'>
        <Navigation/>
        <main>
          <Routes>
          {/* <Route path='/' element={<Home/>}/> */}
          <Route path='/register' element={<Register/>}/>
          <Route path='/login' element={<Login/>}/>
          <Route path='/logout' element={<Logout/>}/>
          </Routes>
        </main> 
      </div>
    </AuthProvider>
  )
}

export default App;
