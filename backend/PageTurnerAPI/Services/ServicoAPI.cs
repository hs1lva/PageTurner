

namespace backend.Services
{
    public class ServicoAPI
    {
        private string procurarOpenLibraryEndpoint = "https://openlibrary.org/search.json";
        private string campos = "&fields=key,title,author_name,first_publish_year,language,subject";
        public ServicoAPI()
        {
        }

        public async Task<string> BuscarLivrosOpenLibrary(string tipoPesquisa, string termo)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{procurarOpenLibraryEndpoint}?{tipoPesquisa}={termo}{campos}";
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