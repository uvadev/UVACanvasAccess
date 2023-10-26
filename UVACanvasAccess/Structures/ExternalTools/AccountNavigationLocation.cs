using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ExternalTools;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public class AccountNavigationLocation : ExternalToolLocation, IToolUrl, IToolText, IToolSelectionDimensions, IToolDisplayType {
        public string Url { get; }
        public string Text { get; }
        public uint? SelectionWidth { get; }
        public uint? SelectionHeight { get; }
        
        public ToolDisplayType? DisplayType { get; }

        internal AccountNavigationLocation(Api api, AccountNavigationModel model) : base(api, model.Enabled) {
            Url = model.Url;
            Text = model.Text;
            SelectionWidth = model.SelectionWidth;
            SelectionHeight = model.SelectionHeight;
            DisplayType = model.DisplayType.ToApiRepresentedEnum<ToolDisplayType>() ?? ToolDisplayType.Default;
        }
    }
}
