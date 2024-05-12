

using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace backend.Services
{
    public class ServicoAPI
    {
        private string procurarOpenLibraryEndpoint = "https://openlibrary.org/search.json";
        private string campos = "&fields=seed,key,title,author_name,first_publish_year,language,subject";

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

                        JObject jsonObject = JObject.Parse(jsonResponse);
                        foreach (var item in jsonObject["docs"])
                        {
                            if (item != null)
                            {
                                string keyTemp = item["seed"]?.ToString();

                                string pattern = @"OL\d+M";

                                // Procura por correspondências no texto usando a expressão regular
                                Match match = Regex.Match(keyTemp, pattern);

                                // Extrai o identificador do livro, se encontrado
                                string key = match.Success ? match.Value : null;

                                string[] capas = new string[]
                                {
                                    $"https://covers.openlibrary.org/b/olid/{key}-S.jpg",
                                    $"https://covers.openlibrary.org/b/olid/{key}-M.jpg",
                                    $"https://covers.openlibrary.org/b/olid/{key}-L.jpg"
                                };

                                //item["capas"] = JToken.FromObject(capas);
                                item["capaSmall"] = capas[0];
                                item["capaMedium"] = capas[1];
                                item["capaLarge"] = capas[2];
                            }
                        }

                        return jsonObject.ToString();
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