import {useState} from "react";
import useAuthStore from "../../services/authService";
import ApiService from "../../services/ApiService";
import {url_server} from "../../contexto/url_servidor";

export default function Comentar({livroId, onNewComment}) {
    const [comentario, setComentario] = useState();
    const [message, setMessage] = useState('');
    const {getUser} = useAuthStore();
    const user = getUser();

    const apiService = new ApiService(url_server());

    const handleComentarioChange = (event) => {
        setComentario(event.target.value);
    };

    const handleComentarioSubmit = (event) => {
        event.preventDefault();
        const data = {
            comentario,
            utilizadorId: user.user_id,
            livroId
        };

        console.log(data)
        apiService.post('/api/ComentarioLivro', data)
            .then(response => {
                onNewComment(); // atualiza a lista
                setComentario(''); // Limpar o campo de entrada
                setMessage('Comentário adicionado com sucesso!'); // Definir a mensagem de sucesso
            })
            .catch(error => {
                console.error(error);
            });
    };

    return (
        <form onSubmit={handleComentarioSubmit} className="mt-4">
            <textarea
                value={comentario}
                onChange={handleComentarioChange}
                className="w-full p-2 mb-4 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400 focus:border-transparent"
                rows="4"
                placeholder="Escreva seu comentário aqui..."
            />
            <button type="submit" className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-700">Comentar
            </button>
            {message && <p className="mt-4 text-green-500">{message}</p>}
        </form>
    );
}