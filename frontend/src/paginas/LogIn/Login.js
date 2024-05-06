import { useState } from "react";
import { url_server } from "../../contexto/url_servidor";
import imagem from "../../imagens/BanerCriarUser.png";
import "./login.css";
import { useNavigate } from "react-router-dom";
import LoadingModal from "../../componetes/Loading/loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { useSelector ,useDispatch } from "react-redux";
import { setter } from "../../contexto/tokenSlice";
import { useToken } from "../../contexto/TokenContext";

export default function Login() {
  // Guardar token redux 
  const tokenGuardado = useSelector((state) => state.token.value); // Ver o token guardado
  const dispatch = useDispatch();
  // Guardar token por Contexto API
  const { updateToken } = useToken();

  const [isLoading, setIsLoading] = useState(false);
  const [utilizador, setUtilizador] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handlerUtilizadorChange = (event) => {
    setUtilizador(event.target.value);
    // console.log(utilizador);
  };

  const handlerPasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const login = async (e) => {
    e.preventDefault();

    console.log("Utilizador: " + utilizador);
    console.log("Password: " + password);
    try {
      // Falta garantir que não é possivel nada vazio
      setIsLoading(true);
      setTimeout(async () => {
      const resp = await fetch(url_server() + "/api/Utilizador/Login", {
        body: JSON.stringify({
          username: utilizador,
          password: password,
        }),
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        method: "POST",
      });

      if (!resp.ok) {
        // Fazer paginas para erros e direcionar para lá
        if (resp.status === 401) {
          setIsLoading(false);
          console.log("Credenciais inválidas");
          toast.error("Error Notification !");
          alert("Credenciais inválidas");
          return;
        }
        if (resp.status === 403) {
          setIsLoading(false);
          toast.error("User sem acesso");
          alert("User sem acesso");
          return;
        }
      }

      const data = await resp.json();
      if (data?.token?.result) {
        // guardamos o token na sessionStorage, apenas até encontrar forma melhor
        sessionStorage.setItem("PageTurnerUser", data.token.result); // Talvez seja melhor passar isto para memoria, ver com prof.
        setIsLoading(false);

        //tentativas para por isto mais seguro
        // Guardar pelo contexto API
        updateToken(data.token.result);

        // Guardar token pelo Redux
        dispatch(setter(data.token.result));

      }

      // alert("Login efetuado com sucesso");
      navigate("/");
      }, 5000);
    } catch (error) {
      console.log(error);
    }
  };

  if (isLoading) {
    return <LoadingModal showModal={isLoading} />;
  }

  return (
    <div className="registar">
      <div className="esquerda"></div>
      <div className="direita"></div>
      <div className="container">
        <input type="checkbox" id="flip" />
        <div className="cover">
          <div className="front">
            <img src={imagem} alt="" />
            <div className="text"></div>
          </div>
          <div className="back">
            <div className="text"></div>
          </div>
        </div>
        <div className="forms">
          <div className="form-content">
            <div className="login-form">
              <div className="title">Login</div>
              <form action="#">
                <div className="input-boxes">
                  <div className="input-box">
                    <i className="fas fa-envelope"></i>
                    <input
                      id="utilizador"
                      type="text"
                      placeholder="Insira o seu email"
                      onChange={handlerUtilizadorChange}
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-lock"></i>
                    <input
                      id="password"
                      type="password"
                      placeholder="Insira a sua password"
                      onChange={handlerPasswordChange}
                      required
                    />
                  </div>
                  {/* <div className="text"><a href="#">Forgot password?</a></div> */}
                  <div className="button input-box">
                    <input onClick={login} type="submit" value="Entrar" />
                  </div>
                  <div className="text sign-up-text">
                    Ainda não tem conta?{" "}
                    <label htmlFor="flip">Registe-se já</label>
                  </div>
                </div>
              </form>
            </div>
            <div className="signup-form">
              <div className="title">Registar</div>
              <form action="submit">
                <div className="input-boxes">
                  <div className="input-box">
                    <i className="fas fa-user"></i>
                    <input type="text" placeholder="Insira o nome" required />
                    <input type="text" placeholder="Insira aplelido" required />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-user"></i>
                    <input
                      type="text"
                      placeholder="Insira o Username"
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-envelope"></i>
                    <input type="text" placeholder="Insira o email" required />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-lock"></i>
                    <input
                      type="password"
                      placeholder="Insira a password"
                      required
                    />
                  </div>
                  <div className="button input-box">
                    <input type="submit" value="Registar" />
                  </div>
                  <div className="text sign-up-text">
                    Já tens conta? <label htmlFor="flip">Login</label>
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
