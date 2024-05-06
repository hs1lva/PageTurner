// import { useSelector } from "react-redux";

// import { useToken } from "../contexto/TokenContext";

export const ReqOptions = ({metodo}) => {
    const tokenSession = sessionStorage.getItem("PageTurnerUser"); // Extrair o cookie do navegador
    // const teste = useSelector((state) => state.token.value);
    // console.log("Token guardado pelo useSelector:")
    // console.log(teste);
    // const { token } = useToken();
    // console.log("Token guardado pelo contexto:")
    // console.log(token);

    const headers = {
      "Content-Type": "application/json",
      Authorization: `Bearer ${tokenSession}`,
    };
    const request = {
      method: metodo,
      headers: headers,
    };

    return request;
  };
  