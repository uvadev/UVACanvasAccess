using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// Participation statistics for one user in a course.
    /// </summary>
    [PublicAPI]
    public struct UserParticipation : IPrettyPrint {
        
        /// <summary>
        /// A history of page hit counts grouped by <see cref="DateTime">hour</see>.
        /// </summary>
        public Dictionary<DateTime, ulong> PageViews { get; }
        
        /// <summary>
        /// A history of all <see cref="UserParticipationEvent">page hits</see>.
        /// </summary>
        public IEnumerable<UserParticipationEvent> Participations { get; }

        internal UserParticipation(UserParticipationModel model) {
            PageViews = model.PageViews;
            Participations = model.Participations.SelectNotNull(p => new UserParticipationEvent(p));
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "UserParticipation {" + 
                   ($"\n{nameof(PageViews)}: {PageViews.ToPrettyString()}," +
                   $"\n{nameof(Participations)}: {Participations.ToPrettyString()}").Indent(4) + 
                   "\n}";
        }
    }
    
    /// <summary>
    /// A single page hit in a <see cref="UserParticipation"/>.
    /// </summary>
    [PublicAPI]
    public struct UserParticipationEvent : IPrettyPrint {
        
        /// <summary>
        /// When the page was viewed.
        /// </summary>
        public DateTime CreatedAt { get; }
        
        /// <summary>
        /// The page URL.
        /// </summary>
        public string Url { get; }

        internal UserParticipationEvent(UserParticipationEventModel model) {
            CreatedAt = model.CreatedAt;
            Url = model.Url;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "UserParticipationEvent {" + 
                   ($"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(Url)}: {Url}").Indent(4) + 
                   "\n}";
        }
    }
}
