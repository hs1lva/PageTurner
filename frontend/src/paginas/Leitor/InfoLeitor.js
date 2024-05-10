import useAuthStore from "../../services/authService";

export default function InfoLeitor({nome, avatar, numAvaliacoes, numComentarios}) {

    const userAvatar = avatar ? avatar : "default.png";

    return (
        <div className="flex w-1/2">
            <img className="object-cover h-36 w-36 rounded-lg" src={userAvatar} alt="Imagem do Leitor"/>
            <div className=" pl-4">
                <h1 className="tracking-wide text-4xl font-bold text-yellow-900">{nome.toUpperCase()}</h1>
                <ul className="list-disc list-inside space-y-2 font-mono">
                    <li className="text-sm">{numAvaliacoes} avaliações</li>
                    <li className="text-sm">{numComentarios} comentários</li>
                    <li className="text-sm">TODO: Correspondencias</li>
                    <li className="text-sm">TODO: Recomendações</li>
                </ul>
            </div>
        </div>
    )
}