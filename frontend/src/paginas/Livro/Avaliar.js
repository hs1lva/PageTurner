import { useEffect, useState } from 'react';
import ApiService from "../../services/ApiService";
import { url_server } from "../../contexto/url_servidor";
import useAuthStore from "../../services/authService";
import ReactStars from "react-rating-stars-component";

export default function Avaliar({ livroId, media, setRefresh, refresh, data }) {
    const [avaliacao, setAvaliacao] = useState(null);
    const [avaliacaoExistente, setAvaliacaoExistente] = useState(null);
    const { getUser } = useAuthStore();
    const user = getUser();

    const apiService = new ApiService(url_server());

    const ratingChanged = (newRating) => {
        handleAvaliacaoChange(newRating);
    };
    useEffect(() => {
        const avaliacao = data.utilizador.avaliacoes.find(avaliacao => avaliacao.livroId === Number(livroId));
        if (avaliacao) {
            setAvaliacao(avaliacao.nota);
            setAvaliacaoExistente(avaliacao);
        } else {
            setAvaliacao(0);
        }
    }, [refresh, data.utilizador, livroId]);

    const handleAvaliacaoChange = (novaAvaliacao) => {
        setAvaliacao(novaAvaliacao);
        const data = {
            nota: novaAvaliacao,
            utilizadorId: user.user_id,
            livroId: livroId,
            dataAvaliacao: new Date().toISOString(),
        };

        if (avaliacaoExistente === null) {
            apiService.post('/api/AvaliacaoLivro', data)
                .then(response => {
                    setRefresh(prevRefresh => !prevRefresh); // Atualizar o estado no componente pai
                })
                .catch(error => {
                    console.error(error);
                });
        } else {
            data.avaliacaoId = avaliacaoExistente.avaliacaoId;
            apiService.put(`/api/AvaliacaoLivro/${avaliacaoExistente.avaliacaoId}`, data)
                .then(response => {
                    setRefresh(prevRefresh => !prevRefresh); // Atualizar o estado no componente pai
                })
                .catch(error => {
                    console.error(error);
                });
        }
    };

    return (
        <div className="flex items-center">
            <ReactStars
                key={avaliacao} // Força a recriação do componente quando o valor muda
                count={5}
                onChange={ratingChanged}
                size={24}
                activeColor="#ffd700"
                value={avaliacao}
            />
            {media !== null ? `(${media})` : "(Sem avaliações)"}
        </div>
    );
}
