import React, {useState} from 'react';
import LoginForm from './LoginForm';
import "./login.css";

import RegisterForm from './RegisterForm';
import imagem from '../../imagens/BanerCriarUser.png';

const NewLogin = () => {
    const [flip, setFlip] = useState(false);

    const handleFlip = () => {
        setFlip(!flip);
    };

    return (
        <div className="registar">
            <div className="esquerda"></div>
            <div className="direita"></div>
            <div className="container">
                <input type="checkbox" id="flip" checked={flip} onChange={handleFlip}/>
                <div className="cover">
                    <div className="front">
                        <img src={imagem} alt=""/>
                        <div className="text"></div>
                    </div>
                    <div className="back">
                        <div className="text"></div>
                    </div>
                </div>
                <div className="forms">
                    <div className="form-content">
                        <div className="form-wrapper">
                            <LoginForm onFlip={handleFlip}/>
                        </div>
                        <div className="form-wrapper">
                            <RegisterForm onFlip={handleFlip}/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default NewLogin;