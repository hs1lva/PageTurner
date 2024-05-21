import React, {useState, useEffect} from 'react';
import ApiService from '../../services/ApiService';
import {url_server} from '../../contexto/url_servidor';
import useAuthStore from '../../services/authService';
import {toast} from 'react-toastify';
import Banner from "./components/Banner";
import TopBooks from './components/TopBooks';
import SuggestedBooks from './components/SuggestedBooks';
import './home.css';

export default function Home() {
    const {getUser} = useAuthStore();
    const user = getUser();

    const apiService = new ApiService(url_server());
    const [books, setBooks] = useState([]);
    const [topBooks, setTopBooks] = useState([]);

    useEffect(() => {
        if (user && user.user_id) {
            apiService.get(`/api/Livro/SugerirLivros`, user.user_id)
                .then(data => {
                    setBooks(data);
                })
                .catch(error => {
                    toast.error("Erro ao encontrar livros sugeridos");
                    console.error("Error fetching suggested books:", error);
                });
        }

        apiService.get(`/api/Livro/Top5Livros`)
            .then(data => {
              setTopBooks(data);
            })
            .catch(error => {
              console.error("Error fetching top rated books:", error);
            });
    }, [user]);

    return (
        <div className="Home">
            <Banner/>
            <TopBooks topBooks={topBooks}/>
            {user && <SuggestedBooks books={books} user={user}/>}
        </div>
    );
}
