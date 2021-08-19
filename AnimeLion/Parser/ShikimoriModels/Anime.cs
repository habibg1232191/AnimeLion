using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AnimeLion.Parser.ShikimoriModels
{
    public class Anime
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("russian")]
        public string? RussianName { get; set; }
        
        [JsonProperty("image")]
        public TitleImage? Image { get; set; }
        
        [JsonProperty("url")]
        public string? Url { get; set; }
        
        [JsonProperty("kind")]
        public string? Kind { get; set; }
        
        [JsonProperty("score")]
        public string? Score { get; set; }
        
        [JsonProperty("status")]
        public string? Status { get; set; }
        
        [JsonProperty("episodes")]
        public int Episodes { get; set; }
        
        [JsonProperty("episodes_aired")]
        public int EpisodesAired { get; set; }
        
        [JsonProperty("aired_on")]
        public DateTime? AiredOn { get; set; }
        
        [JsonProperty("released_on")]
        public string? ReleasedOn { get; set; }
    }

    public class TitleImage
    {
        [JsonPropertyName("original")]
        public string Original { get; set; }
        
        [JsonPropertyName("preview")]
        public string Preview { get; set; }
        
        [JsonPropertyName("x96")]
        public string x96 { get; set; }
        
        [JsonPropertyName("x48")]
        public string x48 { get; set; }
    }
}