using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    [PublicAPI]
    public struct UserParticipation : IPrettyPrint {
        public Dictionary<DateTime, ulong> PageViews { get; }
        public IEnumerable<UserParticipationEvent> Participations { get; }

        internal UserParticipation(UserParticipationModel model) {
            PageViews = model.PageViews;
            Participations = model.Participations.SelectNotNull(p => new UserParticipationEvent(p));
        }

        public string ToPrettyString() {
            return "UserParticipation {" + 
                   ($"\n{nameof(PageViews)}: {PageViews.ToPrettyString()}," +
                   $"\n{nameof(Participations)}: {Participations.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
    
    [PublicAPI]
    public struct UserParticipationEvent : IPrettyPrint {
        public DateTime CreatedAt { get; }
        public string Url { get; }

        internal UserParticipationEvent(UserParticipationEventModel model) {
            CreatedAt = model.CreatedAt;
            Url = model.Url;
        }

        public string ToPrettyString() {
            return "UserParticipationEvent {" + 
                   ($"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(Url)}: {Url}").Indent(4) + 
                   "\n}";
        }
    }
}
