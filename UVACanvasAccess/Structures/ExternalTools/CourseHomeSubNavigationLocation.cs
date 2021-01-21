using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ExternalTools;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public class CourseHomeSubNavigationLocation : ExternalToolLocation, IToolUrl, IToolText, IToolIconUrl {
        public string Url { get; }
        public string Text { get; }
        public string IconUrl { get; }

        internal CourseHomeSubNavigationLocation(Api api, CourseHomeSubNavigationModel model) : base(api, model.Enabled) {
            Url = model.Url;
            Text = model.Text;
            IconUrl = model.IconUrl;
        }
    }
}
