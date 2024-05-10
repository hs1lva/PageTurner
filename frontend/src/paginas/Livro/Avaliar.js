import {useEffect, useState} from 'react';
import ApiService from "../../services/ApiService";
import {url_server} from "../../contexto/url_servidor";
import useAuthStore from "../../services/authService";
import ReactStars from "react-rating-stars-component";

export default function Avaliar({ livroId, media, setRefresh, refresh }) {
    const [avaliacao, setAvaliacao] = useState(null);
    const [avaliacaoExistente, setAvaliacaoExistente] = useState(null);
    const { getUser } = useAuthStore();
    const user = getUser();

    const apiService = new ApiService(url_server());

    const ratingChanged = (newRating) => {
        handleAvaliacaoChange(newRating);
    };

    useEffect(() => {
        apiService.get('/api/Utilizador', user.user_id)
            .then(data => {
                const avaliacao = data.avaliacoes.find(avaliacao => avaliacao.livroId === Number(livroId));
                if (avaliacao) {
                    setAvaliacao(avaliacao.nota);
                    setAvaliacaoExistente(avaliacao);
                }
            })
            .catch(error => {
                console.error(error);
            });
    }, [refresh]);

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
            {avaliacao !== null && (
                <ReactStars
                    count={5}
                    onChange={ratingChanged}
                    size={24}
                    activeColor="#ffd700"
                    value={avaliacao}
                />
            )}
            ({media})
        </div>
    );
}