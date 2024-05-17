import { Link } from 'react-router-dom';
import LinhaLivro from './LinhaLivro';

export default function Estante({ titulo, estantes = [], userId }) {
    const livros = estantes.map(estante => estante.livro).filter(livro => livro);

    return (
        <div className="flex flex-col w-full p-4 rounded-lg shadow-md bg-white border border-gray-200 min-h-[300px]">
            <h2 className="font-serif text-xl text-gray-800 text-center mb-4">{titulo}</h2>
            <div className="flex-grow overflow-auto">
                <ul className="space-y-1">
                    {livros.slice(0, 5).map(livro => (
                        <LinhaLivro key={livro.livroId} livro={livro} />
                    ))}
                </ul>
            </div>
            <Link
                to={{
                    pathname: `/estantes/${titulo.toLowerCase()}/${userId}`,
                    state: { livros }
                }}
                className="mt-4 py-2 px-4 bg-yellow-500 text-white rounded hover:bg-yellow-600 focus:outline-none focus:ring-2 focus:ring-yellow-400 focus:ring-opacity-75 text-center"
            >
                Ver Todos
            </Link>
        </div>
    );
}
