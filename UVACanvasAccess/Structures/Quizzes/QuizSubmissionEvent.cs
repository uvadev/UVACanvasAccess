using System;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz submission event.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionEvent : IPrettyPrint {
        private readonly Api api;
        
        public ulong Id { get; }
        
        public DateTime? CreatedAt { get; }
        
        public string EventType { get; }
        
        [CanBeNull]
        public JToken EventData { get; }

        internal QuizSubmissionEvent(Api api, QuizSubmissionEventModel model) {
            this.api = api;
            Id = model.Id;
            CreatedAt = model.CreatedAt;
            EventType = model.EventType;
            EventData = model.EventData;
        }

        public string ToPrettyString() {
            return "QuizSubmissionEvent {" +
                   ($"\n{nameof(Id)}: {Id}," +
                    $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                    $"\n{nameof(EventType)}: {EventType}," +
                    $"\n{nameof(EventData)}: {EventData}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// A quiz submission event definition used for posting events.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionEventInput {
        
        public string EventType { get; }
        
        [CanBeNull]
        public JToken EventData { get; }
        
        public DateTime? ClientTimestamp { get; }

        public QuizSubmissionEventInput(string eventType, [CanBeNull] JToken eventData = null, DateTime? clientTimestamp = null) {
            EventType = eventType;
            EventData = eventData;
            ClientTimestamp = clientTimestamp;
        }
    }
}
