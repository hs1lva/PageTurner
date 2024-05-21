import React from 'react';
import imagem from '../../../imagens/banner.jpg';
import '../home.css'

export default function Banner() {
    return (
        <div className="contentor">
            <div className="imagem">
                <img src={imagem} alt="logo" />
            </div>
            <div className="texto">
                <h1>PageTurner</h1>
                <p>Descubra e compartilhe novas histórias através de resumos e trocas de livros!</p>
                <p>Explore experiências de leitura personalizadas com recomendações feitas especialmente para você!</p>
                <p>Conecte-se com uma comunidade apaixonada de leitores e transforme páginas em vivências compartilhadas!</p>
            </div>
        </div>
    );
}
