using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ExternalTools;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public class UserNavigationLocation : ExternalToolLocation, IToolUrl, IToolText, IToolVisibility {
        public string Url { get; }
        public string Text { get; }
        public ToolVisibility? Visibility { get; }

        internal UserNavigationLocation(Api api, UserNavigationModel model) : base(api, model.Enabled) {
            Url = model.Url;
            Text = model.Text;
            Visibility = model.Visibility?.ToApiRepresentedEnum<ToolVisibility>();
        }
    }
}
