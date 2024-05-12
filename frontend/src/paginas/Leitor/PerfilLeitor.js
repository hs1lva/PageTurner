import { useParams } from 'react-router-dom';
import ApiService from '../../services/ApiService';
import {url_server} from "../../contexto/url_servidor";
import InfoLeitor from "../Leitor/InfoLeitor";
import { useState, useEffect } from 'react';
import LoadingModal from "../../componetes/Loading/loading";
import Estante from "./Estante";

export default function PerfilLeitor() {
    const { id } = useParams(); // Obter o ID do usuário a partir do URL
    const apiService = new ApiService(url_server());
    const [isLoading, setIsLoading] = useState(true);
    const [userData, setUserData] = useState(null);

    useEffect(() => {
        apiService.get('/api/Utilizador', id) // Buscar dados do usuário com base no ID da URL
            .then(data => {
                setUserData(data);
                setIsLoading(false);
            })
            .catch(error => console.error(error));
    }, []);

    // TODO: buscar as estantes dos utilizadores e os livros, para ja fica um array vazio para nao dar erro
    const livros = [];

    // TODO : Avatar deveria vir no request, temos esse campo ?
    const avatar = '';

    const numAvaliacoes = userData?.avaliacoes.length;
    const numComentarios = userData?.comentarios.length;

    if (isLoading) {
        return <LoadingModal showModal={isLoading}/>
    }

    return (
        <div className="flex justify-center">
            <section className="m-10 mt-10 flex flex-col items-left w-3/4 bg-white p-10 shadow-xl rounded-lg">
                <InfoLeitor nome={userData.nome} avatar={avatar} numAvaliacoes={numAvaliacoes}
                            numComentarios={numComentarios} isEditable={false} />
                <h1 className="text-2xl mt-10 font-bold mb-4">ESTANTES</h1>
                <p className="text-xs">TODO: Encontrar a cor certa para as estantes</p>
                <div className="flex justify-around w-full space-x-8">
                    <Estante titulo="Lidos" livros={livros}/>
                    <Estante titulo="Para Ler" livros={livros}/>
                    <Estante titulo="Para Trocar" livros={livros}/>
                </div>
            </section>
        </div>
    );
}