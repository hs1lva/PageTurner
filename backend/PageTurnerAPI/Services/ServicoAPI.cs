

namespace backend.Services
{
    public class ServicoAPI
    {
        private string procurarOpenLibraryEndpoint = "https://openlibrary.org/search.json";
        public ServicoAPI()
        {
        }

        /*public string ProcurarLivro(string nomeLivro)
        {
            return $"{procurarOpenLibraryEndpoint}?q={nomeLivro}";
        }*/

        public async Task<string> BuscarLivrosPorTitulo(string titulo)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{procurarOpenLibraryEndpoint}?q={titulo}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        return jsonResponse;
                    }
                    else
                    {
                        Console.WriteLine($"Falha na solicitação. Código de status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                }
            }

            return null;
        }
    
    }
}