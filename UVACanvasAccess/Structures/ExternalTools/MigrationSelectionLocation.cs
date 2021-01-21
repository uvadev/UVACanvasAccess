using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ExternalTools;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public class MigrationSelectionLocation : ExternalToolLocation, IToolUrl, IToolMessageType {
        public string Url { get; }
        public string MessageType { get; }

        internal MigrationSelectionLocation(Api api, MigrationSelectionModel model) : base(api, model.Enabled) {
            Url = model.Url;
            MessageType = model.MessageType;
        }
    }
}
