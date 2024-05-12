import { useLocation } from "react-router-dom"; // Esta importação é usada para conseguir aceder aos dados passados pelo useNavigate()
                                                // Passa um estado para a próxima rota. O estado pode ser qualquer coisa, até o endereço anterior. 
import "./titulo.css";

export default function Titulo() {
  const location = useLocation();
  const livros = location.state.livros;
  return (
    <div className="pesquisa-titulo">
      <div className="container-pesquisa">
        <div className="cards">
          {livros.map((livro) => (
            <div className="card" key={livro.key}>
              <img src={livro.capas[1]} alt={livro.title} />{" "}
              {/* // Array 0 é a capa pequena, 1 é a média e 2 é a grande */}
              <h3>{livro.title}</h3>
              <p>{livro.author_name}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
