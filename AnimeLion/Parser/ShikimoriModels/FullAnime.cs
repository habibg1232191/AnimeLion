using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnimeLion.Parser.ShikimoriModels
{
    public class FullAnime : Anime
    {
        [JsonProperty("rating")]
        public string? Rating { get; set; }
        
        [JsonProperty("english")]
        public List<string>? English { get; set; }
        
        [JsonProperty("japanese")]
        public List<string>? Japanese { get; set; }
        
        [JsonProperty("synonyms")]
        public List<string>? Synonyms { get; set; }
        
        [JsonProperty("license_name_ru")]
        public string? LicenseNameRu { get; set; }
        
        [JsonProperty("duration")]
        public int Duration { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("description_html")]
        public string? DescriptionHtml { get; set; }
        
        [JsonProperty("description_source")]
        public string? DescriptionSource { get; set; }
    }
}