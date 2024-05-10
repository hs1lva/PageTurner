import React, { useState } from 'react';
import { toast } from 'react-toastify';
import {url_server} from "../../contexto/url_servidor";

export default function RegisterForm({onFlip}) {
  const [novoUtilizador, setNovoUtilizador] = useState({
    nome: "",
    apelido: "",
    username: "",
    email: "",
    password: "",
  });

  const handleRegistoChange = (event) => {
    const { name, value } = event.target;
    setNovoUtilizador((prevState) => ({
      ...prevState,
      [name]: value,
    }));
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

  return (
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
          <input type="submit" value="Registar"/>
        </div>
        <div className="text sign-up-text">
          Já tens conta? <label onClick={onFlip}>Login</label>
        </div>
      </div>
    </form>
  );
}