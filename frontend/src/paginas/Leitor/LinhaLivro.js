import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import ApiService from '../../services/ApiService';
import { url_server } from "../../contexto/url_servidor";
import useAuthStore from "../../services/authService";

export default function LinhaLivro({ livro, estanteId, userId }) {
    const { getUser } = useAuthStore();
    const user = getUser();
    const [isOwner, setIsOwner] = useState(false);

    useEffect(() => {
        if (user && parseInt(user.user_id, 10) === parseInt(userId, 10)) {
            setIsOwner(true);
        }
    }, [user, userId]);

    const handleRemove = () => {
        const apiService = new ApiService(url_server());
        apiService.delete(`/api/Estante/${estanteId}`)
            .then(() => {
                // handle success, e.g., refresh the list of books
                window.location.reload();
            })
            .catch(error => {
                console.error(error);
            });
    };

    return (
        <li className="mb-1 flex justify-between items-center">
            <Link
                to={`/livro/${livro.livroId}`}
                className="text-blue-500 hover:underline truncate block"
                title={livro.tituloLivro} // Tooltip com título completo
            >
                {livro.tituloLivro}
            </Link>
            {isOwner && (
                <button
                    onClick={handleRemove}
                    className="ml-4 py-1 px-2 bg-red-500 text-white rounded hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-400 focus:ring-opacity-75"
                >
                    Remover
                </button>
            )}
        </li>
    );
}
