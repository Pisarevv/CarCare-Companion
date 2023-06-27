import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import { BrowserRouter } from 'react-router-dom'
import { ReactNotifications } from 'react-notifications-component'
import 'react-notifications-component/dist/theme.css'

ReactDOM.createRoot(document.getElementById('root')).render(
    <BrowserRouter>
    <ReactNotifications/>
    <App />
    </BrowserRouter>
)
