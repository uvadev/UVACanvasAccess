using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using static UVACanvasAccess.Api.DiscussionTopicScopes;
using static UVACanvasAccess.Api.DiscussionTopicInclusions;

namespace UVACanvasAccess.Util {
    public static class Extensions {
        internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            return response;
        }
        
        internal static string Indent(this string value, int spaces) {
            var split = value.Split('\n');
            for (var i = 0; i < split.Length - 1; i++) {
                split[i] += "\n";
            }

            var sb = new StringBuilder();
            
            foreach (var s in split) {
                sb.Append(new string(' ', spaces)).Append(s);
            }

            return sb.ToString();
        }

        internal static string ToPrettyString<K, V>(this Dictionary<K, V> dictionary) {
            var sb = new StringBuilder("{");

            foreach (var entry in dictionary) {
                sb.Append($"\n{entry.Key} -> {entry.Value}".Indent(4));
            }

            return sb.Append("\n}").ToString();
        }

        internal static string ToPrettyString<T>(this IEnumerable<T> enumerable)
        where T : IPrettyPrint {
            var strings = enumerable.Select(e => e.ToPrettyString());
            return "[" + string.Join(", ", strings) + "]";
        }
        
        internal static string GetString(this Api.DiscussionTopicOrdering ordering) {
            switch (ordering) {
                case Api.DiscussionTopicOrdering.Position:
                    return "position";
                case Api.DiscussionTopicOrdering.RecentActivity:
                    return "recent_activity";
                case Api.DiscussionTopicOrdering.Title:
                    return "title";
                default:
                    throw new ArgumentOutOfRangeException(nameof(ordering), ordering, null);
            }
        }

        internal static string GetString(this Api.DiscussionTopicScopes scopes) {
            var scopeList = new List<string>();

            if ((scopes & Locked) == Locked) {
                scopeList.Add("locked");
            }
            
            if ((scopes & Unlocked) == Unlocked) {
                scopeList.Add("unlocked");
            }
            
            if ((scopes & Pinned) == Pinned) {
                scopeList.Add("pinned");
            }
            
            if ((scopes & Unpinned) == Unpinned) {
                scopeList.Add("unpinned");
            }

            return string.Join(",", scopeList);
        }

        internal static IEnumerable<(string, string)> GetTuples(this Api.DiscussionTopicInclusions includes) {
            var list = new List<(string, string)>();
            
            if ((includes & AllDates) == AllDates) {
                list.Add(("include[]", "all_dates"));
            }
            
            if ((includes & Sections) == Sections) {
                list.Add(("include[]", "sections"));
            }
            
            if ((includes & SectionsUserCount) == SectionsUserCount) {
                list.Add(("include[]", "sections_user_count"));
            }
            
            if ((includes & Overrides) == Overrides) {
                list.Add(("include[]", "overrides"));
            }

            return list;
        }
    }
}