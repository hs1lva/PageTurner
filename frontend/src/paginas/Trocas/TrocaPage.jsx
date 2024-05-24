import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import ApiService from '../../services/ApiService';
import { url_server } from "../../contexto/url_servidor";
import LoadingModal from "../../componetes/Loading/loading";
import useAuthStore from "../../services/authService";
import { toast } from "react-toastify";
import { FaExchangeAlt } from 'react-icons/fa';

export default function TrocaPage() {
    const { id } = useParams(); // Obter o ID da troca a partir do URL
    const apiService = new ApiService(url_server());
    const [isLoading, setIsLoading] = useState(true);
    const [troca, setTroca] = useState(null);
    const { getUser } = useAuthStore();
    const user = getUser();
    const navigate = useNavigate();

    useEffect(() => {
        apiService.get(`/api/Troca/${id}`)
            .then(data => {
                if (data) {
                    // Verificar se o user logado está na troca
                    if (parseInt(data.estante2.utilizador.utilizadorID) !== parseInt(user.user_id) && parseInt(data.estante.utilizador.utilizadorID) !== parseInt(user.user_id)) {
                        toast.error("Você não tem permissão para aceder a esta troca.");
                        navigate("/"); // Redirecionar para a página inicial
                    } else {
                        setTroca(data);
                    }
                    setIsLoading(false);
                } else {
                    // Lidar com o caso em que a troca não é encontrada
                    setIsLoading(false);
                }
            })
            .catch(error => {
                console.error(error);
                setIsLoading(false);
            });
    }, [id, user.user_id, navigate]);

    const handleAceitarTroca = async () => {
        setIsLoading(true);
        try {
            const response = await apiService.put(`/api/Troca/aceita-troca/${id}`);
            if (response.estadoTroca.descricaoEstadoTroca === "Aceite") {
                toast.info('Troca aceite com sucesso!');
                setTroca(response); // Atualiza a troca com a nova informação
            } else {
                toast.error('Erro ao aceitar troca.');
            }
        } catch (error) {
            console.error(error);
            toast.error('Erro ao aceitar troca.');
        } finally {
            setIsLoading(false);
        }
    };

    const handleCancelarTroca = async () => {
        setIsLoading(true);
        try {
            const response = await apiService.put(`/api/Troca/rejeita-troca/${id}`);
            if (response) {
                toast.info('Troca cancelada com sucesso!');
                navigate("/"); // Navegar para a página inicia
            } else {
                alert('Erro ao cancelar troca.');
            }
        } catch (error) {
            console.error(error);
            alert('Erro ao cancelar troca.');
        } finally {
            setIsLoading(false);
        }
    };

    if (isLoading) {
        return <LoadingModal showModal={isLoading} />;
    }

    if (!troca) {
        return <div className="flex justify-center p-4 sm:p-8">Troca não encontrada</div>;
    }

    const isUserEstante2 = parseInt(troca.estante2.utilizador.utilizadorID) === parseInt(user.user_id);
    const estadoTroca = troca.estadoTroca.descricaoEstadoTroca;

    const renderCards = () => {
        const userEstante = isUserEstante2 ? troca.estante2 : troca.estante;
        const otherEstante = isUserEstante2 ? troca.estante : troca.estante2;

        return (
            <>
                <div className="bg-gray-100 p-4 rounded-lg shadow-md flex flex-col items-center">
                    <h2 className="tracking-wide text-xl font-bold text-yellow-900">Você</h2>
                    <p className="text-sm">Troca:</p>
                    <div className="mt-2 flex flex-col items-center">
                        <img className="object-cover h-20 w-20 rounded-lg" src={userEstante.livro.capaSmall} alt="Livro" />
                        <p>{userEstante.livro.tituloLivro}</p>
                    </div>
                </div>
                <div className="flex justify-center items-center">
                    <FaExchangeAlt className="text-3xl text-gray-500" />
                </div>
                <div className="bg-gray-100 p-4 rounded-lg shadow-md flex flex-col items-center">
                    <h2 className="tracking-wide text-xl font-bold text-yellow-900">{otherEstante.utilizador.nome}</h2>
                    <p className="text-sm">Troca:</p>
                    <div className="mt-2 flex flex-col items-center">
                        <img className="object-cover h-20 w-20 rounded-lg" src={otherEstante.livro.capaSmall} alt="Livro" />
                        <p>{otherEstante.livro.tituloLivro}</p>
                    </div>
                </div>
            </>
        );
    };

    return (
        <div className="flex justify-center p-4 sm:p-8">
            <section className="w-full max-w-4xl bg-white p-6 sm:p-10 shadow-xl rounded-lg">
                <h1 className="text-2xl mt-10 font-bold mb-4">Detalhes da Troca</h1>
                <div className="flex items-center justify-center mb-6 space-x-4">
                    {renderCards()}
                </div>
                {estadoTroca === 'Pendente' ? (
                    <div className="flex flex-col sm:flex-row justify-between items-center space-y-4 sm:space-y-0">
                        {isUserEstante2 ? (
                            <button
                                onClick={handleAceitarTroca}
                                className="bg-blue-500 text-white px-4 py-2 rounded-full hover:bg-blue-600 transition duration-200"
                            >
                                Aceitar Troca
                            </button>
                        ) : (
                            <p className="text-sm">A aguardar que {troca.estante2.utilizador.nome} aceite a troca.</p>
                        )}
                        <button
                            onClick={handleCancelarTroca}
                            className="bg-red-500 text-white px-4 py-2 rounded-full hover:bg-red-600 transition duration-200"
                        >
                            Cancelar Troca
                        </button>
                    </div>
                ) : (
                    <div className="flex flex-col items-center">
                        <p className="text-sm">Status da troca: {estadoTroca}</p>
                        {troca.dataAceiteTroca && (
                            <p className="text-sm">Data de conclusão: {new Date(troca.dataAceiteTroca).toLocaleDateString()}</p>
                        )}
                    </div>
                )}
            </section>
        </div>
    );
}
