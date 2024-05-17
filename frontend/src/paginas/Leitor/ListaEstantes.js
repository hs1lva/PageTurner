import LinhaLivro from "./LinhaLivro";

export default function ListaEstantes({ titulo, estantes = [], userId }) {
    return (
        <div className="p-4">
            <h2 className="font-serif text-2xl text-gray-800 text-center mb-4">{titulo}</h2>
            <div className="flex flex-col bg-white p-4 rounded-lg shadow-md border border-gray-200">
                {estantes.length > 0 ? (
                    <ul className="space-y-1">
                        {estantes.map(estante => (
                            <LinhaLivro key={estante.livro.livroId} livro={estante.livro} estanteId={estante.estanteId} userId={estante.utilizadorId} />
                        ))}
                    </ul>
                ) : (
                    <p className="text-center text-gray-500">Você ainda não adicionou livros a esta estante.</p>
                )}
            </div>
        </div>
    );
}
