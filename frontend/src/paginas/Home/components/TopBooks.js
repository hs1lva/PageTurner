import React from 'react';
import { Link } from 'react-router-dom';
import { DEFAULT_BOOK_COVER} from "../../../services/constants";
import ReactStars from 'react-rating-stars-component';

export default function TopBooks({ topBooks }) {
    return (
        <div className="my-8 px-4">
            <h2 className="text-2xl font-bold mb-4">Mais bem classificados:</h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
                {topBooks.map((book) => (
                    <div key={book.livroId} className="top-book bg-white p-4 rounded-lg shadow hover:shadow-lg transition-shadow">
                        <Link to={`/livro/${book.livroId}`}>
                            <img
                                src={book.capaLarge || DEFAULT_BOOK_COVER}
                                alt={book.tituloLivro}
                                className="w-full h-48 object-cover mb-4 rounded"
                            />
                            <p className="text-lg font-semibold text-center">{book.tituloLivro}</p>
                        </Link>
                        <div className="flex justify-center mt-2">
                            <ReactStars
                                count={5}
                                value={book.mediaAvaliacao}
                                size={16}
                                edit={false}
                                activeColor="#ffd700"
                            />
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
