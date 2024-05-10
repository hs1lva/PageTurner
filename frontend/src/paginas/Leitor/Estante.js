export default function Estante({titulo, livros}) {
    return (
        <div className="flex w-full flex-col bg-red-100 p-4 h-80 rounded-lg">
            <div className="flex-grow overflow-auto">
                <h2 className="font-serif text-xl text-yellow-900 self text-center mb-4">{titulo}</h2>
                <ul>
                    {livros.map((livro, index) => (
                        <li key={index} className="mb-2">{livro}</li>
                    ))}
                </ul>
            </div>
            <button className="mt-4 text-yellow-900 text-center underline">Ver Todos</button>
        </div>
    )
}