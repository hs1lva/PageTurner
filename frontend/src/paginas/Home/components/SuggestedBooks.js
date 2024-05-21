import React, { useRef, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { DEFAULT_BOOK_COVER } from "../../../services/constants";
import '../home.css';

export default function SuggestedBooks({ books, user }) {
    const carouselRef = useRef(null);

    useEffect(() => {
        if (carouselRef.current) {
            carouselRef.current.style.setProperty('--num-books', books.length);
        }
    }, [books]);

    return (
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
                                    <Link to={`/livro/${livro.livroId}`}>
                                        <img
                                            src={livro.capaSmall || DEFAULT_BOOK_COVER}
                                            alt={livro.tituloLivro}
                                            className="book-cover"
                                        />
                                        <p className="book-title">{livro.tituloLivro}</p>
                                    </Link>
                                </div>
                            ))}
                        </div>
                    </div>
                ) : (
                    <p>Ainda não há sugestões de livros a apresentar.</p>
                )}
            </footer>
        </div>
    );
}
