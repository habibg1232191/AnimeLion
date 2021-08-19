using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnimeLion.Parser.Models
{
    public class VideoInfo
    {
        [JsonProperty("domain")]
        public string? Domain { get; set; }
        
        [JsonProperty("links")]
        public Quality? Links { get; set; }
        
        public int EpisodeNum { get; set; }
    }
    
    public class Quality
    {
        [JsonProperty("360")] 
        public List<Links>? LowQuality { get; set; }
        
        [JsonProperty("480")] 
        public List<Links>? AverageQuality { get; set; }
        
        [JsonProperty("720")] 
        public List<Links>? HighQuality { get; set; }
    }
    
    public class Links
    {
        [JsonProperty("src")]
        public string? Src { get; set; }
        
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}