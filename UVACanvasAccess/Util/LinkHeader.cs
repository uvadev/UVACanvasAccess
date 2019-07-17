using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace UVACanvasAccess.Util {
    
    [PublicAPI]
    internal class LinkHeader {
        internal string FirstLink { get; private set; }
        internal string PrevLink { get; private set; }
        internal string NextLink { get; private set; }
        internal string LastLink { get; private set; }

        internal static LinkHeader LinksFromHeader(string linkHeaderStr) {
            if (string.IsNullOrWhiteSpace(linkHeaderStr)) 
                return null;
            
            string[] linkStrings = linkHeaderStr.Split(',');

            if (!linkStrings.Any())
                return null;
            
            var linkHeader = new LinkHeader();

            foreach (var linkString in linkStrings) {
                var relMatch = Regex.Match(linkString, "(?<=rel=\").+?(?=\")", RegexOptions.IgnoreCase);
                var linkMatch = Regex.Match(linkString, "(?<=<).+?(?=>)", RegexOptions.IgnoreCase);

                if (!relMatch.Success || !linkMatch.Success) 
                    continue;
                
                var rel = relMatch.Value.ToUpper();
                var link = linkMatch.Value;

                switch (rel) {
                    case "FIRST":
                        linkHeader.FirstLink = link;
                        break;
                    case "PREV":
                        linkHeader.PrevLink = link;
                        break;
                    case "NEXT":
                        linkHeader.NextLink = link;
                        break;
                    case "LAST":
                        linkHeader.LastLink = link;
                        break;
                }
            }

            return linkHeader;
        }
    }
}