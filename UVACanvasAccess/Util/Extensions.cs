using System;
using System.Net.Http;

namespace UVACanvasAccess.Util {
    public static class Extensions {
        internal static HttpResponseMessage AssertSuccess(this HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                throw new Exception($"http failure response: {response.StatusCode} {response.ReasonPhrase}");
            }

            return response;
        }
    }
}