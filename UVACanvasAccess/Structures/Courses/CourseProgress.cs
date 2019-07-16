using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    [PublicAPI]
    public class CourseProgress : IPrettyPrint {
        private readonly Api _api;
        
        public uint? RequirementCount { get; }
        
        public uint? RequirementCompletedCount { get; }
        
        [CanBeNull]
        public string NextRequirementUrl { get; }
        
        public DateTime? CompletedAt { get; }

        internal CourseProgress(Api api, CourseProgressModel model) {
            _api = api;
            RequirementCount = model.RequirementCount;
            RequirementCompletedCount = model.RequirementCompletedCount;
            NextRequirementUrl = model.NextRequirementUrl;
            CompletedAt = model.CompletedAt;
        }

        public string ToPrettyString() {
            return "CourseProgress {" +
                   ($"\n{nameof(RequirementCount)}: {RequirementCount}," +
                   $"\n{nameof(RequirementCompletedCount)}: {RequirementCompletedCount}," +
                   $"\n{nameof(NextRequirementUrl)}: {NextRequirementUrl}," +
                   $"\n{nameof(CompletedAt)}: {CompletedAt}").Indent(4) + 
                   "\n}";
        }
    }
}