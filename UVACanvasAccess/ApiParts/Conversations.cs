using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Structures;
using UVACanvasAccess.Structures.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        public async IAsyncEnumerable<Conversation> CreateConversation(IEnumerable<QualifiedId> recipients,
                                                           string body,
                                                           string subject = null,
                                                           bool? forceNew = null,
                                                           bool? groupConversation = null,
                                                           IEnumerable<string> attachmentIds = null,
                                                           string mediaCommentId = null,
                                                           string mediaCommentType = null,
                                                           bool? addJournalEntry = null,
                                                           QualifiedId? context = null) {
            var args = new List<(string, string)> {
                ("subject", subject),
                ("body", body),
                ("force_new", forceNew?.ToShortString()),
                ("group_conversation", groupConversation?.ToShortString()),
                ("media_comment_id", mediaCommentId),
                ("media_comment_type", mediaCommentType),
                ("user_note", addJournalEntry?.ToShortString()),
                ("context_code", context?.AsString)
            };
            
            args.AddRange(recipients.Select(id => ("recipients[]", id.AsString)));

            if (attachmentIds != null) {
                args.AddRange(attachmentIds.Select(attachment => ("attachment_ids[]", attachment)));
            }

            var response = await _client.PostAsync("conversations", BuildHttpArguments(args));

            await foreach (var conversation in StreamDeserializePages<ConversationModel>(response)) {
                yield return new Conversation(this, conversation);
            }
        }
        
    }
}
