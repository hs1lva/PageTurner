import { Link } from "react-router-dom";
import "./header.css";
import React from "react";

import imagem from "../../imagens/banner.jpg";
export default function Navbar() {
  
    const token = sessionStorage.getItem("PageTurnerUser");
    // const isAuthenticated = !!token;


  return (
    <div className="nav-container">
      <nav className="nav-bar">
        <img src={imagem} width="100px" alt="" className="imagem" />
        <input className="placeholder:italic placeholder:text-slate-400 block bg-white w-full border border-slate-300 rounded-md py-2 pl-9 pr-3 shadow-sm focus:outline-none focus:border-sky-500 focus:ring-sky-500 focus:ring-1 sm:text-sm" placeholder="Pesquise livros, autores, leitores e muito mais..." type="text" name="search"/>
        <ul className="nav-items">

          <li>
            <Link to={`/SobreNos`} className="nav-item1" >Sobre</Link>
          </li>
          {/* {isAuthenticated ? <p>Userlog</p> : <p>Sem Login</p>} */}
          <li>
            <Link to={`/login`} className="nav-item2">Login</Link>
          </li>
        </ul>
      </nav>
    </div>
  );
}
