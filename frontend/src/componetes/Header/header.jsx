import { Link, useNavigate } from "react-router-dom";
import "./header.css";
import imagem from "../../imagens/banner.jpg";
import useAuthStore from "../../services/authService";
import BarraPesquisa from "./BarraPesquisa";

export default function Navbar() {
  const { isAuthenticated, logout } = useAuthStore();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
      <div className="nav-container">
        <nav className="nav-bar">
          <Link to={`/`}>
          <img src={imagem} width="100px" alt="" className="imagem" />
          </Link>          <div className="input-pesquisa">
            <BarraPesquisa />
          </div>
          <ul className="nav-items">
            <li>
              <Link to={`/SobreNos`} className="nav-item1 text-red-400">
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
                        to={`/`}
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
