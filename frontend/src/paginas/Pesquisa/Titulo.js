import { useNavigate } from "react-router-dom";
import { useContext, useState, useEffect } from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import LazyLoad from 'react-lazyload';
import { SearchContext } from "../../contexto/SearchContext";
import "./titulo.css";
import ApiService from "../../services/ApiService";
import {url_server} from "../../contexto/url_servidor";

export default function Titulo() {
    const navigate = useNavigate();
    const { searchResults } = useContext(SearchContext);
    const [items, setItems] = useState(searchResults.slice(0, 20)); // Carregar os primeiros 20 livros
    const [hasMore, setHasMore] = useState(searchResults.length > 20);
    const [loadingBookKey, setLoadingBookKey] = useState(null);

    useEffect(() => {
        setItems(searchResults.slice(0, 20));
        setHasMore(searchResults.length > 20);
    }, [searchResults]);

    const fetchMoreData = () => {
        if (items.length >= searchResults.length) {
            setHasMore(false);
            return;
        }
        setTimeout(() => {
            setItems(prevItems => [...prevItems, ...searchResults.slice(prevItems.length, prevItems.length + 20)]);
        }, 500);
    };

    const handleAddBook = async (livro) => {
        if (loadingBookKey) return;

        setLoadingBookKey(livro.key);
        const apiService = new ApiService(url_server());
        try {
            const data = {
                tituloLivro: livro.title,
                anoPrimeiraPublicacao: livro.first_publish_year,
                keyOL: livro.key,
                capaSmall: livro.capaSmall ? livro.capaSmall : 'default',
                capaMedium: livro.capaMedium ? livro.capaMedium : 'default',
                capaLarge: livro.capaLarge ? livro.capaLarge : 'default',
                autorLivroNome: livro.author_name || [],
                generoLivroNome: livro.subject || []
            };
            const response = await apiService.postLivroOL(data);
            navigate(`/livro/${response.livroId}`);
        } catch (error) {
            console.error(error);
        } finally {
            setLoadingBookKey(null);
        }
    };


    return (
        <div className="pesquisa-titulo">
            <div className="container-pesquisa">
                <InfiniteScroll
                    dataLength={items.length}
                    next={fetchMoreData}
                    hasMore={hasMore}
                    loader={<h4>Loading...</h4>}
                    endMessage={<p>Não há mais livros para mostrar</p>}
                >
                    <div className="cards">
                        {items.map((livro) => (
                            <div className="card" key={livro.key} onClick={() => handleAddBook(livro)}>
                                <label className="label-card-titulo">
                                    <input className="checkbox-card-titulo" type="checkbox"/>
                                    {livro.capaLarge ? (
                                        <LazyLoad height={200} offset={100} once>
                                            <img src={livro.capaLarge} alt={livro.title}/>
                                        </LazyLoad>
                                    ) : (
                                        <div className="placeholder-capa">Capa não disponível</div>
                                    )}
                                </label>
                                <h3>{livro.title}</h3>
                                <p>{livro.author_name ? livro.author_name.join(", ") : "Autor desconhecido"}</p>
                            </div>
                        ))}
                    </div>
                </InfiniteScroll>
            </div>
        </div>
    );
}
