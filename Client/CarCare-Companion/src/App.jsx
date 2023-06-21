import { Routes, Route } from 'react-router-dom';

import { AuthProvider } from './contexts/AuthContext';
import Navigation  from './components/Navigation/Navigation';

import './App.css'

function App() {
  return(
    <AuthProvider>
      <div className='App'>
        <Navigation/>

        
      </div>
    </AuthProvider>
  )
}

export default App
