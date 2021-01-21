using Newtonsoft.Json;

namespace UVACanvasAccess.Model.CustomGradebookColumns {
    
    internal struct ColumnDatumModel {
        
        [JsonProperty("content")]
        public string Content { get; set; }
        
        [JsonProperty("user_id")]
        public ulong UserId { get; set; }
    }
}
