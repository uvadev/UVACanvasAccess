using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Modules {
    
    internal class ModuleItemSequenceModel {
        
        [JsonProperty("items")]
        public IEnumerable<ModuleItemSequenceNodeModel> Items { get; set; }
    }
}
