import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import ApiService from '../../services/ApiService';
import { url_server } from "../../contexto/url_servidor";
import LoadingModal from "../../componetes/Loading/loading";
import useAuthStore from "../../services/authService";
import {toast} from "react-toastify";

export default function MatchesPage() {
    const location = useLocation();
    const { matches, user } = location.state; // Obter matches e user do estado passado pela navegação
    const apiService = new ApiService(url_server());
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);

    const handleIniciarTroca = async (match) => {
        setIsLoading(true);
        try {
            const response = await apiService.post(`/api/Troca/solicita-troca-direta/${user.user_id}/${match.queEuQuero.estanteId}`);
            if (response.trocaId) {
                navigate(`/troca/${response.trocaId}`); // Navegar para a página de detalhes da troca
           } else {
                toast.error('Erro ao enviar pedido de troca.');
           }
        } catch (error) {
            console.error(error);
            alert('Erro ao enviar pedido de troca.');
        } finally {
            setIsLoading(false);
        }
    };

    if (isLoading) {
        return <LoadingModal showModal={isLoading} />;
    }

    return (
        <div className="flex justify-center p-4 sm:p-8">
            <section className="w-full max-w-4xl bg-white p-6 sm:p-10 shadow-xl rounded-lg">
                <h1 className="text-2xl mt-10 font-bold mb-4">Matches</h1>
                <ul>
                    {matches.map((match, index) => (
                        <li key={index} className="mb-4 bg-gray-100 p-4 rounded-lg shadow-md">
                            <div className="flex justify-between items-center">
                                <div className="flex items-start space-x-4">
                                    <div>
                                        <h2 className="tracking-wide text-xl font-bold text-yellow-900">Você</h2>
                                        <p className="text-sm">O livro que você quer:</p>
                                        <div className="mt-2">
                                            <img className="object-cover h-20 w-20 rounded-lg" src={match.queEuQuero.livro.capaSmall} alt="Livro" />
                                            <p>{match.queEuQuero.livro.tituloLivro}</p>
                                        </div>
                                    </div>
                                    <div>
                                        <h2 className="tracking-wide text-xl font-bold text-yellow-900">{match.userComQueDeiMatch.nome}</h2>
                                        <p className="text-sm">O livro que ele quer:</p>
                                        <div className="mt-2">
                                            <img className="object-cover h-20 w-20 rounded-lg" src={match.queEuTenho.livro.capaSmall} alt="Livro" />
                                            <p>{match.queEuTenho.livro.tituloLivro}</p>
                                        </div>
                                    </div>
                                </div>
                                <button
                                    onClick={() => handleIniciarTroca(match)}
                                    className="bg-blue-500 text-white px-4 py-2 rounded-full hover:bg-blue-600 transition duration-200"
                                >
                                    Iniciar/Ver Troca
                                </button>
                            </div>
                        </li>
                    ))}
                </ul>
            </section>
        </div>
    );
}
