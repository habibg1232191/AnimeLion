using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnimeLion.Parser.Models
{
    public class Episodes
    {
        [JsonProperty("results")]
        public List<Results>? Results { get; set; }
    }

    public class Results
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        
        [JsonProperty("type")]
        public string? Type { get; set; }
        
        [JsonProperty("link")]
        public string? Link { get; set; }
        
        [JsonProperty("translation")]
        public Translation? Translation { get; set; }
        
        [JsonProperty("last_season")]
        public int LastSeason { get; set; }
        
        [JsonProperty("episodes_count")]
        public int EpisodesCount { get; set; }
    }

    public class Translation
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("title")]
        public string? Title { get; set; }
        
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}