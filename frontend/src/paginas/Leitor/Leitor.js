import {url_server} from "../../contexto/url_servidor";
import {useEffect, useState} from "react";
import LoadingModal from "../../componetes/Loading/loading";
import 'react-toastify/dist/ReactToastify.css';
import InfoLeitor from "./InfoLeitor";
import ApiService from "../../services/ApiService";
import useAuthStore from "../../services/authService";
import Estante from "./Estante";


export default function Leitor() {
    const {getUser} = useAuthStore();
    const user = getUser();

    const apiService = new ApiService(url_server());
    const [isLoading, setIsLoading] = useState(true);
    const [userData, setUserData] = useState(null);

    useEffect(() => {
        apiService.get('/api/Utilizador', user.user_id)
            .then(data => {
                setUserData(data);
                setIsLoading(false);
            })
            .catch(error => console.error(error));
    }, []);

    if (!userData) {
        // ou um loading spinner
        return null;
    }


    // TODO: buscar as estantes dos utilizadores e os livros, para ja fica um array vazio para nao dar erro
    const livros = [];

    // TODO : Avatar deveria vir no request, temos esse campo ?
    const avatar = '';

    const numAvaliacoes = userData.avaliacoes.length;
    const numComentarios = userData.comentarios.length;

    if (isLoading) {
        return <LoadingModal showModal={isLoading}/>
    }

    return (
        <div className="flex justify-center">
            <section className="m-10 mt-10 flex flex-col items-left w-3/4 bg-white p-10 shadow-xl rounded-lg">
                <InfoLeitor nome={userData.nome} avatar={avatar} numAvaliacoes={numAvaliacoes}
                            numComentarios={numComentarios}/>
                <h1 className="text-2xl mt-10 font-bold mb-4">AS MINHAS ESTANTES</h1>
                <p className="text-xs">TODO: Encontrar a cor certa para as estantes</p>
                <div className="flex justify-around w-full space-x-8">
                    <Estante titulo="Meus Livros" livros={livros}/>
                    <Estante titulo="Quero Ler" livros={livros}/>
                    <Estante titulo="Quero Trocar" livros={livros}/>
                </div>
            </section>
        </div>
    );
}