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

    const [isLoading, setIsLoading] = useState(true);
    const [userData, setUserData] = useState(null);
    const [estantesDesejos, setEstantesDesejos] = useState([]);
    const [estantesTroca, setEstantesTroca] = useState([]);
    const [estantesPessoal, setEstantesPessoal] = useState([]);


    useEffect(() => {
        const apiService = new ApiService(url_server()); // Movi esta linha para dentro do useEffect para nao dar aviso ESLint

        apiService.get('/api/Utilizador', user.user_id)
            .then(data => {
                setUserData(data.utilizador);

                setEstantesDesejos(data.estanteDesejos || []);
                setEstantesTroca(data.estanteTroca || []);
                setEstantesPessoal(data.estantePessoal || []);
                setIsLoading(false);
            })
            .catch(error => console.error(error));

    }, [user, setIsLoading, setUserData]);

    if (!userData) {
        // ou um loading spinner
        return null;
    }


    const numAvaliacoes = userData.avaliacoes.length;
    const numComentarios = userData.comentarios.length;

    if (isLoading) {
        return <LoadingModal showModal={isLoading}/>
    }


    return (
        <div className="flex justify-center p-4 sm:p-8">
            <section className="w-full max-w-4xl bg-white p-6 sm:p-10 shadow-xl rounded-lg">
                <InfoLeitor nome={userData.nome} avatar={userData.fotoPerfil} numAvaliacoes={numAvaliacoes}
                            numComentarios={numComentarios} isEditable="true"/>
                <h1 className="text-2xl mt-10 font-bold mb-4">AS MINHAS ESTANTES</h1>
                <p className="text-xs mb-4">TODO: Encontrar a cor certa para as estantes</p>
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
                    <Estante titulo="Desejos" estantes={estantesDesejos} userId={user.user_id}/>
                    <Estante titulo="Troca" estantes={estantesTroca} userId={user.user_id}/>
                    <Estante titulo="Pessoal" estantes={estantesPessoal} userId={user.user_id}/>
                </div>
            </section>
        </div>
    );
}