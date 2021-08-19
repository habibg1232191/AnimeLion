using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AnimeLion.Parser.Models
{
    public class TopAnimeCurrentYear
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("shikimoriId")]
        public string ShikimoriId { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}