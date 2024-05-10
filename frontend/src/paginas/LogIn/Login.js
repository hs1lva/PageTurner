import { useState } from "react";
import { url_server } from "../../contexto/url_servidor";
import imagem from "../../imagens/BanerCriarUser.png";
import "./login.css";
import { useNavigate } from "react-router-dom";
import LoadingModal from "../../componetes/Loading/loading";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

import { setter } from "../../contexto/tokenSlice";

import useAuthStore from "../../services/authService";

export default function Login() {
  const { setToken, setUser } = useAuthStore();

  // Guardar token redux 
  //const tokenGuardado = useSelector((state) => state.token.value); // Ver o token guardado
  //const dispatch = useDispatch();
  // Guardar token por Contexto API
 // const { updateToken } = useToken();

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
    setIsLoading(true);
    try {
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

      console.log("response", resp);

      if (!resp.ok) {
        console.error('Erro na resposta da API:', resp.status);
        if (resp.status === 401) {
          toast.error("Error Notification !");
          return;
        }
        if (resp.status === 403) {
          toast.error("User sem acesso");
          return;
        }
      }

      const data = await resp.json();
      if (data?.token?.result) {
        sessionStorage.setItem("PageTurnerUser", data.token.result);
        setToken(data.token.result);
        navigate("/");
      }
    } catch (error) {
      console.log(error);
    } finally {
      setIsLoading(false);
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

  <div className="front">
    <img src={imagem} alt="" />
    <div className="text"></div>
  </div>}
