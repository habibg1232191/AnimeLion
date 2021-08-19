using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AnimeLion.Parser.ShikimoriModels;
using Newtonsoft.Json;

namespace AnimeLion.Parser
{
    class ShikimoriApi
    {
        private string UrlShikimori = "https://shikimori.one/api/animes";
        
        public async Task<List<Anime>> GetAnimes(Dictionary<string, string> keys)
        {
            var webClient = new WebClient {Encoding = System.Text.Encoding.UTF8};
            var shikimoriResponse = await webClient.DownloadStringTaskAsync(new Uri(UrlShikimori + "?" + DictionaryToParameters(keys)));
            List<Anime>? animes = JsonConvert.DeserializeObject<List<Anime>>(shikimoriResponse);

            return animes!;
        }

        public async Task<FullAnime> GetAnimeWithId(int id)
        {
            var webClient = new WebClient {Encoding = System.Text.Encoding.UTF8};
            var shikimoriResponse = await webClient.DownloadStringTaskAsync(new Uri(UrlShikimori + "/" + id));
            
            
            return JsonConvert.DeserializeObject<FullAnime>(shikimoriResponse)!;
        }

        private string DictionaryToParameters(Dictionary<string, string> parameters)
        {
            string result = String.Empty;

            foreach (var parameter in parameters)
                result += parameter.Key + "=" + parameter.Value + "&";
            
            return result;
        }
    }
}