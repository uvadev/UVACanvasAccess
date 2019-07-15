using System.Collections.Generic;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Accounts {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class HelpLink : IPrettyPrint {
        private readonly Api _api;
        
        public string Id { get; }
        
        public string Text { get; }
        
        public string Subtext { get; }
        
        public string Url { get; }
        
        public IEnumerable<string> AvailableTo { get; }

        public HelpLink(Api api, HelpLinkModel model) {
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