using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ExternalTools;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public class ResourceSelectionLocation : ExternalToolLocation, IToolUrl, IToolIconUrl, IToolSelectionDimensions {
        public string Url { get; }
        public string IconUrl { get; }
        public uint? SelectionWidth { get; }
        public uint? SelectionHeight { get; }

        internal ResourceSelectionLocation(Api api, ResourceSelectionModel model) : base(api, model.Enabled) {
            Url = model.Url;
            IconUrl = model.IconUrl;
            SelectionWidth = model.SelectionWidth;
            SelectionHeight = model.SelectionHeight;
        }
    }
}
