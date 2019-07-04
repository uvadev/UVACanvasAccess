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
        
        /// <summary>
        /// Assert that this response has a successful response code.
        /// </summary>
        /// <returns>This.</returns>
        /// <exception cref="Exception">If the response has a failing code.</exception>
        internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}\n" +
                                    $"{JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result)}");
            }

            return response;
        }
        
        /// <summary>
        /// Indents this string by <paramref name="spaces"/> spaces.
        /// </summary>
        /// <param name="value">This string.</param>
        /// <param name="spaces">The number of spaces to indent by.</param>
        /// <returns>The indented string.</returns>
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

        /// <summary>
        /// Pretty-prints this <see cref="Dictionary{TKey,TValue}"/>.
        /// </summary>
        /// <returns>The pretty string.</returns>
        internal static string ToPrettyString<TK, TV>(this Dictionary<TK, TV> dictionary) {
            var sb = new StringBuilder("{");

            foreach (var entry in dictionary) {
                sb.Append($"\n{entry.Key} -> {entry.Value}".Indent(4));
            }

            return sb.Append("\n}").ToString();
        }

        /// <summary>
        /// Pretty-prints this <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns>The pretty string.</returns>
        /// <remarks>
        /// If type <typeparamref name="T"/> implements <see cref="IPrettyPrint"/>, the implementation of
        /// <see cref="IPrettyPrint.ToPrettyString"/> will be used for the values of the collection.
        /// Otherwise, <see cref="object.ToString"/> will be used.
        /// </remarks>
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

        /// <summary>
        /// Gets the string used by Canvas used to refer to this enum member.
        /// </summary>
        /// <returns>The string.</returns>
        /// <remarks>
        /// For any enum member decorated with the <see cref="ApiRepresentationAttribute"/> attribute,
        /// the name returned by this function must be used in requests instead of the literal name of the member,
        /// and any API response will refer to this member by that name.
        /// </remarks>
        /// <seealso cref="ApiRepresentationAttribute"/>
        [CanBeNull]
        internal static string GetApiRepresentation(this Enum en) {
            var member = en.GetType().GetMember(en.ToString());

            if (member.Length <= 0) 
                return null;
            
            var attribute = member[0].GetCustomAttributes(typeof(ApiRepresentationAttribute), false);
            
            return attribute.Length > 0 ? ((ApiRepresentationAttribute) attribute[0]).Representation 
                                        : null;
        }

        /// <summary>
        /// Gets a collection of every flag represented by this flag enum.
        /// </summary>
        /// <returns>The collection of flags.</returns>
        /// <remarks>
        /// If this enum is empty (<c>0x0</c>), any "default" <c>0x0</c> flag will not be returned, and the collection
        /// will be empty.
        /// </remarks>
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

        /// <summary>
        /// If this object is non-<c>null</c>, returns the result of applying the mapping function <paramref name="f"/>.
        /// Otherwise, returns <c>null</c>.
        /// </summary>
        /// <param name="o">The input.</param>
        /// <param name="f">The mapping function.</param>
        /// <returns>The converted object, or <c>null</c>.</returns>
        [ContractAnnotation("o:null => null; o:notnull => notnull")]
        internal static TO ConvertIfNotNull<TI, TO>([CanBeNull] this TI o, [NotNull] Func<TI, TO> f) where TO: class {
            return o == null ? null
                             : f(o);
        }

        /// <summary>
        /// Creates a <see cref="IEnumerable{T}"/> with a single element: this object.
        /// </summary>
        /// <returns>The IEnumerable.</returns>
        internal static IEnumerable<T> Yield<T>(this T t) {
            yield return t;
        }

        /// <summary>
        /// Formats this DateTime according to ISO 8601, as expected by Canvas.
        /// </summary>
        /// <returns>The formatted datetime.</returns>
        internal static string ToIso8601Date(this DateTime dateTime) {
            var s = JsonConvert.SerializeObject(dateTime);
            return s.Substring(1, s.Length - 2);
        }

        /// <summary>
        /// Converts this collection of key-value pairs into a <see cref="ILookup{TKey,TElement}"/>.
        /// </summary>
        /// <returns>The lookup.</returns>
        public static ILookup<TK, TV> Lookup<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> ie) {
            return ie.ToLookup(kv => kv.Key, kv => kv.Value);
        }
        
        /// <summary>
        /// Converts this collection of key-value tuples into a <see cref="ILookup{TKey,TElement}"/>.
        /// </summary>
        /// <returns>The lookup.</returns>
        public static ILookup<TK, TV> Lookup<TK, TV>(this IEnumerable<(TK, TV)> ie) {
            return ie.ToLookup(kv => kv.Item1, kv => kv.Item2);
        }

        /// <summary>
        /// Unzips this collection of tuples.
        /// </summary>
        /// <returns>The unzipped collection.</returns>
        internal static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(this IEnumerable<(T1, T2)> ie) {
            var tuples = ie as (T1, T2)[] ?? ie.ToArray();
            return ValueTuple.Create(tuples.Select(e => e.Item1), 
                                     tuples.Select(e => e.Item2));
        }

        /// <summary>
        /// Flattens this <see cref="ILookup{TKey,TElement}"/> to a collection of key-value tuples.
        /// </summary>
        /// <returns>The collection.</returns>
        internal static IEnumerable<(TK, TV)> Flatten<TK, TV>(this ILookup<TK, TV> lookup) {
            return lookup.SelectMany(k => k, (k, v) => (k.Key, v));
        }

        /// <summary>
        /// Unzips and flattens this collection of tuples by unzipping it and concatenating the results.
        /// </summary>
        /// <returns>The chained collection.</returns>
        /// <example><code>
        /// var x = new[] { (1, 2), (3, 4), (5, 6) };
        /// var y = x.Chain(); // [ 1, 3, 5, 2, 4, 6 ]
        /// </code></example>
        internal static IEnumerable<T> Chain<T>(this IEnumerable<(T, T)> ie) {
            var tuples = ie as (T, T)[] ?? ie.ToArray();
            return tuples.Select(e => e.Item1).Concat(tuples.Select(e => e.Item2));
        }
        
        /// <summary>
        /// Unzips and flattens this collection of tuples by chaining each element together.
        /// </summary>
        /// <returns>The interleaved collection.</returns>
        /// <example><code>
        /// var x = new[] { (1, 2), (3, 4), (5, 6) };
        /// var y = x.Interleave(); // [ 1, 2, 3, 4, 5, 6 ]
        /// </code></example>
        internal static IEnumerable<T> Interleave<T>(this IEnumerable<(T, T)> ie) {
            return ie.SelectMany(t => new[] {t.Item1, t.Item2}, (_, e) => e);
        }
        
        /// <summary>
        /// Filters this collection, removing all <c>null</c> elements.
        /// </summary>
        /// <returns>The filtered collection; guaranteed to have no <c>null</c> elements.</returns>
        [ItemNotNull]
        internal static IEnumerable<T> DiscardNull<T>([ItemCanBeNull] this IEnumerable<T> ie) {
            return ie.Where(e => e != null);
        }
    }
}