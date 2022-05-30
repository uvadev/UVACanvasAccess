using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Files {
    
    /// <summary>
    /// Represents a content license.
    /// </summary>
    [PublicAPI]
    public class License : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The license id.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// The license name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The license url.
        /// </summary>
        public string Url { get; }

        internal License(Api api, LicenseModel model) {
            _api = api;
            Id = model.Id;
            Name = model.Name;
            Url = model.Url;
        }

        /// <inheritdoc /> 
        public string ToPrettyString() {
            return "License {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Url)}: {Url}").Indent(4) + 
                   "\n}";
        }
    }
}
