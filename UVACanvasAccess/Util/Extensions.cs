using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using static UVACanvasAccess.Api;

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
            return ordering.GetApiRepresentation();
        }

        internal static string GetString(this DiscussionTopicScopes scopes) { 
            return string.Join(",", scopes.GetFlags().Select(f => f.GetFlags()));
        }

        internal static IEnumerable<(string, string)> GetTuples(this DiscussionTopicInclusions includes) {
            return includes.GetFlags().Select(f => ("include[]", f.GetApiRepresentation())).ToList();
        }
        
        internal static IEnumerable<(string, string)> GetTuples(this AssignmentInclusions includes) {
            return includes.GetFlags().Select(f => ("include[]", f.GetApiRepresentation())).ToList();
        }

        [CanBeNull]
        internal static string GetApiRepresentation(this Enum en) {
            var member = en.GetType().GetMember(en.ToString());

            if (member.Length <= 0) 
                return null;
            
            var attribute = member[0].GetCustomAttributes(typeof(ApiRepresentationAttribute), false);
            
            return attribute.Length > 0 ? ((ApiRepresentationAttribute) attribute[0]).Representation 
                                        : null;
        }

        internal static IEnumerable<TE> GetFlags<TE>(this TE en) where TE : Enum {
            ulong flag = 1;
            
            foreach (var value in Enum.GetValues(en.GetType()).Cast<TE>()) {
                var bits = Convert.ToUInt64(value);
                
                while (flag < bits) {
                    flag <<= 1;
                }

                if (flag == bits && en.HasFlag(value)) {
                    yield return value;
                }
            }
        }

        [ContractAnnotation("o:null => null; o:notnull => notnull")]
        internal static TO ConvertIfNotNull<TI, TO>([CanBeNull] this TI o, [NotNull] Func<TI, TO> f) where TO: class {
            return o == null ? null
                             : f(o);
        }

        internal static IEnumerable<T> Yield<T>(this T t) {
            yield return t;
        }
    }
}