import { useState } from "react";
import { useNavigate } from "react-router-dom";
import useSearch from "../../hooks/useSearch";
import LoadingModal from "../Loading/loading";

export default function BarraPesquisa() {
    const [query, setQuery] = useState("");
    const { search, isLoading } = useSearch();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        await search(query);
        navigate("/pesquisa");
    };

    return (
        <>
            {isLoading && <LoadingModal showModal={isLoading} />}
            <form className="w-full" onSubmit={handleSubmit}>
                <input
                    className="placeholder:italic placeholder:text-slate-400 block bg-white w-full border border-slate-300 rounded-md py-2 pl-9 pr-3 shadow-sm focus:outline-none focus:border-sky-500 focus:ring-sky-500 focus:ring-1 sm:text-sm"
                    placeholder="Pesquise livros, autores, leitores e muito mais..."
                    type="text"
                    name="search"
                    value={query}
                    onChange={(e) => setQuery(e.target.value)}
                />
            </form>
        </>
    );
}
