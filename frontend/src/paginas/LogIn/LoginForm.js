import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import useAuthStore from "../../services/authService";
import {url_server} from "../../contexto/url_servidor";

export default function LoginForm({onFlip}) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { setToken } = useAuthStore();

  const handlerUtilizadorChange = (event) => {
    setUsername(event.target.value);
  };

  const handlerPasswordChange = (event) => {
    setPassword(event.target.value);
  };

  const login = async (e) => {
    e.preventDefault();

    const response = await fetch(url_server() + "/api/Utilizador/Login", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        username,
        password,
      }),
    });

    if (response.ok) {
      const data = await response.json();
      setToken(data.token.result);
      navigate('/');
    } else {
      console.error('Login failed');
    }
  };

  return (
    <form onSubmit={login}>
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
        <div className="button input-box">
          <input type="submit" value="Entrar"/>
        </div>
        <div className="text sign-up-text">
          Ainda não tem conta?{" "}
          <label onClick={onFlip}>Registe-se já</label>
        </div>
      </div>
    </form>
  );
}