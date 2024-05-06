import React from "react";
import { Routes, Route } from "react-router-dom";
import "./App.css";

import Home from "./paginas/Home/index.js";
import Login from "./paginas/LogIn/Login.js";
import Header from "./componetes/Header/header.jsx";
import SobreNos from "./paginas/SobreNos/SobreNos.js";
import Registar from "./paginas/Registar/registar.js";
import Leitor from "./paginas/Leitor/Leitor.js";
function App() {
  return (
    <div className="App">
      <header>
        <Header />
      </header>
      <Routes >
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/sobreNos" element={<SobreNos />} />
        <Route path="/registar" element={<Registar />} />
        <Route path="/leitor" element={<Leitor />} />
      </Routes>
    </div>
  );
}

export default App;
