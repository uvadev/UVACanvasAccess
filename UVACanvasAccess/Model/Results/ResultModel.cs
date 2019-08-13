using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Results {
    
    internal class ResultModel {
        // yes indeed the docs say this model specifically uses camelCase properties

        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("userId")]
        public string UserId { get; set; }
        
        [JsonProperty("resultScore")]
        public string ResultScore { get; set; }
        
        [JsonProperty("resultMaximum")]
        public string ResultMaximum { get; set; }
        
        [JsonProperty("comment")]
        public string Comment { get; set; }
        
        [JsonProperty("scoreOf")]
        public string ScoreOf { get; set; }
    }
}
