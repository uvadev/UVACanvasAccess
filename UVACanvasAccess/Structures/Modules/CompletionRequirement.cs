using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Modules;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Modules {
    
    [PublicAPI]
    public class CompletionRequirement : IPrettyPrint {
        private readonly Api _api;

        public string Type { get; }
        
        public uint? MinScore { get; }
        
        public bool? Completed { get; }

        internal CompletionRequirement(Api api, CompletionRequirementModel model) {
            _api = api;
            Type = model.Type;
            MinScore = model.MinScore;
            Completed = model.Completed;
        }

        public string ToPrettyString() {
            return "CompletionRequirement {" +
                   ($"\n{nameof(Type)}: {Type}," +
                   $"\n{nameof(MinScore)}: {MinScore}," +
                   $"\n{nameof(Completed)}: {Completed}").Indent(4) + 
                  "\n}";
        }
    }
}
