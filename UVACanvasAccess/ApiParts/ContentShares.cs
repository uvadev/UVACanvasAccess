using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.ContentShares;
using UVACanvasAccess.Structures.ContentShares;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        /// <summary>
        /// Create a content share between one sender and one or more receivers.
        /// </summary>
        /// <param name="receivers">The list of receiver ids.</param>
        /// <param name="contentType">The type of the content being shared.</param>
        /// <param name="contentId">The id of the content being shared.</param>
        /// <param name="senderId">(Optional) The id of the sender. Self by default.</param>
        /// <returns>The created content share.</returns>
        public async Task<ContentShare> CreateContentShare([NotNull] IEnumerable<ulong> receivers,
                                                           ContentShareType contentType,
                                                           ulong contentId,
                                                           ulong? senderId = null) {
            var args = new List<(string, string)> {
                ("content_id", contentId.ToString()),
                ("content_type", contentType.GetApiRepresentation())
            };

            args.AddRange(receivers.Select(id => ("receiver_ids[]", id.ToString())));

            var response = await _client.PostAsync($"users/{senderId?.ToString() ?? "self"}/content_shares",
                                                   BuildHttpArguments(args));

            var model = JsonConvert.DeserializeObject<ContentShareModel>(await response.Content.ReadAsStringAsync());
            return ContentShare.NewContentShare(this, model);
        }

        /// <summary>
        /// Create a content share between one sender and one or more receivers.
        /// </summary>
        /// <param name="receivers">The list of receivers.</param>
        /// <param name="contentType">The type of the content being shared.</param>
        /// <param name="contentId">The id of the content being shared.</param>
        /// <param name="senderId">(Optional) The id of the sender. Self by default.</param>
        /// <returns>The created content share.</returns>
        public Task<ContentShare> CreateContentShare([NotNull] [ItemNotNull] IEnumerable<User> receivers, 
                                                     ContentShareType contentType, 
                                                     ulong contentId, 
                                                     ulong? senderId = null) {
            return CreateContentShare(receivers.Select(r => r.Id), contentType, contentId, senderId);
        }

        /// <summary>
        /// Stream the content shares for which some user is a sender.
        /// </summary>
        /// <param name="userId">(Optional) The id of the sender. Self by default.</param>
        /// <returns>The stream of shares.</returns>
        /// <remarks>
        /// If <paramref name="userId"/> is not Self, the current user must be an observer of the user, or an administrator.
        /// </remarks>
        public async IAsyncEnumerable<ContentShareWithReceivers> StreamSentContentShares(ulong? userId = null) {
            var response = await _client.GetAsync($"users/{userId?.ToString() ?? "self"}/content_shares/sent");

            await foreach (var model in StreamDeserializePages<ContentShareModel>(response)) {
                yield return new ContentShareWithReceivers(this, model);
            }
        }
        
        /// <summary>
        /// Stream the content shares for which some user is a sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <returns>The stream of shares.</returns>
        /// <remarks>
        /// The current user must be an observer of <paramref name="sender"/>, or an administrator.
        /// </remarks>
        public IAsyncEnumerable<ContentShareWithReceivers> StreamSentContentShares([NotNull] User sender) {
            return StreamSentContentShares(sender.Id);
        }
        
        /// <summary>
        /// Stream the content shares for which some user is a receiver.
        /// </summary>
        /// <param name="userId">(Optional) The id of the receiver. Self by default.</param>
        /// <returns>The stream of shares.</returns>
        /// <remarks>
        /// If <paramref name="userId"/> is not Self, the current user must be an observer of the user, or an administrator.
        /// </remarks>
        public async IAsyncEnumerable<ContentShareWithSender> StreamReceivedContentShares(ulong? userId = null) {
            var response = await _client.GetAsync($"users/{userId?.ToString() ?? "self"}/content_shares/received");

            await foreach (var model in StreamDeserializePages<ContentShareModel>(response)) {
                yield return new ContentShareWithSender(this, model);
            }
        }
        
        /// <summary>
        /// Stream the content shares for which some user is a receiver.
        /// </summary>
        /// <param name="receiver">The receiver.</param>
        /// <returns>The stream of shares.</returns>
        /// <remarks>
        /// The current user must be an observer of <paramref name="receiver"/>, or an administrator.
        /// </remarks>
        public IAsyncEnumerable<ContentShareWithSender> StreamReceivedContentShares([NotNull] User receiver) {
            return StreamReceivedContentShares(receiver.Id);
        }
    }
}
