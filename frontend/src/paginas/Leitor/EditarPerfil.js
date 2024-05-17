import React, { useState, useEffect } from 'react';
import ApiService from '../../services/ApiService';
import {url_server} from "../../contexto/url_servidor";
import useAuthStore from "../../services/authService";
import {useNavigate} from "react-router-dom";
import { toast } from "react-toastify";

export default function EditarPerfil() {
    const {getUser} = useAuthStore();
    const user = getUser();

    const apiService = new ApiService(url_server());
    const [userData, setUserData] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        apiService.get('/api/Utilizador', user.user_id)
            .then(data => {
                const userData = data.utilizador;
                // Format the date to "yyyy-MM-dd"
                userData.dataNascimento = formatDate(userData.dataNascimento);
                setUserData(userData);
            })
            .catch(error => console.error(error));
    }, []);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setUserData((prevState) => ({
            ...prevState,
            [name]: value,
        }));
    };

    const formatDate = (dateString) => {
        const date = new Date(dateString);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        apiService.put(`/api/Utilizador/${user.user_id}`, userData)
            .then(() => {
                toast.success('Perfil atualizado com sucesso!');
                navigate('/Leitor');
            })
            .catch(error => {
                toast.error('Erro ao atualizar perfil:', error);
                console.error('Erro ao atualizar perfil:', error);
            });
    };

    if (!userData) {
        return <div>Carregando...</div>;
    }

    return (
        <div className="flex justify-center">
            <section className="m-10 mt-10 flex flex-col items-left w-3/4 bg-white p-10 shadow-xl rounded-lg">
                <form onSubmit={handleSubmit} className="space-y-6">
                    <div className="rounded-md shadow-sm -space-y-px">
                        <label className="block text-sm font-medium text-gray-700">
                            Nome:
                            <input type="text" name="nome" value={userData.nome} onChange={handleInputChange} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                        </label>
                        <label className="block text-sm font-medium text-gray-700">
                            Apelido:
                            <input type="text" name="apelido" value={userData.apelido} onChange={handleInputChange} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                        </label>
                        <label className="block text-sm font-medium text-gray-700">
                            Data de Nascimento:
                            <input type="date" name="dataNascimento" value={userData.dataNascimento} onChange={handleInputChange} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                        </label>
                        <label className="block text-sm font-medium text-gray-700">
                            Avatar:
                            <input placeholder="url da imagem" type="text" name="fotoPerfil" value={userData.fotoPerfil} onChange={handleInputChange} className="mt-1 block w-full py-2 px-3 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm" />
                        </label>
                    </div>
                    <div>
                        <button type="submit" className="w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                            Atualizar Perfil
                        </button>
                    </div>
                </form>
            </section>
        </div>
    );
}