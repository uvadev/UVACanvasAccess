using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Accounts {
    
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TermsOfServiceModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("terms_type")]
        public string TermsType { get; set; }
        
        [JsonProperty("passive")]
        public bool Passive { get; set; }
        
        [JsonProperty("account_id")]
        public ulong AccountId { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}