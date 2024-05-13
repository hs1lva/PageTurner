import React, {useEffect, useState} from 'react';
import {useParams} from 'react-router-dom';
import ApiService from '../../services/ApiService';
import {url_server} from "../../contexto/url_servidor";
import InfoLivro from "./InfoLivro";
import ComentarioLivro from "./ComentariosLivro";
import Comentar from "./Comentar";
import Avaliar from "./Avaliar";

export default function Livro() {
    const {id} = useParams(); // Obter o ID do livro a partir do URL
    const [livro, setLivro] = useState(null);
    const [error, setError] = useState(null);
    const [refresh, setRefresh] = useState(false); // estado para garantir que faz um refresh aos comentarios quando for preciso

    const apiService = new ApiService(url_server());


    useEffect(() => {
        apiService.get('/api/Livro', id)
            .then(data => {
                setLivro(data);
            })
            .catch(error => {
                setError(error); // Armazenar o erro no estado
            });
    }, [refresh]); // o useeffect e atualizado quando o fresh for alterado


    if (error) {
        return (
            <div className="flex items-center justify-center h-screen text-2xl text-red-500">
                Livro não encontrado
            </div>
        );
    }

    const handleNewComment = () => {
        setRefresh(prevRefresh => !prevRefresh); //  funcao que atualiza o refresh, para passarmos para os comentarios
    };

    // Renderizar o componente
    if (!livro) {
        return <div>Carregando...</div>; // Renderizar algum tipo de spinner de carregamento enquanto os dados estão sendo buscados
    }
    return (
        <div className="m-10 mt-20 flex flex-col items-left">
            <InfoLivro livro={livro}>
                <Avaliar livroId={livro.livroId} media={livro.mediaAvaliacao} setRefresh={setRefresh}
                         refresh={refresh}/>
            </InfoLivro>
            <div className="flex-col justify-around w-1/3 space-y-8">

                <h1 className="text-2xl mt-10 font-bold mb-4">Comentários</h1>
                <Comentar livroId={livro.livroId} onNewComment={handleNewComment}/>
                {/*Ordena os Livros Por data*/}
                {livro.comentarios.sort((a, b) => new Date(b.dataComentario) - new Date(a.dataComentario)).map((comentario) => (
                    <ComentarioLivro key={comentario.comentarioId} comentario={comentario}/>
                ))}
            </div>
            <p> TODO : Alguma limitacao do numero de comentarios ou paginicao ?</p>
        </div>

    );
}