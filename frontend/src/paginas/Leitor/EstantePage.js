import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import ApiService from '../../services/ApiService';
import ListaEstantes from './ListaEstantes';
import { url_server } from "../../contexto/url_servidor";
import LoadingModal from "../../componetes/Loading/loading";

export default function EstantePage() {
    const { tipo, userId } = useParams();
    const [isLoading, setIsLoading] = useState(true);
    const [estantes, setEstantes] = useState([]);

    useEffect(() => {
        const apiService = new ApiService(url_server());

        let endpoint = '';
        switch (tipo) {
            case 'desejos':
                endpoint = `/api/Estante/tipoEstante/desejos/Utilizador/${userId}`;
                break;
            case 'troca':
                endpoint = `/api/Estante/tipoEstante/troca/Utilizador/${userId}`;
                break;
            case 'pessoal':
                endpoint = `/api/Estante/tipoEstante/pessoal/Utilizador/${userId}`;
                break;
            default:
                break;
        }

        apiService.getEstante(endpoint)
            .then(data => {
                setEstantes(data);
                setIsLoading(false);
            })
            .catch(error => {
                console.error(error);
                setIsLoading(false);
            });
    }, [tipo, userId]);

    if (isLoading) {
        return <LoadingModal showModal={isLoading} />;
    }

    return (
        <div className="p-4 sm:p-8">
            <ListaEstantes titulo={tipo.charAt(0).toUpperCase() + tipo.slice(1)} estantes={estantes} userId={userId} />
        </div>
    );
}
