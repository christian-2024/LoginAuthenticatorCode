import React, { useState } from 'react';
import reactLogo from './assets/react.svg';
import viteLogo from '/vite.svg';

/// React router dom
import {  Route, Routes } from 'react-router-dom';

//css
import './App.css';

//Pages
import Dashboard from './jsx/pages/Dashboard';
import Login from './jsx/pages/Login';

function App() {
  return (
      <Routes>
        <Route path="/login" element={<Login /> }/>
        <Route path="/dashboard" element={<Dashboard /> }/>
      </Routes>
  )
}

export default App;
