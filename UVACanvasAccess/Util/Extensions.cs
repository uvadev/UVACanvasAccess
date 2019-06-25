using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
    }
}