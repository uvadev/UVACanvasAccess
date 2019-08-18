using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Files {
    
    [PublicAPI]
    public class License : IPrettyPrint {
        private readonly Api _api;
        
        public string Id { get; }
        
        public string Name { get; }
        
        public string Url { get; }

        internal License(Api api, LicenseModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            Url = model.Url;
        }

        public string ToPrettyString() {
            return "License {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Url)}: {Url}").Indent(4) + 
                   "\n}";
        }
    }
}
