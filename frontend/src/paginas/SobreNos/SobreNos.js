import { useState } from "react";
import imagem from "../../imagens/BanerCriarUser.png";
import LoadingModal from "../../componetes/Loading/loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export default function Login() {
  const [isLoading, setIsLoading] = useState(false);
  const [utilizador, setUtilizador] = useState("");
  const [password, setPassword] = useState("");

  const handlerUtilizadorChange = (event) => {
    setUtilizador(event.target.value);
  };

  const handlerPasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const login = async (e) => {
    e.preventDefault();

    console.log("Utilizador: " + utilizador);
    console.log("Password: " + password);
    try {
      setIsLoading(true);
      setTimeout(() => {
        setIsLoading(false);
        // Simulando sucesso
        toast.success("Login efetuado com sucesso");
      }, 3000);
    } catch (error) {
      console.log(error);
    }
  };

  if (isLoading) {
    return <LoadingModal showModal={isLoading} />;
  }

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
              <form onSubmit={login}>
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
