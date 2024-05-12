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
import pesquisaTitulo from "./paginas/Pesquisa/titulo.js";

function App() {
    return (
        <div>
            <header>
                <Header/>
            </header>
            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/login" element={<RequireGuestRoute Component={NewLogin}/>}/>
                <Route path="/sobreNos" element={<RequireLoginRoute Component={SobreNos}/>}/>
                <Route path="/leitor" element={<RequireLoginRoute Component={Leitor}/>}/>
                <Route path="/livro/:id" element={<RequireLoginRoute Component={Livro}/>}/>
                <Route path="/pesquisa" element={<RequireLoginRoute Component={pesquisaTitulo}/>}/>
                <Route path="*" element={<Pagina404/>}/>
            </Routes>
        </div>
    );
}

export default App;
