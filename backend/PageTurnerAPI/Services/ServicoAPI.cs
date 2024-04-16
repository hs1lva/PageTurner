

namespace backend.Services
{
    public class ServicoAPI
    {
        private string procurarOpenLibraryEndpoint = "https://openlibrary.org/search.json";
        public ServicoAPI()
        {
        }

        public string ProcurarLivro(string nomeLivro)
        {
            return $"{procurarOpenLibraryEndpoint}?q={nomeLivro}";
        }
    
    }
}