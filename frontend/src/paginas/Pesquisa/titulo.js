import { useLocation } from "react-router-dom";
import "./titulo.css";

export default function Titulo() {
  const location = useLocation();
  const livros = location.state.livros;
console.log(livros)
  return (
    <div className="pesquisa-titulo">
      <div className="cards">

        {livros.map((livro) => (
          <div className="card" key={livro.key}>
            <img src={livro.capas[1]} alt={livro.title} /> {/* // Array 0 é a capa pequena, 1 é a média e 2 é a grande */}
            <h3>{livro.title}</h3>
            <p>{livro.author_name}</p>
          </div>
        ))}
        
      </div>
    </div>
  );
}
