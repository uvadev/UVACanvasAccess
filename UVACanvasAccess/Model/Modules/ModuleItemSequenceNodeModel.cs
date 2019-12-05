using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UVACanvasAccess.Model.Modules {
    internal class ModuleItemSequenceNodeModel {
        
        [CanBeNull]
        [JsonProperty("prev")]
        public ModuleItemModel Prev { get; set; }
        
        [JsonProperty("current")]
        public ModuleItemModel Current { get; set; }
        
        [CanBeNull]
        [JsonProperty("next")]
        public ModuleItemModel Next { get; set; }
        
        [CanBeNull]
        [JsonProperty("mastery_path")]
        public JObject MasteryPath { get; set; } // todo concrete type?
    }
}
