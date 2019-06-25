using System;
using System.Diagnostics;
using StatePrinting;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Structures.Users {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public abstract class ActivityStreamObject {
        protected readonly Api Api;
        
        public DateTime CreatedAt { get; }
        
        public DateTime? UpdatedAt { get; }
        
        public ulong Id { get; }

        public string Title { get; }

        public string BodyMessage { get; }
        
        public string Type { get; }
        
        public bool ReadState { get; }
        
        public string ContextType { get; }
        
        public ulong? CourseId { get; }
        
        public ulong? GroupId { get; }
        
        public string HtmlUrl { get; }

        protected ActivityStreamObject(Api api, ActivityStreamObjectModel model) {
            Api = api;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            Id = model.Id;
            Title = model.Title;
            BodyMessage = model.Message;
            Type = model.Type;
            ReadState = model.ReadState;
            ContextType = model.ContextType;
            CourseId = model.CourseId;
            GroupId = model.GroupId;
            HtmlUrl = model.HtmlUrl;
        }

        public static ActivityStreamObject FromModel(Api api, ActivityStreamObjectModel model) {
            switch (model.Type) {
                case "DiscussionTopic":
                    return new DiscussionTopic(api, model);
                case "Announcement":
                    return new Announcement(api, model);
                case "Conversation":
                    return new Conversation(api, model);
                case "Message":
                    return new Message(api, model);
                case "Conference":
                    return new Conference(api, model);
                case "Collaboration":
                    return new Collaboration(api, model);
                case "AssignmentRequest":
                    return new AssignmentRequest(api, model);
                default:
                    throw new NotImplementedException("unknown ActivityStreamObject type " + model.Type);
            }
        }
        
        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }

        public class DiscussionTopic : ActivityStreamObject {
            
            public uint? TotalRootDiscussionEntries { get; }
        
            public bool? RequireInitialPost { get; }
        
            public bool? UserHasPosted { get; }
        
            public object RootDiscussionEntries { get; } // todo this class/model
            
            public ulong DiscussionTopicId { get; }
            
            public DiscussionTopic(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.DiscussionTopicId != null, "model.DiscussionTopicId != null");
                
                TotalRootDiscussionEntries = model.TotalRootDiscussionEntries;
                RequireInitialPost = model.RequireInitialPost;
                UserHasPosted = model.UserHasPosted;
                RootDiscussionEntries = model.RootDiscussionEntries;
                DiscussionTopicId = (ulong) model.DiscussionTopicId;
            }
        }

        public class Announcement : ActivityStreamObject {

            public uint? TotalRootDiscussionEntries { get; }
        
            public bool? RequireInitialPost { get; }
        
            public bool? UserHasPosted { get; }
        
            public object RootDiscussionEntries { get; } // todo this class/model
            
            public ulong AnnouncementId { get; }
            
            public Announcement(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.AnnouncementId != null, "model.AnnouncementId != null");
                
                TotalRootDiscussionEntries = model.TotalRootDiscussionEntries;
                RequireInitialPost = model.RequireInitialPost;
                UserHasPosted = model.UserHasPosted;
                RootDiscussionEntries = model.RootDiscussionEntries;
                AnnouncementId = (ulong) model.AnnouncementId;
            }
        }

        public class Conversation : ActivityStreamObject {
            
            public ulong ConversationId { get; }
        
            public bool? Private { get; } 
        
            public uint? ParticipantCount { get; }

            public Conversation(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.ConversationId != null, "model.ConversationId != null");
                
                ConversationId = (ulong) model.ConversationId;
                Private = model.Private;
                ParticipantCount = model.ParticipantCount;
            }
        }

        public class Message : ActivityStreamObject {
            
            public ulong MessageId { get; }
        
            public string NotificationCategory { get; }

            public Message(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.MessageId != null, "model.MessageId != null");
                
                MessageId = (ulong) model.MessageId;
                NotificationCategory = model.NotificationCategory;
            }
        }

        public class Conference : ActivityStreamObject {
            
            public ulong WebConferenceId { get; }

            public Conference(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.WebConferenceId != null, "model.WebConferenceId != null");
                
                WebConferenceId = (ulong) model.WebConferenceId;
            }
        }

        public class Collaboration : ActivityStreamObject {
            
            public ulong CollaborationId { get; }

            public Collaboration(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.CollaborationId != null, "model.CollaborationId != null");
                
                CollaborationId = (ulong) model.CollaborationId;
            }
        }

        public class AssignmentRequest : ActivityStreamObject {

            public ulong AssignmentRequestId { get; }
            
            public AssignmentRequest(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.AssignmentRequestId != null, "model.AssignmentRequestId != null");
                
                AssignmentRequestId = (ulong) model.AssignmentRequestId;
            }
        }
    }
}