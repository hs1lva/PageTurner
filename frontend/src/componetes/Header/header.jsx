import { Link, useNavigate } from "react-router-dom";
import "./header.css";
import imagem from "../../imagens/banner.jpg";
import useAuthStore from "../../services/authService";
import ApiService from "../../services/ApiService";
import { url_server } from "../../contexto/url_servidor";
import { useState } from "react";
import LoadingModal from "../Loading/loading";

export default function Navbar() {
  const { isAuthenticated, logout } = useAuthStore();
  const navigate = useNavigate();
  const apiService = new ApiService(url_server());
  const [isLoading, setIsLoading] = useState(false);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setIsLoading(true);
    apiService
      .get(
        "/api/Livro/PesquisarLivro/OpenLibrary/Titulo",
        e.target.search.value
      )
      .then((data) => {
        navigate("/pesquisa", { state: { livros: data.docs } });
      })
      .catch((error) => console.error(error));
    setIsLoading(false);
  };

  if (isLoading) {
    return <LoadingModal showModal={isLoading} />;
  }

  return (
    <div className="nav-container">
      <nav className="nav-bar">
        <img src={imagem} width="100px" alt="" className="imagem" />
        <div className="w-full">
          <form className="w-full" action="submit" onSubmit={handleSubmit}>
            <input
              className="placeholder:italic placeholder:text-slate-400 block bg-white w-full border border-slate-300 rounded-md py-2 pl-9 pr-3 shadow-sm focus:outline-none focus:border-sky-500 focus:ring-sky-500 focus:ring-1 sm:text-sm"
              placeholder="Pesquise livros, autores, leitores e muito mais..."
              type="text"
              name="search"
            />
          </form>
        </div>
        <ul className="nav-items">
          <li>
            <Link to={`/SobreNos`} className="nav-item1">
              Sobre
            </Link>
          </li>

          {isAuthenticated() ? (
            <>
              <li>
                <Link to={`/Leitor`} className="nav-item1">
                  Leitor
                </Link>
              </li>
              <li>
                <Link
                  to={`/logout`}
                  className="nav-item2"
                  onClick={handleLogout}
                >
                  Logout
                </Link>
              </li>
            </>
          ) : (
            <li>
              <Link to={`/login`} className="nav-item2">
                Login
              </Link>
            </li>
          )}
        </ul>
      </nav>
    </div>
  );
}
