using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Exceptions;
using static UVACanvasAccess.ApiParts.Api;

namespace UVACanvasAccess.Util {
    public static class Extensions {
        
        /// <summary>
        /// Assert that this response has a successful response code.
        /// </summary>
        /// <returns>This.</returns>
        /// <exception cref="Exception">If the response has a failing code.</exception>
        internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response) {
            if (response.IsSuccessStatusCode) 
                return response;
            try {
                var body = JObject.Parse(response.Content.ReadAsStringAsync().Result)["errors"].ToString();
                switch (response.StatusCode) {
                    case HttpStatusCode.NotFound: 
                        throw new DoesNotExistException(body);
                    default: 
                        throw new CommunicationException($"Http Failure: {response.StatusCode}\n{body}");
                }
            } catch (JsonException) {
                throw new CommunicationException($"Http Failure: {response.StatusCode}\n");
            }
        }
        
        internal static Task<HttpResponseMessage> AssertSuccess(this Task<HttpResponseMessage> response) {
            return response.ThenApply(r => {
                                           r.AssertSuccess();
                                           return r;
                                       });
        }
        
        /// <summary>
        /// Indents this string by <paramref name="spaces"/> spaces.
        /// </summary>
        /// <param name="value">This string.</param>
        /// <param name="spaces">The number of spaces to indent by.</param>
        /// <returns>The indented string.</returns>
        [Pure]
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
        [Pure]
        public static string ToPrettyString<TK, TV>(this Dictionary<TK, TV> dictionary) {
            var sb = new StringBuilder("{");

            var kIsPretty = IsA<IPrettyPrint, TK>();
            var vIsPretty = IsA<IPrettyPrint, TV>();
            
            foreach (var (key, val) in dictionary) {
                var k = kIsPretty ? ((IPrettyPrint) key).ToPrettyString() 
                                  : key.ToString();
                var v = vIsPretty ? ((IPrettyPrint) val).ToPrettyString() 
                                  : val.ToString();
                sb.Append($"\n{k} -> {v}".Indent(4));
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
        [Pure]
        public static string ToPrettyString<T>([NotNull] [ItemNotNull] this IEnumerable<T> enumerable) {
            var strings = IsA<IPrettyPrint, T>() ? enumerable.Cast<IPrettyPrint>().Select(e => e.ToPrettyString()) 
                                                 : enumerable.Select(e => e.ToString());
            
            return "[" + string.Join(", ", strings) + "]";
        }

        // C# lacks proper generic specialization which makes me sad. This is the best we have.
        [Pure]
        internal static bool IsA<TInterface, TType>() {
            return typeof(TInterface).IsAssignableFrom(typeof(TType));
        }

        [Pure]
        internal static string GetString(this DiscussionTopicOrdering ordering) {
            return ordering.GetApiRepresentation();
        }

        [Pure]
        internal static string GetString(this DiscussionTopicScopes scopes) { 
            return string.Join(",", scopes.GetFlags().Select(f => f.GetFlags()));
        }

        [Pure]
        internal static IEnumerable<(string, string)> GetTuples(this DiscussionTopicInclusions includes) {
            return includes.GetFlags().Select(f => ("include[]", f.GetApiRepresentation())).ToList();
        }
        
        [Pure]
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
        [Pure]
        internal static string GetApiRepresentation([NotNull] this Enum en) {
            MemberInfo[] member = en.GetType().GetMember(en.ToString());

            if (member.Length <= 0) 
                return null;
            
            object[] attribute = member[0].GetCustomAttributes(typeof(ApiRepresentationAttribute), false);
            
            return attribute.Length > 0 ? ((ApiRepresentationAttribute) attribute[0]).Representation 
                                        : null;
        }

        [PublicAPI]
        public static IEnumerable<T> Peek<T>([NotNull] this IEnumerable<T> ie, Action<T> a) {
            List<T> list = ie.ToList();
            foreach (var e in list) {
                a(e);
            }

            return list;
        }

        [Pure]
        internal static IEnumerable<string> GetApiRepresentations([NotNull] this IEnumerable<Enum> ie) {
            return ie.Select(e => e.GetApiRepresentation());
        }

        [Pure]
        internal static IEnumerable<string> GetFlagsApiRepresentations([NotNull] this Enum en) {
            return en.GetFlags().GetApiRepresentations();
        }

        [CanBeNull]
        [Pure]
        internal static T? ToApiRepresentedEnum<T>(this string str) where T: struct, Enum {
            foreach (T field in Enum.GetValues(typeof(T))) {
                var representation = field.GetApiRepresentation();
                if (str == representation) {
                    return field;
                }
            }
            return null;
        }
        
        [Pure]
        internal static T ToApiRepresentedFlagsEnum<T>([NotNull] this IEnumerable<string> ie) where T: struct, Enum {
            Debug.Assert(Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) != null);
            
            return (T) ie.SelectNotNullValue(s => s.ToApiRepresentedEnum<T>())
                         .Cast<dynamic>()
                         .Aggregate((a, b) => a | b);
        }

        internal static void Deconstruct<TK, TV>(this KeyValuePair<TK, TV> kvp, out TK key, out TV val) {
            key = kvp.Key;
            val = kvp.Value;
        }
        
        [Pure]
        internal static string ToShortString(this bool b) {
            return b ? "1" 
                     : "0";
        }

        /// <summary>
        /// Gets a collection of every flag represented by this flag enum.
        /// </summary>
        /// <returns>The collection of flags.</returns>
        /// <remarks>
        /// If this enum is empty (<c>0x0</c>), any "default" <c>0x0</c> flag will not be returned, and the collection
        /// will be empty.
        /// </remarks>
        [Pure]
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
        [Pure]
        internal static TO ConvertIfNotNull<TI, TO>([CanBeNull] this TI o, [NotNull] Func<TI, TO> f) where TO: class {
            return o == null ? null
                             : f(o);
        }

        [NotNull]
        [Pure]
        internal static IEnumerable<TO> SelectNotNull<TI, TO>([CanBeNull] [ItemCanBeNull] this IEnumerable<TI> ie, 
                                                              [NotNull] Func<TI, TO> f) {
            return ie?.DiscardNull().Select(f) ?? Enumerable.Empty<TO>();
        }
        
        [NotNull]
        [Pure]
        internal static IEnumerable<TO> SelectNotNullKey<TI, TO>([CanBeNull] [ItemCanBeNull] this IEnumerable<TI?> ie, 
                                                                 [NotNull] Func<TI, TO> f) where TI: struct {
            return ie?.DiscardNullValue().Select(f) ?? Enumerable.Empty<TO>();
        }
        
        [NotNull]
        [Pure]
        internal static IEnumerable<TO> SelectNotNullValue<TI, TO>([CanBeNull] [ItemCanBeNull] this IEnumerable<TI> ie, 
                                                                   [NotNull] Func<TI, TO?> f) where TO: struct {
            return ie?.DiscardNull().Select(f).DiscardNullValue() ?? Enumerable.Empty<TO>();
        }

        [Pure]
        internal static KeyValuePair<TO, TV> KeySelect<TK, TV, TO>(this KeyValuePair<TK, TV> kvp, Func<TK, TO> f) {
            return KeyValuePair.New(f(kvp.Key), kvp.Value);
        }
        
        [Pure]
        internal static KeyValuePair<TK, TO> ValSelect<TK, TV, TO>(this KeyValuePair<TK, TV> kvp, Func<TV, TO> f) {
            return KeyValuePair.New(kvp.Key, f(kvp.Value));
        }
        
        [Pure]
        internal static (TO, TV) KeySelect<TK, TV, TO>(this (TK, TV) kvp, Func<TK, TO> f) {
            return (f(kvp.Item1), kvp.Item2);
        }
        
        [Pure]
        internal static (TK, TO) ValSelect<TK, TV, TO>(this (TK, TV) kvp, Func<TV, TO> f) {
            return (kvp.Item1, f(kvp.Item2));
        }
        
        [Pure]
        internal static IEnumerable<(TO, TV)> KeySelect<TK, TV, TO>(this IEnumerable<(TK, TV)> kvp, Func<TK, TO> f) {
            return kvp.Select(kv => kv.KeySelect(f));
        }
        
        [Pure]
        internal static IEnumerable<(TK, TO)> ValSelect<TK, TV, TO>(this IEnumerable<(TK, TV)> kvp, Func<TV, TO> f) {
            return kvp.Select(kv => kv.ValSelect(f));
        }


        [Pure]
        internal static Dictionary<TO, TV> KeySelect<TK, TV, TO>(this IDictionary<TK, TV> d, Func<TK, TO> f) {
            return d.Select(kv => kv.KeySelect(f)).IdentityDictionary();
        }
        
        [Pure]
        internal static Dictionary<TK, TO> ValSelect<TK, TV, TO>(this IDictionary<TK, TV> d, Func<TV, TO> f) {
            return d.Select(kv => kv.ValSelect(f)).IdentityDictionary();
        }

        [Pure]
        internal static Dictionary<TK, TV> IdentityDictionary<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> ie) {
            return ie.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        /// Creates a <see cref="IEnumerable{T}"/> with a single element: this object.
        /// </summary>
        /// <returns>The IEnumerable.</returns>
        [Pure]
        internal static IEnumerable<T> Yield<T>(this T t) {
            yield return t;
        }

        /// <summary>
        /// Formats this DateTime according to ISO 8601, as expected by Canvas.
        /// </summary>
        /// <returns>The formatted datetime.</returns>
        [Pure]
        internal static string ToIso8601Date(this DateTime dateTime) {
            var s = JsonConvert.SerializeObject(dateTime);
            return s.Substring(1, s.Length - 2);
        }

        /// <summary>
        /// Converts this collection of key-value pairs into a <see cref="ILookup{TKey,TElement}"/>.
        /// </summary>
        /// <returns>The lookup.</returns>
        [Pure]
        public static ILookup<TK, TV> Lookup<TK, TV>(this IEnumerable<KeyValuePair<TK, TV>> ie) {
            return ie.ToLookup(kv => kv.Key, kv => kv.Value);
        }
        
        /// <summary>
        /// Converts this collection of key-value tuples into a <see cref="ILookup{TKey,TElement}"/>.
        /// </summary>
        /// <returns>The lookup.</returns>
        [Pure]
        public static ILookup<TK, TV> Lookup<TK, TV>(this IEnumerable<(TK, TV)> ie) {
            return ie.ToLookup(kv => kv.Item1, kv => kv.Item2);
        }

        /// <summary>
        /// Unzips this collection of tuples.
        /// </summary>
        /// <returns>The unzipped collection.</returns>
        [Pure]
        internal static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(this IEnumerable<(T1, T2)> ie) {
            var tuples = ie as (T1, T2)[] ?? ie.ToArray();
            return ValueTuple.Create(tuples.Select(e => e.Item1), 
                                     tuples.Select(e => e.Item2));
        }

        /// <summary>
        /// Flattens this <see cref="ILookup{TKey,TElement}"/> to a collection of key-value tuples.
        /// </summary>
        /// <returns>The collection.</returns>
        [Pure]
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
        [Pure]
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
        [Pure]
        internal static IEnumerable<T> Interleave<T>(this IEnumerable<(T, T)> ie) {
            return ie.SelectMany(t => new[] {t.Item1, t.Item2}, (_, e) => e);
        }
        
        /// <summary>
        /// Filters this collection, removing all <c>null</c> elements.
        /// </summary>
        /// <returns>The filtered collection; guaranteed to have no <c>null</c> elements.</returns>
        [ItemNotNull]
        [Pure]
        internal static IEnumerable<T> DiscardNull<T>([ItemCanBeNull] this IEnumerable<T> ie) {
            return ie.Where(e => e != null);
        }
        
        [Pure]
        internal static IEnumerable<T> DiscardNullValue<T>(this IEnumerable<T?> ie) where T: struct {
            return ie.Where(e => e != null)
                     .Select(e => e.Value);
        }

        public static Task<TO> ThenApply<TI, TO>(this Task<TI> task, Func<TI, TO> f) {
            return task.ContinueWith(t => f(t.Result));
        }
        
        public static Task ThenAccept<TI>(this Task<TI> task, Action<TI> f) {
            return task.ContinueWith(t => f(t.Result));
        }
        
        public static Task<TI> ThenPeek<TI>(this Task<TI> task, Action<TI> f) {
            return task.ContinueWith(t => { 
                                         f(t.Result);
                                         return t.Result;
                                     });
        }
    }
}