using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Modules {
    
    [PublicAPI]
    public class ModuleItemSequenceNode : IPrettyPrint {
        private readonly Api _api;
        
        public ModuleItem Prev { get; set; }
        
        public ModuleItem Current { get; set; }
        
        [CanBeNull]
        public ModuleItem Next { get; set; }
        
        [CanBeNull]
        public JObject MasteryPath { get; set; } // todo concrete type?

        internal ModuleItemSequenceNode(Api api, ModuleItemSequenceNodeModel model) {
            _api = api;
            Prev = model.Prev.ConvertIfNotNull(m => new ModuleItem(api, m));
            Current = model.Current.ConvertIfNotNull(m => new ModuleItem(api, m));
            Next = model.Next.ConvertIfNotNull(m => new ModuleItem(api, m));
            MasteryPath = model.MasteryPath;
        }

        public string ToPrettyString() {
            return "ModuleItemSequenceNode {" + 
                   ($"\n{nameof(Prev)}: {Prev?.ToPrettyString()}," +
                   $"\n{nameof(Current)}: {Current?.ToPrettyString()}," +
                   $"\n{nameof(Next)}: {Next?.ToPrettyString()}," +
                   $"\n{nameof(MasteryPath)}: {MasteryPath?.ToString(Formatting.Indented)}").Indent(4) + 
                   "\n}";
        }
    }
}
