using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace AnimeLion.Parser
{
    public class EpisodesParser
    {
        private readonly string _link;

        public EpisodesParser(string link)
        {
            _link = link;
        }

        public async Task<List<EpisodeParser>> GetEpisodes()
        {
            var config = new HtmlParserOptions { IsScripting = true };
            HtmlParser parser = new HtmlParser(config);
            var downloadString = new WebClient();
            var document = parser.ParseDocument(await downloadString.DownloadStringTaskAsync("http:" + _link));
            var episodes = document.QuerySelectorAll(".serial-panel .serial-series-box select option");
            List<EpisodeParser> episodesParser = new List<EpisodeParser>();

            foreach (var element in episodes)
            {
                episodesParser.Add(new EpisodeParser(element.GetAttribute("value"), 
                    Int32.Parse(element.GetAttribute("data-num"))));
            }
            
            return episodesParser;
        }
    }
}