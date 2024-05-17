export default function ComentarioLivro({comentario, utilizador}) {
    return (
        <div key={comentario.comentarioId} className="bg-white shadow overflow-hidden sm:rounded-lg mb-4 p-4">
            <div className="px-4 py-5 sm:px-6 flex items-center">
                <img src={utilizador.fotoPerfil ? utilizador.fotoPerfil : "/default.png" } alt="Avatar do user" className="w-10 h-10 rounded-full mr-4"/>
                <div>
                    <h1 className="mb-2">{utilizador.nome}</h1>
                    <p className="mt-2 max-w-2xl text-sm text-gray-500">{new Date(comentario.dataComentario).toLocaleDateString()}</p>
                </div>
            </div>
            <p className="px-4 py-5 sm:px-6 text-sm leading-6 font-medium text-gray-900">{comentario.comentario}</p>
        </div>
    )
}