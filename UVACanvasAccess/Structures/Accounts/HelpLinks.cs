using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    [PublicAPI]
    public class HelpLinks : IPrettyPrint {
        private readonly Api _api;
        
        public string HelpLinkName { get; }
        
        public string HelpLinkIcon { get; }
        
        public IEnumerable<HelpLink> CustomHelpLinks { get; }
        
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