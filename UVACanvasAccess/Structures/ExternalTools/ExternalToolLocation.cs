using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;

namespace UVACanvasAccess.Structures.ExternalTools {

    [PublicAPI]
    public abstract class ExternalToolLocation {
        
        private protected Api Api;
        public bool Enabled { get; }

        private protected ExternalToolLocation(Api api, bool? enabled) {
            Api = api;
            Enabled = enabled ?? false;
        }
    }
}
