import React from "react";
import {Routes, Route} from "react-router-dom";
import Home from "./paginas/Home/index.js";
import Header from "./componetes/Header/header.jsx";
import SobreNos from "./paginas/SobreNos/SobreNos.js";
import Leitor from "./paginas/Leitor/Leitor.js";
import NewLogin from "./paginas/LogIn/NewLogin";
import { RequireLoginRoute, RequireGuestRoute } from "./services/ProtectedRoute";
import Livro from "./paginas/Livro/Livro";
import Pagina404 from "./paginas/Erros/Pagina404";
import PerfilLeitor from "./paginas/Leitor/PerfilLeitor";
import EditarPerfil from "./paginas/Leitor/EditarPerfil";
import {ToastContainer} from "react-toastify";

function App() {
    return (
        <div>
            <ToastContainer />
            <header>
                <Header/>
            </header>
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/login" element={<RequireGuestRoute Component={NewLogin}/>}/>
                <Route path="/sobreNos" element={<RequireLoginRoute Component={SobreNos}/>}/>
                <Route path="/editar-perfil" element={<RequireLoginRoute Component={EditarPerfil}/>}/>
                <Route path="/leitor" element={<RequireLoginRoute Component={Leitor}/>}/>
                <Route path="/leitor/:id" element={<RequireLoginRoute Component={PerfilLeitor}/>}/>
                <Route path="/livro/:id" element={<RequireLoginRoute Component={Livro}/>}/>
                <Route path="*" element={<Pagina404/>}/>
            </Routes>
        </div>
    );
}

export default App;
