using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// Representa um comentário feito em um livro dentro da plataforma.
    /// </summary>
    public class ComentarioLivro
    {
        [Key]
        public int comentarioId { get; set; }

        [Required(ErrorMessage = "O conteúdo do comentário é obrigatório.")]
        public string comentario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime dataComentario { get; set; }

        /// <summary>
        /// Estado atual do comentário (por exemplo, pendente, ativo, removido).
        /// </summary>
        public EstadoComentario estadoComentario { get; set; }

        /// <summary>
        /// Lista de conteúdos ofensivos associados a este comentário, se houver.
        /// </summary>
        public ICollection<ComentarioLivroConteudoOfensivo> comentarioConteudoOfensivo { get; set; }

        /// <summary>
        /// Identificador do utilizador que submeteu o comentário.
        /// </summary>
        [Required]
        public int utilizadorId { get; set; }

        /// <summary>
        /// Identificador do livro ao qual o comentário está associado.
        /// </summary>
        [Required]
        public int livroId { get; set; }
    }
}