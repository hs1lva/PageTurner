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
  const [novoUtilizador, setNovoUtilizador] = useState({
    nome: "",
    apelido: "",
    username: "",
    email: "",
    password: "",
  });
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

  const registarNovoUtilizador = async (dadosUtilizador) => {
    try {
      const resposta = await fetch(`${url_server()}/api/Utilizador`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          "nome": dadosUtilizador.nome,
          "apelido": dadosUtilizador.apelido,
          "dataNascimento": new Date().toISOString(), // Para o utilizador atualizar
          "username": dadosUtilizador.username,
          "password": dadosUtilizador.password,
          "email": dadosUtilizador.email,
          "fotoPerfil": "",
          "dataRegisto": new Date().toISOString(), // Usar a data atual
          "ultimologin": new Date().toISOString(), // Usar a data atual
          "notficacaoPedidoTroca": true,
          "notficacaoAceiteTroca": true,
          "notficacaoCorrespondencia": true,
          "tipoUtilizadorId": 1,
          "estadoContaId": 0,
          "cidadeId": 1
        }),
      });

      if (!resposta.ok) {
        // Fazer paginas para erros e direcionar para lá
        if (resposta.status === 409) {
          setIsLoading(false);
          console.log("Utilizador já existe");
          toast.error("Error Notification !");
          alert("Utilizador já existe, altere o username ou email.");
          return;
        }
      }

      alert('Utilizador criado com sucesso, valide a sua conta com o email que lhe foi enviado.');
  
      // Se o utilizador for criado com sucesso, você pode redirecionar ou fazer outra ação aqui
      // Por exemplo, redirecionar para a página de login
      // navigate('/login');
    } catch (erro) {
      console.error(erro);
      // Tratar o erro de alguma forma, como exibir uma mensagem de erro para o utilizador
      toast.error('Erro ao registrar novo utilizador. Por favor, tente novamente.');
    }
  };
  
  const handleRegistoChange = (event) => {
    const { name, value } = event.target;
    setNovoUtilizador((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleRegistroSubmit = async (e) => {
    e.preventDefault();
  
    // Apanhar os valores dos campos do formulário
    const { nome, apelido, username, email, password } = novoUtilizador;
  
    // Criar o objeto dadosUtilizador com os dados do formulário
    const dadosUtilizador = {
      nome,
      apelido,
      username,
      email,
      password,
      // Adicione os outros campos do formulário, se houver
    };
  
    // Chamar a função para registar o novo utilizador
    await registarNovoUtilizador(dadosUtilizador);
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
                      placeholder="Username"
                      onChange={handlerUtilizadorChange}
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-lock"></i>
                    <input
                      id="password"
                      type="password"
                      placeholder="Password"
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
              <form onSubmit={handleRegistroSubmit}>
                <div className="input-boxes">
                  <div className="input-box">
                    <i className="fas fa-user"></i>
                    <input
                      type="text"
                      name="nome"
                      placeholder="Nome"
                      value={novoUtilizador.nome}
                      onChange={handleRegistoChange}
                      required
                    />
                    <input
                      type="text"
                      name="apelido"
                      placeholder="Apelido"
                      value={novoUtilizador.apelido}
                      onChange={handleRegistoChange}
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-user"></i>
                    <input
                      type="text"
                      name="username"
                      placeholder="Username"
                      value={novoUtilizador.username}
                      onChange={handleRegistoChange}
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-envelope"></i>
                    <input
                      type="text"
                      name="email"
                      placeholder="Email"
                      value={novoUtilizador.email}
                      onChange={handleRegistoChange}
                      required
                    />
                  </div>
                  <div className="input-box">
                    <i className="fas fa-lock"></i>
                    <input
                      type="password"
                      name="password"
                      placeholder="Password"
                      value={novoUtilizador.password}
                      onChange={handleRegistoChange}
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
