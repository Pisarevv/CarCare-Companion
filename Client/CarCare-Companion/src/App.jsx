import { Routes, Route } from 'react-router-dom';
import Navigation  from './components/Navigation/Navigation';

import { AuthProvider } from './contexts/AuthContext';

// import Home from './components/Home/Home';
import Register from './components/Register/Register';
import Login from './components/Login/Login';

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
          </Routes>
        </main> 
      </div>
    </AuthProvider>
  )
}

export default App;
