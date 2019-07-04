using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class ExternalToolTagAttributes : IPrettyPrint {
        private readonly Api _api;
        
        public string Url { get; }
        
        public bool NewTab { get; }
        
        public string ResourceLinkId { get; }

        public ExternalToolTagAttributes(Api api, ExternalToolTagAttributesModel model) {
            _api = api;
            Url = model.Url;
            NewTab = model.NewTab;
            ResourceLinkId = model.ResourceLinkId;
        }

        public string ToPrettyString() {
            return "ExternalToolTagAttributes {" +
                   ($"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(NewTab)}: {NewTab}," +
                   $"\n{nameof(ResourceLinkId)}: {ResourceLinkId}").Indent(4) + 
                   "\n}";
        }
    }
}