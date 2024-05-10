import React from "react";
import {Link, useNavigate} from "react-router-dom";
import "./header.css";
import imagem from "../../imagens/banner.jpg";
import useAuthStore from "../../services/authService";

export default function Navbar() {
    const {isAuthenticated, logout} = useAuthStore();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div className="nav-container">
            <nav className="nav-bar">
                <img src={imagem} width="100px" alt="" className="imagem"/>
                <input
                    className="placeholder:italic placeholder:text-slate-400 block bg-white w-full border border-slate-300 rounded-md py-2 pl-9 pr-3 shadow-sm focus:outline-none focus:border-sky-500 focus:ring-sky-500 focus:ring-1 sm:text-sm"
                    placeholder="Pesquise livros, autores, leitores e muito mais..." type="text" name="search"/>
                <ul className="nav-items">
                    <li>
                        <Link to={`/SobreNos`} className="nav-item1">Sobre</Link>
                    </li>

                    {isAuthenticated() ? (
                        <>
                            <li>
                                <Link to={`/Leitor`} className="nav-item1">Leitor</Link>
                            </li>
                            <li>
                                <Link to={`/logout`} className="nav-item2" onClick={handleLogout}>Logout</Link>
                            </li>
                        </>
                    ) : (
                        <li>
                            <Link to={`/login`} className="nav-item2">Login</Link>
                        </li>
                    )}
                </ul>
            </nav>
        </div>
    );
}