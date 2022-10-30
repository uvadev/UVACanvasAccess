using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Discussions {
    
    /// <summary>
    /// Represents a discussion topic.
    /// </summary>
    [PublicAPI]
    public class DiscussionTopic : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// Which type of entity the discussion belongs to.
        /// </summary>
        public DiscussionHome Home { get; }
        
        /// <summary>
        /// The id of the <see cref="Home">entity</see> the discussion belongs to.
        /// </summary>
        public ulong HomeId { get; }
        
        /// <summary>
        /// The discussion id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The discussion title.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// The content of the discussion topic.
        /// </summary>
        public string Message { get; }
        
        /// <summary>
        /// The webpage url of the discussion topic.
        /// </summary>
        public string HtmlUrl { get; }
        
        /// <summary>
        /// When the topic was posted.
        /// </summary>
        public DateTime? PostedAt { get; }
        
        /// <summary>
        /// When the last reply was made to the topic.
        /// </summary>
        public DateTime? LastReplyAt { get; }
        
        /// <summary>
        /// Whether a user must first post a reply before being allowed to respond to other replies.
        /// </summary>
        public bool? RequireInitialPost { get; }
        
        /// <summary>
        /// Whether a user can see other replies.
        /// </summary>
        public bool? UserCanSeePosts { get; }
        
        /// <summary>
        /// The amount of entries in the topic.
        /// </summary>
        public uint? DiscussionSubentryCount { get; }
        
        /// <summary>
        /// The read state of the topic for the current user.
        /// </summary>
        public string ReadState { get; }
        
        /// <summary>
        /// The amount of unread entries in the topic for the current user.
        /// </summary>
        public uint? UnreadCount { get; }
        
        /// <summary>
        /// Whether the user is subscribed to the topic.
        /// </summary>
        public bool? Subscribed { get; }
        
        /// <summary>
        /// If the user is forbidden from subscribing to the topic, the reason.
        /// </summary>
        public string SubscriptionHold { get; }
        
        /// <summary>
        /// If this topic is for grading an assignment, the assignment id.
        /// </summary>
        public ulong? AssignmentId { get; }
        
        /// <summary>
        /// If the topic is set to be posted after a delay, when the topic will be posted.
        /// </summary>
        public DateTime? DelayedPostAt { get; }
        
        /// <summary>
        /// Whether the topic is published.
        /// </summary>
        public bool? Published { get; }
        
        /// <summary>
        /// When the topic will lock.
        /// </summary>
        public DateTime? LockAt { get; }
        
        /// <summary>
        /// Whether the topic is locked.
        /// </summary>
        public bool? Locked { get; }
        
        /// <summary>
        /// Whether the topic is pinned.
        /// </summary>
        public bool? Pinned { get; }
        
        /// <summary>
        /// Whether the topic is locked for the current user.
        /// </summary>
        public bool? LockedForUser { get; }
        
        /// <summary>
        /// Present if the topic is locked for the current user.
        /// </summary>
        [Enigmatic]
        public object LockInfo { get; }
        
        /// <summary>
        /// If the topic is locked for the current user, the reason.
        /// </summary>
        public string LockExplanation { get; }
        
        /// <summary>
        /// The name of the topic creator.
        /// </summary>
        public string UserName { get; }
        
        public IEnumerable<uint> TopicChildren { get; }
        
        public object GroupTopicChildren { get; }
        
        /// <summary>
        /// The root topic id, if a root topic exists.
        /// </summary>
        public ulong? RootTopicId { get; }
        
        /// <summary>
        /// If the topic is a podcast, the podcast url.
        /// </summary>
        public string PodcastUrl { get; }
        
        /// <summary>
        /// The discussion type, i. e. whether replies may themselves have nested replies.
        /// </summary>
        public DiscussionType? DiscussionType { get; }
        
        /// <summary>
        /// If this is a group discussion, the group category id.
        /// </summary>
        public ulong? GroupCategoryId { get; }
        
        /// <summary>
        /// Any attachments to the discussion.
        /// </summary>
        public IEnumerable<FileAttachment> Attachments { get; }
        
        /// <summary>
        /// The current user's permissions for the discussion.
        /// </summary>
        public Dictionary<string, bool> Permissions { get; }
        
        /// <summary>
        /// Whether users can rate replies.
        /// </summary>
        public bool? AllowRating { get; }
        
        /// <summary>
        /// Whether rating is restricted to graders.
        /// </summary>
        public bool? OnlyGradersCanRate { get; }
        
        /// <summary>
        /// Whether replies should be sorted by rating.
        /// </summary>
        public bool? SortByRating { get; }
        
        /// <summary>
        /// The author of the topic.
        /// </summary>
        [CanBeNull]
        public UserDisplay Author { get; set; }

        /// <summary>
        /// Gets the list of entries for this topic.
        /// </summary>
        /// <returns>The list of entries.</returns>
        public Task<IEnumerable<TopicEntry>> GetEntries() {
            return Home switch {
                DiscussionHome.Course => api.ListCourseDiscussionTopicEntries(HomeId, Id),
                DiscussionHome.Group => api.ListGroupDiscussionTopicEntries(HomeId, Id),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        internal DiscussionTopic(Api api, DiscussionTopicModel model, string home, ulong homeId)
            : this(api, model, home == "courses" ? DiscussionHome.Course : DiscussionHome.Group, homeId) { }

        internal DiscussionTopic(Api api, DiscussionTopicModel model, DiscussionHome home, ulong homeId) {
            this.api = api;
            Home = home;
            HomeId = homeId;
            Id = model.Id;
            Title = model.Title;
            Message = model.Message;
            HtmlUrl = model.HtmlUrl;
            PostedAt = model.PostedAt;
            LastReplyAt = model.LastReplyAt;
            RequireInitialPost = model.RequireInitialPost;
            UserCanSeePosts = model.UserCanSeePosts;
            DiscussionSubentryCount = model.DiscussionSubentryCount;
            ReadState = model.ReadState;
            UnreadCount = model.UnreadCount;
            Subscribed = model.Subscribed;
            SubscriptionHold = model.SubscriptionHold;
            AssignmentId = model.AssignmentId;
            DelayedPostAt = model.DelayedPostAt;
            Published = model.Published;
            LockAt = model.LockAt;
            Locked = model.Locked;
            Pinned = model.Pinned;
            LockedForUser = model.LockedForUser;
            LockInfo = model.LockInfo;
            LockExplanation = model.LockExplanation;
            UserName = model.UserName;
            TopicChildren = model.TopicChildren;
            GroupTopicChildren = model.GroupTopicChildren;
            RootTopicId = model.RootTopicId;
            PodcastUrl = model.PodcastUrl;
            DiscussionType = model.DiscussionType?.ToApiRepresentedEnum<DiscussionType>();
            GroupCategoryId = model.GroupCategoryId;
            Attachments = model.Attachments.Select(a => new FileAttachment(api, a));
            Permissions = model.Permissions;
            AllowRating = model.AllowRating;
            OnlyGradersCanRate = model.OnlyGradersCanRate;
            SortByRating = model.SortByRating;
            
            if (model.Author is { Id: { } }) { // Author might be empty instead of null
                Author = new UserDisplay(api, model.Author);
            }
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "DiscussionTopic {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(Message)}: {Message}," +
                   $"\n{nameof(HtmlUrl)}: {HtmlUrl}," +
                   $"\n{nameof(PostedAt)}: {PostedAt}," +
                   $"\n{nameof(LastReplyAt)}: {LastReplyAt}," +
                   $"\n{nameof(RequireInitialPost)}: {RequireInitialPost}," +
                   $"\n{nameof(UserCanSeePosts)}: {UserCanSeePosts}," +
                   $"\n{nameof(DiscussionSubentryCount)}: {DiscussionSubentryCount}," +
                   $"\n{nameof(ReadState)}: {ReadState}," +
                   $"\n{nameof(UnreadCount)}: {UnreadCount}," +
                   $"\n{nameof(Subscribed)}: {Subscribed}," +
                   $"\n{nameof(SubscriptionHold)}: {SubscriptionHold}," +
                   $"\n{nameof(AssignmentId)}: {AssignmentId}," +
                   $"\n{nameof(DelayedPostAt)}: {DelayedPostAt}," +
                   $"\n{nameof(Published)}: {Published}," +
                   $"\n{nameof(LockAt)}: {LockAt}," +
                   $"\n{nameof(Locked)}: {Locked}," +
                   $"\n{nameof(Pinned)}: {Pinned}," +
                   $"\n{nameof(LockedForUser)}: {LockedForUser}," +
                   $"\n{nameof(LockInfo)}: {LockInfo}," +
                   $"\n{nameof(LockExplanation)}: {LockExplanation}," +
                   $"\n{nameof(UserName)}: {UserName}," +
                   $"\n{nameof(TopicChildren)}: {TopicChildren.ToPrettyString()}," +
                   $"\n{nameof(GroupTopicChildren)}: {GroupTopicChildren}," +
                   $"\n{nameof(RootTopicId)}: {RootTopicId}," +
                   $"\n{nameof(PodcastUrl)}: {PodcastUrl}," +
                   $"\n{nameof(DiscussionType)}: {DiscussionType}," +
                   $"\n{nameof(GroupCategoryId)}: {GroupCategoryId}," +
                   $"\n{nameof(Attachments)}: {Attachments.ToPrettyString()}," +
                   $"\n{nameof(Permissions)}: {Permissions.ToPrettyString()}," +
                   $"\n{nameof(AllowRating)}: {AllowRating}," +
                   $"\n{nameof(OnlyGradersCanRate)}: {OnlyGradersCanRate}," +
                   $"\n{nameof(SortByRating)}: {SortByRating}," +
                   $"\n{nameof(Author)}: {Author?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// The types of entities a <see cref="DiscussionTopic"/> can belong to.
    /// </summary>
    [PublicAPI]
    public enum DiscussionHome {
        /// <summary>
        /// The discussion belongs to a <see cref="Structures.Courses.Course"/>.
        /// </summary>
        Course,
        /// <summary>
        /// The discussion belongs to a <see cref="Structures.Groups.Group"/>
        /// </summary>
        Group
    }

    /// <summary>
    /// The types of discussion.
    /// </summary>
    [PublicAPI]
    public enum DiscussionType {
        /// <summary>
        /// The discussion only allows single replies.
        /// </summary>
        [ApiRepresentation("side_comment")]
        SideComment,
        /// <summary>
        /// Replies to the discussion may themselves have nested replies.
        /// </summary>
        [ApiRepresentation("threaded")]
        Threaded
    }
}