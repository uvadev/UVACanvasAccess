using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Model.Conversations;
using UVACanvasAccess.Structures;
using UVACanvasAccess.Structures.Conversations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        /// <summary>
        /// Creates and streams one or more conversations.
        /// <br />
        /// When creating a private conversation, if a private conversation already exists between the current user
        /// and the recipient, the existing one will be reused, unless <paramref name="forceNew"/> is true.
        /// </summary>
        /// <param name="recipients">The recipients of the created conversations(s).</param>
        /// <param name="body">The message to send.</param>
        /// <param name="subject">(Optional) The subject of the conversation. This is ignored when a conversation is reused.</param>
        /// <param name="forceNew">(Optional) If true, prevents conversations from being reused.</param>
        /// <param name="groupConversation">(Optional) If true, a group conversation will be created with all recipients.</param>
        /// <param name="attachmentIds">(Optional) Attachment ids. Attachments must have been uploaded to the current user's attachments folder.</param>
        /// <param name="mediaCommentId">(Optional) Media comment id.</param>
        /// <param name="mediaCommentType">(Optional) Media comment type.</param>
        /// <param name="addJournalEntry">(Optional) If true, a faculty journal entry will be created to record this conversation.</param>
        /// <param name="context">(Optional) The context of this conversation (???).</param>
        /// <returns>The created conversations.</returns>
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

        /// <summary>
        /// Streams the conversations visible to the current user.
        /// </summary>
        /// <param name="readState">(Optional) The read state to filter by.</param>
        /// <param name="filter">(Optional) The qualified ids to filter by.</param>
        /// <param name="filterIntersection">(Optional) If true, the predicate in <paramref name="filter"/> is an AND instead of an OR.</param>
        /// <param name="includeParticipantAvatars">(Optional) If true, include the participant avatars in the result.</param>
        /// <returns>The stream of conversations.</returns>
        public async IAsyncEnumerable<Conversation> StreamConversations(ConversationReadState? readState = null,
                                                                        IEnumerable<QualifiedId> filter = null,
                                                                        bool filterIntersection = false,
                                                                        bool includeParticipantAvatars = false) {
            var args = new List<(string, string)> {
                ("scope", readState?.GetApiRepresentation()),
                ("filter_mode", filterIntersection ? "and" : "or")
            };

            if (includeParticipantAvatars) {
                args.Add(("include[]", "participant_avatars"));
            }

            if (filter != null) {
                args.AddRange(filter.Select(id => ("filter[]", id.AsString)));
            }

            var response = await _client.GetAsync("conversations" + BuildDuplicateKeyQueryString(args.ToArray()));
            
            await foreach (var conversation in StreamDeserializePages<ConversationModel>(response)) {
                yield return new Conversation(this, conversation);
            }
        }

        /// <summary>
        /// Gets a single conversation by id.
        /// </summary>
        /// <param name="conversationId">The conversation id.</param>
        /// <param name="markAsRead">(Optional, default false) If true, mark the conversation as read.</param>
        /// <exception cref="DoesNotExistException">If the conversation with the given id does not exist or is not visible to the current user.</exception>
        /// <returns>The conversation.</returns>
        public async Task<DetailedConversation> GetConversation(ulong conversationId, bool markAsRead = false) {
            var args = new List<(string, string)> {
                ("auto_mark_as_read", markAsRead.ToShortString())
            };

            var response = await _client.GetAsync($"conversations/{conversationId}" + BuildDuplicateKeyQueryString(args.ToArray()));

            try {
                return new DetailedConversation(this, JsonConvert.DeserializeObject<DetailedConversationModel>(await response.Content.ReadAsStringAsync()));
            } catch (InvalidOperationException) {
                throw new DoesNotExistException($"Conversation with id {conversationId} does not exist or is not visible to the current user.");
            }
        }

        /// <summary>
        /// Edits the current user's view of a conversation by id.
        /// </summary>
        /// <param name="conversationId">The conversation id.</param>
        /// <param name="readState">(Optional) Changes the read state of the conversation.</param>
        /// <param name="subscribed">(Optional) Changes the subscribed state of this conversation.</param>
        /// <param name="starred">(Optional) Changes the starred state of this conversation.</param>
        /// <returns>The edited conversation.</returns>
        public async Task<Conversation> EditConversation(ulong conversationId, 
                                                         ConversationReadState? readState = null,
                                                         bool? subscribed = null,
                                                         bool? starred = null) {
            var args = new List<(string, string)> {
                ("conversation[workflow_state]", readState?.GetApiRepresentation()),
                ("conversation[subscribed]", subscribed?.ToShortString()),
                ("conversation[starred]", starred?.ToShortString())
            };

            var response = await _client.PutAsync($"conversations/{conversationId}", BuildHttpArguments(args));
            
            try {
                return new Conversation(this, JsonConvert.DeserializeObject<ConversationModel>(await response.Content.ReadAsStringAsync()));
            } catch (InvalidOperationException) {
                throw new DoesNotExistException($"Conversation with id {conversationId} does not exist or is not visible to the current user.");
            }
        }
    }
}
