using backend.SimplifiedModels;

namespace backend.Models
{
    public class MatchResultDTO
    {
        public List<Match> Matches { get; set; }
    }

    public class Match
    {
        public UtilizadorSimplificado UserComQueDeiMatch { get; set; }
        public EstanteSimplificada QueEuQuero { get; set; }
        public EstanteSimplificada QueEuTenho { get; set; }
    }
}
