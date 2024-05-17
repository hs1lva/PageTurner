import {useNavigate, useParams} from 'react-router-dom';
import ApiService from '../../services/ApiService';
import { url_server } from "../../contexto/url_servidor";
import InfoLeitor from "../Leitor/InfoLeitor";
import { useState, useEffect } from 'react';
import LoadingModal from "../../componetes/Loading/loading";
import Estante from "../Leitor/Estante";

export default function PerfilLeitor() {
    const { id } = useParams(); // Obter o ID do usuário a partir do URL
    const apiService = new ApiService(url_server());
    const [isLoading, setIsLoading] = useState(true);
    const [userData, setUserData] = useState(null);
    const [estantesDesejos, setEstantesDesejos] = useState([]);
    const [estantesTroca, setEstantesTroca] = useState([]);
    const [estantesPessoal, setEstantesPessoal] = useState([]);
    const navigate = useNavigate();


    useEffect(() => {
        apiService.get('/api/Utilizador', id)
            .then(data => {
                if (data.utilizador) {
                    setUserData(data.utilizador);
                    setEstantesDesejos(data.estanteDesejos || []);
                    setEstantesTroca(data.estanteTroca || []);
                    setEstantesPessoal(data.estantePessoal || []);
                    setIsLoading(false);
                } else {
                    navigate('/404');
                }
            })
            .catch(error => {
                console.error(error);
                navigate('/404');
            });
    }, [id, navigate]);

    if (isLoading) {
        return <LoadingModal showModal={isLoading} />;
    }

    const numAvaliacoes = userData?.avaliacoes.length || 0;
    const numComentarios = userData?.comentarios.length || 0;

    return (
        <div className="flex justify-center p-4 sm:p-8">
            <section className="w-full max-w-4xl bg-white p-6 sm:p-10 shadow-xl rounded-lg">
                <InfoLeitor
                    nome={userData.nome}
                    avatar={userData.fotoPerfil}
                    numAvaliacoes={numAvaliacoes}
                    numComentarios={numComentarios}
                    isEditable={false}
                />
                <h1 className="text-2xl mt-10 font-bold mb-4">ESTANTES</h1>
                <p className="text-xs mb-4">TODO: Encontrar a cor certa para as estantes</p>
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
                    <Estante titulo="Desejos" estantes={estantesDesejos} userId={id} />
                    <Estante titulo="Troca" estantes={estantesTroca} userId={id} />
                    <Estante titulo="Pessoal" estantes={estantesPessoal} userId={id} />
                </div>
            </section>
        </div>
    );
}
