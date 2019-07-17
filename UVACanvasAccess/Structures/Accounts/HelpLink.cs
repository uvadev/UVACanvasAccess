using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    /// <summary>
    /// Represents an account help link.
    /// </summary>
    [PublicAPI]
    public class HelpLink : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The help link's id.
        /// </summary>
        public string Id { get; }
        
        /// <summary>
        /// The help link's name.
        /// </summary>
        public string Text { get; }
        
        /// <summary>
        /// The help link's description.
        /// </summary>
        public string Subtext { get; }
        
        /// <summary>
        /// The help link's URL.
        /// </summary>
        public string Url { get; }
        
        /// <summary>
        /// The roles that can see this help link.
        /// </summary>
        public IEnumerable<string> AvailableTo { get; }

        internal HelpLink(Api api, HelpLinkModel model) {
            _api = api;
            Id = model.Id;
            Text = model.Text;
            Subtext = model.Subtext;
            Url = model.Url;
            AvailableTo = model.AvailableTo;
        }

        public string ToPrettyString() {
            return "HelpLink {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Text)}: {Text}," +
                   $"\n{nameof(Subtext)}: {Subtext}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(AvailableTo)}: {AvailableTo.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}