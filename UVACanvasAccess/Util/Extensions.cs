using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using static UVACanvasAccess.Api;
using static UVACanvasAccess.Api.AssignmentInclusions;
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

        internal static string ToPrettyString<TK, TV>(this Dictionary<TK, TV> dictionary) {
            var sb = new StringBuilder("{");

            foreach (var entry in dictionary) {
                sb.Append($"\n{entry.Key} -> {entry.Value}".Indent(4));
            }

            return sb.Append("\n}").ToString();
        }

        internal static string ToPrettyString<T>([NotNull] [ItemNotNull] this IEnumerable<T> enumerable) {
            var strings = IsA<IPrettyPrint, T>() ? enumerable.Cast<IPrettyPrint>().Select(e => e.ToPrettyString()) 
                                                 : enumerable.Select(e => e.ToString());
            
            return "[" + string.Join(", ", strings) + "]";
        }

        // C# lacks proper generic specialization which makes me sad. This is the best we have.
        internal static bool IsA<TInterface, TType>() {
            return typeof(TInterface).IsAssignableFrom(typeof(TType));
        }

        internal static string GetString(this DiscussionTopicOrdering ordering) {
            switch (ordering) {
                case DiscussionTopicOrdering.Position:
                    return "position";
                case DiscussionTopicOrdering.RecentActivity:
                    return "recent_activity";
                case DiscussionTopicOrdering.Title:
                    return "title";
                default:
                    throw new ArgumentOutOfRangeException(nameof(ordering), ordering, null);
            }
        }

        internal static string GetString(this DiscussionTopicScopes scopes) {
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

        internal static IEnumerable<(string, string)> GetTuples(this DiscussionTopicInclusions includes) {
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
            
            if ((includes & DiscussionTopicInclusions.Overrides) == DiscussionTopicInclusions.Overrides) {
                list.Add(("include[]", "overrides"));
            }

            return list;
        }
        
        internal static IEnumerable<(string, string)> GetTuples(this AssignmentInclusions includes) {
            var list = new List<(string, string)>();
            
            if ((includes & Submission) == Submission) {
                list.Add(("include[]", "submission"));
            }
            
            if ((includes & AssignmentVisibility) == AssignmentVisibility) {
                list.Add(("include[]", "assignment_visibility"));
            }
            
            if ((includes & AssignmentInclusions.Overrides) == AssignmentInclusions.Overrides) {
                list.Add(("include[]", "overrides"));
            }
            
            if ((includes & ObservedUsers) == ObservedUsers) {
                list.Add(("include[]", "observed_users"));
            }

            return list;
        }
    }
}