import imagem from "../../imagens/BanerCriarUser.png";
import "./registar.css";
export default function Registar() {
  return (
    <div className="registar">
      <div className="container-registar">
        <div className="imagem-registo">
          <img src={imagem} alt="" />
        </div>

        {/* <div className="separador-registo"></div> */}

        <div className="registo">
          <div className="opcoes">
            <button>Sobre nós</button>
            <div className="separador-opcoes"></div>
            <button>Pagina Inicial</button>
          </div>
          <div className="ja-tem-conta">
            <p>Já tens conta? </p>
            <p>
              <a href="/login">Login</a>Faça login aqui!
            </p>
          </div>
          <div className="caixa">
            <div className="titulo-caixa"></div>
            <form action="">
              <div className="nome-apelido">
                <input type="text" placeholder="Nome" />
                <input type="text" placeholder="Apelido" />
              </div>
              <div className="email">
                <input type="text" placeholder="email" />
              </div>
              <div className="palavra-pass">
                <input type="text" placeholder="Palavra Pass" />
                <input type="text" placeholder="Confirmar" />
              </div>
              <div className="aviso">
                <p>
                  Antes de prosseguir, por favor, esteja ciente de que ao clicar
                  em 'Registrar, você estará concordando com os nossos Termos de
                  Uso. É importante que você leia e compreenda os termos que
                  regem o uso deste serviço. Caso tenha dúvidas ou preocupações,
                  não hesite em entrar em contato conosco. Obrigado por escolher
                  se juntar à nossa comunidade!"
                </p>
              </div>
              <button type="submit">Registar</button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}
