using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using static UVACanvasAccess.ApiParts.Api;

namespace UVACanvasAccess.Util {
    public static class Extensions {
        internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}\n" +
                                    $"{JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result)}");
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

        internal static string ToIso8601Date(this DateTime dateTime) {
            var s = JsonConvert.SerializeObject(dateTime);
            return s.Substring(1, s.Length - 2);
        }

        public static ILookup<TK, TV> Lookup<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> ie) {
            return ie.ToLookup(kv => kv.Key, kv => kv.Value);
        }
        
        public static ILookup<TK, TV> Lookup<TK, TV>(this IEnumerable<(TK, TV)> ie) {
            return ie.ToLookup(kv => kv.Item1, kv => kv.Item2);
        }

        internal static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(this IEnumerable<(T1, T2)> ie) {
            var tuples = ie as (T1, T2)[] ?? ie.ToArray();
            return ValueTuple.Create(tuples.Select(e => e.Item1), 
                                     tuples.Select(e => e.Item2));
        }

        internal static IEnumerable<(TK, TV)> Flatten<TK, TV>(this ILookup<TK, TV> lookup) {
            return lookup.SelectMany(k => k, (k, v) => (k.Key, v));
        }

        internal static IEnumerable<T> Chain<T>(this IEnumerable<(T, T)> ie) {
            var tuples = ie as (T, T)[] ?? ie.ToArray();
            return tuples.Select(e => e.Item1).Concat(tuples.Select(e => e.Item2));
        }
        
        internal static IEnumerable<T> Interleave<T>(this IEnumerable<(T, T)> ie) {
            return ie.SelectMany(t => new[] {t.Item1, t.Item2}, (_, e) => e);
        }
    }
}