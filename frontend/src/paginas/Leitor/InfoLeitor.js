import { FiEdit } from 'react-icons/fi';
import { Link } from 'react-router-dom';

export default function InfoLeitor({nome, avatar, numAvaliacoes, numComentarios, numMatches, isEditable, onVerMatches}) {

    const userAvatar = avatar ? avatar : "/default.png";

    return (
        <div className="flex items-start space-x-4">
            <img className="object-cover h-36 w-36 rounded-lg" src={userAvatar} alt="Imagem do Leitor"/>
            <div>
                <h1 className="tracking-wide text-4xl font-bold text-yellow-900">{nome.toUpperCase()}</h1>
                <ul className="list-disc list-inside space-y-2 font-mono">
                    <li className="text-sm">{numAvaliacoes} avaliações</li>
                    <li className="text-sm">{numComentarios} comentários</li>
                    <li className="text-sm cursor-pointer" onClick={onVerMatches}>{numMatches} Correspondencias</li>
                </ul>
            </div>
            <div className="flex-grow"></div>
            {isEditable && (
                <Link to="/editar-perfil"
                      className="flex items-center bg-blue-500 text-white px-2 py-1 rounded-full hover:bg-blue-600 transition duration-200">
                    <FiEdit className="mr-1" size={14}/>
                </Link>
            )}
        </div>
    );
}
