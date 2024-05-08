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
              <div className="title">Login</div>
              <form onSubmit={login}>
                <div className="input-boxes">
                  <div className="input-box">
                    <i className="fas fa-envelope"></i>
                    <input
                      type="text"
                      placeholder="Email"
                      onChange={handlerUtilizadorChange}
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-lock"></i>
                    <input
                      type="password"
                      placeholder="Password"
                      onChange={handlerPasswordChange}
                      required
                    />
                  </div>
                  <div className="button">
                    <input type="submit" value="Entrar" />
                  </div>
                  <div className="text sign-up-text">
                    Ainda não tem conta?{" "}
                    <label htmlFor="flip">Registe-se já</label>
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
