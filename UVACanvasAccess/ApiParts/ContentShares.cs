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
    }
}
