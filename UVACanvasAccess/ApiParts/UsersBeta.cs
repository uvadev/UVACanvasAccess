using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.Users.Beta;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        [PublicAPI]
        public enum PageViewsFormat {
            [ApiRepresentation("jsonl")]
            JsonL,
            [ApiRepresentation("csv")]
            Csv
        }

        [PublicAPI]
        public enum PageViewsQueryStatus {
            [ApiRepresentation("queued")]
            Queued,
            [ApiRepresentation("running")]
            Running,
            [ApiRepresentation("finished")]
            Finished,
            [ApiRepresentation("failed")]
            Failed
        }
        
        private async Task<AsyncQueryResponseModel> InitiatePageViewsQuery(ulong userId, DateTime startDate, DateTime endDate, PageViewsFormat format) {
            var args = new[] {
                ("start_date", startDate.ToShortIso8601Date()),
                ("end_date", endDate.ToShortIso8601Date()),
                ("results_format", format.GetApiRepresentation())
            };
            
            var response = await client.PostAsync($"users/{userId}/page_views/query", BuildHttpArguments(args));
            response.AssertSuccess();
            
            return JsonConvert.DeserializeObject<AsyncQueryResponseModel>(await response.Content.ReadAsStringAsync());
        }

        private async Task<AsyncQueryResponseModel> InitiateBatchPageViewsQuery(IEnumerable<ulong> userIds,
                                                                                DateTime startDate,
                                                                                DateTime endDate,
                                                                                PageViewsFormat format) {
            var args = new JObject {
                ["start_date"] = startDate.ToShortIso8601Date(),
                ["end_date"] = endDate.ToShortIso8601Date(),
                ["results_format"] = format.GetApiRepresentation(),
                ["user_ids"] = JArray.FromObject(userIds)
            };
            
            var response = await client.PostAsync("users/page_views/query", BuildHttpJsonBody(args));
            response.AssertSuccess();
            
            return JsonConvert.DeserializeObject<AsyncQueryResponseModel>(await response.Content.ReadAsStringAsync());
        }
        
        private async Task<AsyncQueryStatusResponseModel> PollPageViewsQuery(ulong userId, string queryId) {
            var response = await client.GetAsync($"users/{userId}/page_views/query/{queryId}");
            response.AssertSuccess();
            
            return JsonConvert.DeserializeObject<AsyncQueryStatusResponseModel>(await response.Content.ReadAsStringAsync());
        }
        
        private async Task<AsyncQueryStatusResponseModel> PollPageViewsQuery(string url) {
            var response = await client.GetAsync(url);
            response.AssertSuccess();
            
            return JsonConvert.DeserializeObject<AsyncQueryStatusResponseModel>(await response.Content.ReadAsStringAsync());
        }
        
        [PublicAPI]
        public struct PageViewsQueryResults {
            public Stream Data { get; }
            public (string Code, string Message)[] Warnings { get; }

            private PageViewsQueryResults(Stream data, (string Code, string Message)[] warnings) {
                Data = data;
                Warnings = warnings;
            }
            
            internal static PageViewsQueryResults From(AsyncQueryStatusResponseModel status, Stream data) {
                return new PageViewsQueryResults(data, status.Warnings?.Select(w => (w.Code, w.Message)).ToArray() ?? Array.Empty<(string, string)>());
            }
        }

        private async Task<PageViewsQueryResults> AwaitPageViewsQueryCompletion(AsyncQueryResponseModel query, int pollingDelayMs, bool bufferResponseContent) {
            for (;;) {
                var status = await PollPageViewsQuery(query.PollUrl);

                switch (status.Status.ToApiRepresentedEnum<PageViewsQueryStatus>()) {
                    case PageViewsQueryStatus.Running:
                    case PageViewsQueryStatus.Queued:
                        await Task.Delay(pollingDelayMs);
                        break;
                    case PageViewsQueryStatus.Finished:
                        return PageViewsQueryResults.From(status, await DownloadPageViewsResults(status.ResultsUrl, bufferResponseContent));
                    case PageViewsQueryStatus.Failed:
                        throw new CommunicationException(status.ErrorCode ?? "Query failed with no error code."); 
                    default:
                        throw new CommunicationException($"Encountered unexpected status: {status.Status}.");
                }
            }
        }

        private async Task<Stream> DownloadPageViewsResults(string resultsUrl, bool bufferResponseContent) {
            if (bufferResponseContent) {
                using var response = await client.GetAsync(resultsUrl);
                response.AssertSuccess();
                var data = await response.Content.ReadAsByteArrayAsync();
                return await DecompressAndStream(data);
            }

            var streamingResponse = await client.GetAsync(resultsUrl, HttpCompletionOption.ResponseHeadersRead);
            streamingResponse.AssertSuccess();

            var source = await streamingResponse.Content.ReadAsStreamAsync();
            var prefix = new byte[2];
            var prefixLength = await ReadPrefixAsync(source, prefix);
            Stream stream = new PrefixedReadStream(prefix, prefixLength, source);

            if (prefixLength == prefix.Length && IsGzipPayload(prefix)) {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            return stream;
        }

        private static async Task<int> ReadPrefixAsync(Stream stream, byte[] prefix) {
            var bytesRead = 0;
            while (bytesRead < prefix.Length) {
                var read = await stream.ReadAsync(prefix, bytesRead, prefix.Length - bytesRead);
                if (read == 0) {
                    break;
                }

                bytesRead += read;
            }

            return bytesRead;
        }
        
        private static bool IsGzipPayload(IReadOnlyList<byte> payload) {
            return payload.Count >= 2 && payload[0] == 0x1f && payload[1] == 0x8b;
        }

        private static async Task<Stream> DecompressAndStream(byte[] payload) {
            if (IsGzipPayload(payload)) {
                using var compressed = new MemoryStream(payload);
                using var decompressed = new GZipStream(compressed, CompressionMode.Decompress);
                var decompressedPayload = new MemoryStream();
                await decompressed.CopyToAsync(decompressedPayload);
                decompressedPayload.Position = 0;
                return decompressedPayload;
            } else {
                return new MemoryStream(payload);
            }
        }

        /// <summary>
        /// Runs an asynchronous Page Views query for a user and returns the resulting data stream.
        /// </summary>
        /// <param name="userId">The Canvas user id.</param>
        /// <param name="startDate">The inclusive start month. Must be the first day of a month.</param>
        /// <param name="endDate">The exclusive end month. Must be the first day of a month and after <paramref name="startDate"/>.</param>
        /// <param name="format">The output format for the results.</param>
        /// <param name="pollingDelayMs">Delay in milliseconds between poll requests while the query is running.</param>
        /// <param name="bufferResponseContent">
        /// If set to <c>false</c>, this avoids an intermediate buffer and can improve performance.
        /// However, slow consumers can cause Canvas to time out the response stream (for example, printing to stdout).
        /// Writing the stream to a file is usually safe.
        /// If set to <c>true</c>, the response is fully buffered in memory before returning and is always safe.
        /// </param>
        /// <returns>A stream containing the query output in the requested format.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when either date is not on the first of the month, or when <paramref name="startDate"/> is not before <paramref name="endDate"/>.
        /// </exception>
        /// <exception cref="CommunicationException">Thrown when the Canvas async query fails or returns an unexpected status.</exception>
        public async Task<PageViewsQueryResults> QueryPageViews(ulong userId, 
                                                                DateTime startDate, 
                                                                DateTime endDate, 
                                                                PageViewsFormat format,
                                                                int pollingDelayMs = 5000,
                                                                bool bufferResponseContent = false) {
            if (startDate.Day != 1 || endDate.Day != 1) {
                throw new ArgumentException("startDate and endDate must be on the first of the month");
            }
            
            if (startDate >= endDate) {
                throw new ArgumentException("startDate must be before endDate");
            }
            
            var query = await InitiatePageViewsQuery(userId, startDate, endDate, format);
            return await AwaitPageViewsQueryCompletion(query, pollingDelayMs, bufferResponseContent);
        }

        /// <summary>
        /// Runs an asynchronous Page Views query for multiple users and returns the resulting data stream.
        /// </summary>
        /// <param name="userIds">The Canvas user ids.</param>
        /// <param name="startDate">The inclusive start month. Must be the first day of a month.</param>
        /// <param name="endDate">The exclusive end month. Must be the first day of a month and after <paramref name="startDate"/>.</param>
        /// <param name="format">The output format for the results.</param>
        /// <param name="pollingDelayMs">Delay in milliseconds between poll requests while the query is running.</param>
        /// <param name="bufferResponseContent">
        /// If set to <c>false</c>, this avoids an intermediate buffer and can improve performance.
        /// However, slow consumers can cause Canvas to time out the response stream (for example, printing to stdout).
        /// Writing the stream to a file is usually safe.
        /// If set to <c>true</c>, the response is fully buffered in memory before returning and is always safe.
        /// </param>
        /// <returns>A stream containing the query output in the requested format.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when either date is not on the first of the month, or when <paramref name="startDate"/> is not before <paramref name="endDate"/>.
        /// </exception>
        /// <exception cref="CommunicationException">Thrown when the Canvas async query fails or returns an unexpected status.</exception>
        public async Task<PageViewsQueryResults> QueryPageViews(IEnumerable<ulong> userIds,
                                                                DateTime startDate,
                                                                DateTime endDate,
                                                                PageViewsFormat format,
                                                                int pollingDelayMs = 5000,
                                                                bool bufferResponseContent = false) {
            var query = await InitiateBatchPageViewsQuery(userIds, startDate, endDate, format);
            return await AwaitPageViewsQueryCompletion(query, pollingDelayMs, bufferResponseContent);
        }
    }
}
