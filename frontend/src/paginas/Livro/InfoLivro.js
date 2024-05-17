import React from "react";
import { DEFAULT_BOOK_COVER } from "../../services/constants";

export default function InfoLivro({children, livro}) {
    return (
        <div className="flex w-1/2">
            <div className=" pl-4">
                <h1 className="tracking-wide text-4xl font-bold text-yellow-900">{livro.tituloLivro.toUpperCase()}</h1>
                <img className="mt-5 object-cover h-56 w-36 rounded-lg" src={livro.capaLarge || DEFAULT_BOOK_COVER}
                     alt="Capa do livro"/>
                <ul className="mt-3 list-disc list-inside space-y-2 font-mono">
                   {children}
                    <li className="text-sm">Autor: {livro.autorLivro.nomeAutorNome}</li>
                    <li className="text-sm">Ano de Publicação: {livro.anoPrimeiraPublicacao}</li>
                </ul>
            </div>
        </div>
    )
}