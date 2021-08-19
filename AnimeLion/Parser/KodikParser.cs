using System;
using System.Net;
using System.Threading.Tasks;
using AnimeLion.Parser.Models;
using AnimeLion.Parser.ShikimoriModels;
using Newtonsoft.Json;

namespace AnimeLion.Parser
{
    public class KodikParser
    {
        public Episodes Episodes;
        public readonly string UrlKodikApi;
        public readonly Anime Anime;

        public KodikParser(Anime anime)
        {
            UrlKodikApi = "https://kodikapi.com/search?token=4a15fd264f4860aa018cfa26d8868a3d&shikimori_id=";
            Anime = anime;
            Episodes = new Episodes();
        }

        public async Task<Episodes> GetEpisodes()
        {
            var webClient = new WebClient {Encoding = System.Text.Encoding.UTF8};
            var shikimoriResponse = await webClient.DownloadStringTaskAsync(new Uri(UrlKodikApi + Anime.Id));
            Episodes = JsonConvert.DeserializeObject<Episodes>(shikimoriResponse)!;
            return Episodes;
        }
    }
}