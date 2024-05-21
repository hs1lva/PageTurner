import React, { useState, useEffect, useRef } from "react";
import ApiService from "../../services/ApiService";
import { url_server } from "../../contexto/url_servidor";
import useAuthStore from "../../services/authService";
import { toast } from "react-toastify";
import "./home.css";
import imagem from "../../imagens/banner.jpg";
import { DEFAULT_BOOK_COVER } from "../../services/constants";

export default function Home() {
  const { getUser } = useAuthStore();
  const user = getUser();

  const apiService = new ApiService(url_server());
  const [books, setBooks] = useState([]);
  const [topBooks, setTopBooks] = useState([]);
  const carouselRef = useRef(null);

  useEffect(() => {
    if (user && user.user_id) {
      apiService.get(`/api/Livro/SugerirLivros`, user.user_id)
        .then(data => {
          setBooks(data);
          if (carouselRef.current) {
            carouselRef.current.style.setProperty('--num-books', data.length);
          }
        })
        .catch(error => {
          toast.error("Erro ao encontrar livros sugeridos");
          console.error("Error fetching suggested books:", error);
        });
    }

    apiService.get(`/api/Livro/TopLivros`) // Obter os livros mais avaliados (tem que ser feito no backend)
      .then(data => {
        setTopBooks(data);
      })
      .catch(error => {
        // toast.error("Erro ao encontrar os livros mais avaliados");
        console.error("Error fetching top rated books:", error);
      });
  }, [user]);

  return (
    <div className="Home">
      <div className="contentor">
        <div className="imagem">
          <img src={imagem} alt="logo" />
        </div>
        <div className="texto">
          <h1>PageTurner</h1>
          <p>Descubra e compartilhe novas histórias através de resumos e trocas de livros!</p>
          <p>Explore experiências de leitura personalizadas com recomendações feitas especialmente para você!</p>
          <p>Conecte-se com uma comunidade apaixonada de leitores e transforme páginas em vivências compartilhadas!</p>
        </div>
      </div>

      <div className="top-books-section">
        <h2>As melhores avaliações da semana:</h2>
        <div className="top-books">
          {topBooks.map((book) => (
            <div key={book.id} className="top-book">
              <img
                src={book.capaSmall || DEFAULT_BOOK_COVER}
                alt={book.titulo}
                className="top-book-cover"
              />
              <p className="top-book-title">{book.titulo}</p>
            </div>
          ))}
        </div>
      </div>

      {user && (
        <div className="footer">
          <footer className="footer-noticias">
            <div className="suggestion-title">
              A nossa sugestão de livros baseada na sua leitura:
            </div>
            {books.length > 0 ? (
              <div className="book-carousel">
                <div className="book-carousel-wrapper" ref={carouselRef}>
                  {books.map((livro) => (
                    <div key={livro.livroId} className="book">
                      <img
                        src={livro.capaSmall || DEFAULT_BOOK_COVER} // Usar a URL da capa do livro small openlibrary
                        alt={livro.tituloLivro}
                        className="book-cover"
                      />
                      <p className="book-title">{livro.tituloLivro}</p>
                    </div>
                  ))}
                </div>
              </div>
            ) : (
              <p>Ainda não há sugestões de livros a apresentar.</p>
            )}
          </footer>
        </div>
      )}
    </div>
  );
}
