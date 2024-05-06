import React from "react";
import "./loading.css"; // Estilos do modal de loading

const LoadingModal = ({ showModal }) => {
  return (
    // Modal de loading
    showModal && (
      <div className="loading-modal-overlay">
        <div className="loading-modal">
          <div className="loader book">
            <figure className="page"></figure>
            <figure className="page"></figure>
            <figure className="page"></figure>
          </div>
          <div className="loading-text">Um momento, estamos a ler os dados...</div>
          <p className="text-xs">(Claro que não demora tanto, é só para show off)</p>
        </div>
      </div>
    )
  );
};

export default LoadingModal;
