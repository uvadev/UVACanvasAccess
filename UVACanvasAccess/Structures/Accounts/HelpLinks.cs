using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    /// <summary>
    /// Represents the set of default and custom help links for an account.
    /// </summary>
    [PublicAPI]
    public class HelpLinks : IPrettyPrint {
        private readonly Api _api;
        
        /// <summary>
        /// The name of the help button.
        /// </summary>
        public string HelpLinkName { get; }
        
        /// <summary>
        /// The icon used for the help button.
        /// </summary>
        public string HelpLinkIcon { get; }
        
        /// <summary>
        /// Account-defined <see cref="HelpLink">help links</see>.
        /// </summary>
        public IEnumerable<HelpLink> CustomHelpLinks { get; }
        
        /// <summary>
        /// Default <see cref="HelpLink">help links</see>.
        /// </summary>
        public IEnumerable<HelpLink> DefaultHelpLinks { get; }

        internal HelpLinks(Api api, HelpLinksModel model) {
            _api = api;
            HelpLinkName = model.HelpLinkName;
            HelpLinkIcon = model.HelpLinkIcon;
            CustomHelpLinks = model.CustomHelpLinks.SelectNotNull(m => new HelpLink(api, m));
            DefaultHelpLinks = model.DefaultHelpLinks.SelectNotNull(m => new HelpLink(api, m));
        }

        public string ToPrettyString() {
            return "HelpLinks {" + 
                   ($"\n{nameof(HelpLinkName)}: {HelpLinkName}," +
                   $"\n{nameof(HelpLinkIcon)}: {HelpLinkIcon}," +
                   $"\n{nameof(CustomHelpLinks)}: {CustomHelpLinks.ToPrettyString()}," +
                   $"\n{nameof(DefaultHelpLinks)}: {DefaultHelpLinks.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
}