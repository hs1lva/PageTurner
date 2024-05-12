import imagem from "../../imagens/BanerCriarUser.png";
import "react-toastify/dist/ReactToastify.css";

export default function SobreNos() {
 


  return (
    <div className="registar">
      <div className="container">
        <div className="cover">
          <img src={imagem} alt="" />
        </div>
        <div className="forms">
          <div className="form-content">
            <div className="login-form">
              <div className="title">Sobre Nós...</div>
              <form >
                <div className="input-boxes">
                  <div className="text">
                    {" "}
                    PageTurner é uma plataforma online onde os apaixonados por
                    livros podem partilhar as suas experiências de leitura,
                    descobrir novos títulos e interagir com outros membros da
                    comunidade literária.{" "}
                  </div>
                  <br></br>
                  <div className="title"> O que temos de novo? </div>
                  <br></br>
                  <div className="text">
                    {" "}
                    <p>- Registo de Leitores;</p>
                    <p>- Biblioteca Pessoal;</p>
                    <p>- Avaliação de Livros;</p>
                    <p>- Comentários;</p>
                    <p>- Recomendações;</p>
                    <p>- Troca de Livros com outros Leitores;</p>{" "}
                  </div>
                  <div className="text mt-10">
                    Suporte:{" "}
                    <a
                      className="hover:underline text-blue-500"
                      href="mailto:pageturner@outlook.pt"
                    >
                      pageturner@outlook.pt
                    </a>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
