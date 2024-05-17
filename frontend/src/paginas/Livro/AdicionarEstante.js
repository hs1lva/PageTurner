import React, { useState, useEffect } from 'react';
import { FaBook, FaPlus } from 'react-icons/fa';
import ApiService from "../../services/ApiService";
import { url_server } from "../../contexto/url_servidor";
import { toast } from 'react-toastify';

export default function AdicionarEstante({ tipoEstanteNome, data, livroId, nomeEstante }) {
    const [isInEstante, setIsInEstante] = useState(false);

    useEffect(() => {
        // Verificar se o livro já está na estante do utilizador
        let estantes = [];
        if (tipoEstanteNome === 'desejos') {
            estantes = data.estanteDesejos || [];
        } else if (tipoEstanteNome === 'pessoal') {
            estantes = data.estantePessoal || [];
        }

        const estanteExistente = estantes.some(estante => {
            return estante.livro.livroId === livroId;
        });

        if (estanteExistente) {
            setIsInEstante(true);
        }
    }, [data, tipoEstanteNome, livroId]);

    const handleAdicionarEstante = () => {
        const apiService = new ApiService(url_server());
        const tipoEstanteId = tipoEstanteNome === 'desejos' ? 1 : tipoEstanteNome === 'pessoal' ? 3 : null;

        if (tipoEstanteId === null) {
            toast.error("Tipo de estante inválido.");
            return;
        }

        const postData = {
            tipoEstanteId,
            utilizadorId: data.utilizador.utilizadorID,
            livroId
        };

        apiService.post('/api/Estante', postData)
            .then(response => {
                if (response) {
                    setIsInEstante(true);
                    toast.success("Livro adicionado com sucesso!");
                }
            })
            .catch(error => {
                if (error.response && error.response.status === 400) {
                    setIsInEstante(true);
                    toast.error(error.response.data);
                } else {
                    console.error(error);
                    toast.error("Erro ao adicionar o livro.");
                }
            });
    };

    return (
        <button
            onClick={handleAdicionarEstante}
            className="flex items-center space-x-2 py-2 px-4 bg-gray-200 text-gray-800 rounded hover:bg-gray-300 disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={isInEstante}
        >
            <FaPlus />
            <FaBook />
            <span>{nomeEstante}</span>
        </button>
    );
}
