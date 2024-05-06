import "./home.css";
import imagem from "../../imagens/banner.jpg";

export default function Home() {
  return (
    <div className="Home">
      <div className="contentor">
        <div className="imagem">
          <img src={imagem} alt="logo" />
        </div>
        <div className="separador"></div>
        <div className="noticias">
          <h1>
            PageTurner
          </h1>
          <p>NotDescubra e compartilhe novas histórias através de resumos e trocas
            de livros!</p>
          <p>
            Explore experiências de leitura personalizadas com recomendações
            feitas especialmente para você!
          </p>

          <p>
            Conecte-se com uma comunidade apaixonada de leitores e transforme
            páginas em vivências compartilhadas!
          </p>
        <footer className="footer-noticias">
        Nos encontre também em nossas redes sociais!
        </footer>
        </div>
      </div>
    </div>
  );
}
