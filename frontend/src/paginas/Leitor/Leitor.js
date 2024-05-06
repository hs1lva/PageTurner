import "./a.css"
import { url_server } from "../../contexto/url_servidor";
import {  useState } from "react";
import LoadingModal from "../../componetes/Loading/loading";
import {  toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { ReqOptions } from "../../contexto/reqOptions";


export default function Leitor() {
    const [isLoading, setIsLoading] = useState(false);
    const [autoresData, setAutoresData] = useState([]);
    const [cidadesData, setCidadesData] = useState([]);
    // const tokenGuardado = useSelector((state) => state.token.value);


    const  autores= async () => {
        try{
        setIsLoading(true);
        const response = await fetch(url_server() + '/api/AutorLivro')



        if(response.ok){

            setTimeout(async () => { // Só para testar o loading
                const data = await response.json();
                setAutoresData(data);

                setIsLoading(false);
                toast.success("Autores carregados");

              }, 3000);
        }
    }catch(erro){
        // tratar dos erros criar paginas
    }finally{
        setIsLoading(false);
    }
        
    }

    const cidade = async () => {
        try{
            setIsLoading(true);
            
            const response = await fetch(url_server() + '/api/Cidade', ReqOptions("GET"))
            if(response.ok){
                const data = await response.json();
                setCidadesData(data)
                setIsLoading(false);
            }

            if(response.status === 401){
                console.log("Sem login");
                setCidadesData(["User sem login"]);
                setIsLoading(false);

            }

            if(response.status === 403){
                console.log("Sem autorização")
                setCidadesData(["User sem autorizacão"]);
                setIsLoading(false);

            }
 
            
        }catch(c){
            console.log(c)
        }

    }

    if(isLoading){
        return <LoadingModal showModal={isLoading} />
    }

    return (
        <div className="leitor">
            <h1 className="text-red-600 ">Leitor</h1>
            <h2 className="coisa">qql cosia </h2>
            <div className="flex flex-col w-48 justify-center items-center h-full gap-2">
            <button onClick={autores} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-1 px-4 rounded ">autores </button>
            <div id="autores" className="text-blue-black flex">
                {
                autoresData.map((autores, index) => (
                    <div key={index}>{autores.nomeAutorNome}</div>
                ))
                }

            </div>
            <button onClick={cidade} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-1 px-4 rounded " >cidade</button>
            </div>
            <div className="teste">
                {
                    cidadesData.map((cidades, index) => (
                        <div key={index}>{cidades.nomeCidade}</div>
                    ))
                }
            </div>
        </div>
    );
}