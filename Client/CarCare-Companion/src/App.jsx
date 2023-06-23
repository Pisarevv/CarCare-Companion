import { Routes, Route } from 'react-router-dom';

import { AuthProvider } from './contexts/AuthContext';
import Navigation  from './components/Navigation/Navigation';

import './App.css'
import Register from './components/Register/Register';

function App() {
  return(
    <AuthProvider>
      <div className='App'>
        <Navigation/>
        <main>
          <Routes>
          <Route path='/register' element={<Register/>}/>
          </Routes>
        </main> 
      </div>
    </AuthProvider>
  )
}

export default App;
