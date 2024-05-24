namespace backend.SimplifiedModels
{
    public class EstanteSimplificada
    {
        public int EstanteId { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public string TipoEstante { get; set; }
        public UtilizadorSimplificado Utilizador { get; set; }
        public LivroSimplificado Livro { get; set; }
        public bool LivroNaEstante { get; set; }
    }
}