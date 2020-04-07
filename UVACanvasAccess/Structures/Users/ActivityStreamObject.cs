using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Assignments;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Structures.Submissions;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    [PublicAPI]
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

        internal ActivityStreamObject(Api api, ActivityStreamObjectModel model) {
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

        internal static ActivityStreamObject FromModel(Api api, ActivityStreamObjectModel model) {
            return model.Type switch {
                "DiscussionTopic"   => (ActivityStreamObject) new DiscussionTopic(api, model),
                "Announcement"      => new Announcement(api, model),
                "Conversation"      => new Conversation(api, model),
                "Message"           => new Message(api, model),
                "Conference"        => new Conference(api, model),
                "Collaboration"     => new Collaboration(api, model),
                "AssignmentRequest" => new AssignmentRequest(api, model),
                "Submission"        => new SubmissionObject(api, model),
                _                   => throw new NotImplementedException("unknown ActivityStreamObject type " + model.Type)
            };
        }

        public class SubmissionObject : ActivityStreamObject {
            
            public ulong? AssignmentId { get; }
        
            [CanBeNull]
            public Assignment Assignment { get; }
        
            [CanBeNull]
            public Course Course { get; }

            public uint? Attempt { get; }
        
            [CanBeNull]
            public string Body { get; }

            public string Grade { get; }

            public bool? GradeMatchesCurrentSubmission { get; }

            public string PreviewUrl { get; }

            public decimal? Score { get; }
        
            [CanBeNull]
            public IEnumerable<SubmissionComment> SubmissionComments { get; }

            public string SubmissionType { get; }
        
            public DateTime? SubmittedAt { get; }
        
            [CanBeNull]
            public string Url { get; }

            public ulong? UserId { get; }

            [Enigmatic]
            public long? GraderId { get; }
        
            public DateTime? GradedAt { get; }

            public User User { get; }

            public bool? Late { get; }

            public bool? AssignmentVisible { get; }

            public bool? Excused { get; }
        
            public bool? Missing { get; }

            public string LatePolicyStatus { get; }

            public double? PointsDeducted { get; }
        
            public double? SecondsLate { get; }

            public string WorkflowState { get; }
        
            public uint? ExtraAttempts { get; }
        
            [CanBeNull]
            public string AnonymousId { get; }
            
            internal SubmissionObject(Api api, ActivityStreamObjectModel model) : base(api, model) {
                //Debug.Assert(model.AssignmentId != null, "model.AssignmentId != null");
                //Debug.Assert(model.UserId != null, "model.UserId != null");

                AssignmentId = model.AssignmentId;
                Assignment = model.Assignment.ConvertIfNotNull(m => new Assignment(api, m));
                Course = model.Course.ConvertIfNotNull(c => new Course(api, c));
                Attempt = model.Attempt;
                Body = model.Body;
                Grade = model.Grade;
                GradeMatchesCurrentSubmission = model.GradeMatchesCurrentSubmission;
                PreviewUrl = model.PreviewUrl;
                Score = model.Score;
                SubmissionComments = model.SubmissionComments.ConvertIfNotNull(ie => ie.Select(m => new SubmissionComment(api, m)));
                SubmissionType = model.SubmissionType;
                SubmittedAt = model.SubmittedAt;
                Url = model.Url;
                UserId = model.UserId;
                GraderId = model.GraderId;
                GradedAt = model.GradedAt;
                User = model.User.ConvertIfNotNull(m => new User(api, m));
                Late = model.Late;
                AssignmentVisible = model.AssignmentVisible;
                Excused = model.Excused;
                Missing = model.Missing;
                LatePolicyStatus = model.LatePolicyStatus;
                PointsDeducted = model.PointsDeducted;
                SecondsLate = model.SecondsLate;
                WorkflowState = model.WorkflowState;
                ExtraAttempts = model.ExtraAttempts;
                AnonymousId = model.AnonymousId;
            }
        }

        public class DiscussionTopic : ActivityStreamObject {
            
            public uint? TotalRootDiscussionEntries { get; }
        
            public bool? RequireInitialPost { get; }
        
            public bool? UserHasPosted { get; }
        
            public object RootDiscussionEntries { get; } // todo this class/model
            
            public ulong DiscussionTopicId { get; }
            
            internal DiscussionTopic(Api api, ActivityStreamObjectModel model) : base(api, model) {
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
            
            internal Announcement(Api api, ActivityStreamObjectModel model) : base(api, model) {
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

            internal Conversation(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.ConversationId != null, "model.ConversationId != null");
                
                ConversationId = (ulong) model.ConversationId;
                Private = model.Private;
                ParticipantCount = model.ParticipantCount;
            }
        }

        public class Message : ActivityStreamObject {
            
            public ulong? MessageId { get; }
        
            public string NotificationCategory { get; }

            internal Message(Api api, ActivityStreamObjectModel model) : base(api, model) {
                MessageId = model.MessageId;
                NotificationCategory = model.NotificationCategory;
            }
        }

        public class Conference : ActivityStreamObject {
            
            public ulong WebConferenceId { get; }

            internal Conference(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.WebConferenceId != null, "model.WebConferenceId != null");
                
                WebConferenceId = (ulong) model.WebConferenceId;
            }
        }

        public class Collaboration : ActivityStreamObject {
            
            public ulong CollaborationId { get; }

            internal Collaboration(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.CollaborationId != null, "model.CollaborationId != null");
                
                CollaborationId = (ulong) model.CollaborationId;
            }
        }

        public class AssignmentRequest : ActivityStreamObject {

            public ulong AssignmentRequestId { get; }
            
            internal AssignmentRequest(Api api, ActivityStreamObjectModel model) : base(api, model) {
                Debug.Assert(model.AssignmentRequestId != null, "model.AssignmentRequestId != null");
                
                AssignmentRequestId = (ulong) model.AssignmentRequestId;
            }
        }
    }
}