import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import ApiService from '../../services/ApiService';
import { url_server } from "../../contexto/url_servidor";
import InfoLivro from "./InfoLivro";
import ComentariosLivro from "./ComentariosLivro";
import Comentar from "./Comentar";
import Avaliar from "./Avaliar";
import AdicionarEstante from "./AdicionarEstante";
import useAuthStore from "../../services/authService";
import 'react-toastify/dist/ReactToastify.css';
import InfiniteScroll from 'react-infinite-scroll-component';

export default function Livro() {
    const { id } = useParams(); // Obter o ID do livro a partir do URL
    const [livro, setLivro] = useState(null);
    const [utilizador, setUtilizador] = useState(null); // Estado para o utilizador
    const [comentarios, setComentarios] = useState([]); // Estado para os comentários
    const [hasMore, setHasMore] = useState(true); // para o infinite scroll
    const [error, setError] = useState(null);
    const [refresh, setRefresh] = useState(false); // estado para garantir que faz um refresh aos comentarios quando for preciso
    const { getUser } = useAuthStore();
    const currentUser = getUser();

    const apiService = new ApiService(url_server());

    useEffect(() => {
        // Fazer a chamada para obter o livro
        apiService.get('/api/Livro', id)
            .then(data => {
                setLivro(data);
                setComentarios(data.comentarios.sort((a, b) => new Date(b.dataComentario) - new Date(a.dataComentario)).slice(0, 10)); // Carregar os primeiros 10 comentários ordenados por data
                setHasMore(data.comentarios.length > 10);
            })
            .catch(error => {
                setError(error); // Armazenar o erro no estado
            });

        // Fazer a chamada para obter o utilizador
        apiService.get('/api/Utilizador', currentUser.user_id)
            .then(data => {
                setUtilizador(data);
            })
            .catch(error => {
                console.error(error);
            });
    }, [refresh, id, currentUser.user_id]); // o useEffect é atualizado quando o refresh, id, ou currentUser.user_id for alterado

    const fetchMoreComentarios = () => {
        if (comentarios.length >= livro.comentarios.length) {
            setHasMore(false);
            return;
        }
        setTimeout(() => {
            setComentarios(prevComentarios => [
                ...prevComentarios,
                ...livro.comentarios.slice(prevComentarios.length, prevComentarios.length + 10)
            ]);
        }, 500);
    };

    if (error) {
        return (
            <div className="flex items-center justify-center h-screen text-2xl text-red-500">
                Livro não encontrado
            </div>
        );
    }

    const handleRefresh = () => {
        setRefresh(prevRefresh => !prevRefresh); //  função que atualiza o refresh, para passarmos para os comentários
    };

    // Função para verificar se o livro está na estante pessoal
    const naEstantePessoal = () => {
        return utilizador.estantePessoal.some(estante => estante.livro.livroId === livro.livroId);
    };

    // Renderizar o componente
    if (!livro || !utilizador) {
        return <div>Carregando...</div>; // Renderizar algum tipo de spinner de carregamento enquanto os dados estão sendo buscados
    }

    return (
        <div className="m-10 mt-20 flex flex-col items-left">
            <div className="flex space-x-4 mb-4">
            </div>
            <InfoLivro livro={livro}>
                <AdicionarEstante onAddEstante={handleRefresh} tipoEstanteNome="desejos" data={utilizador} livroId={livro.livroId} nomeEstante="Desejos" /> {/* Estante de Desejos */}
                <AdicionarEstante onAddEstante={handleRefresh} tipoEstanteNome="pessoal" data={utilizador} livroId={livro.livroId} nomeEstante="Pessoal" /> {/* Estante Pessoal */}
                {naEstantePessoal() && (
                    <AdicionarEstante onAddEstante={handleRefresh} tipoEstanteNome="troca" data={utilizador} livroId={livro.livroId} nomeEstante="Troca" />
                    )}
                <Avaliar livroId={livro.livroId} media={livro.mediaAvaliacao} setRefresh={setRefresh} refresh={refresh} data={utilizador} />
            </InfoLivro>
            <div className="flex-col justify-around w-96 space-y-8">
                <h1 className="text-2xl mt-10 font-bold mb-4">Comentários ({livro.comentarios.length})</h1>
                <Comentar livroId={livro.livroId} onNewComment={handleRefresh} />
                <InfiniteScroll
                    dataLength={comentarios.length}
                    next={fetchMoreComentarios}
                    hasMore={hasMore}
                    loader={<h4>Carregando...</h4>}
                    endMessage={<p>Não há mais comentários para mostrar</p>}
                >
                    {comentarios.sort((a, b) => new Date(b.dataComentario) - new Date(a.dataComentario)).map((comentario) => (
                        <ComentariosLivro key={comentario.comentarioId} comentario={comentario} utilizador={comentario.utilizador} />
                    ))}
                </InfiniteScroll>
            </div>
        </div>
    );
}
