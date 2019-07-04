using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class AssignmentOverride : IPrettyPrint {
        private readonly Api _api;

        public ulong Id { get; }

        public ulong AssignmentId { get; }

        [CanBeNull] public IEnumerable<ulong> StudentIds { get; }

        public ulong? GroupId { get; }

        public ulong CourseSectionId { get; }

        public string Title { get; }

        public DateTime? DueAt { get; }

        public bool? AllDay { get; }

        public DateTime? AllDayDate { get; }

        public DateTime? UnlockAt { get; }

        public DateTime? LockAt { get; }

        public AssignmentOverride(Api api, AssignmentOverrideModel model) {
            _api = api;
            Id = model.Id;
            AssignmentId = model.AssignmentId;
            StudentIds = model.StudentIds;
            GroupId = model.GroupId;
            CourseSectionId = model.CourseSectionId;
            Title = model.Title;
            DueAt = model.DueAt;
            AllDay = model.AllDay;
            AllDayDate = model.AllDayDate;
            UnlockAt = model.UnlockAt;
            LockAt = model.LockAt;
        }

        public string ToPrettyString() {
            return "AssignmentOverride {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(AssignmentId)}: {AssignmentId}," +
                   $"\n{nameof(StudentIds)}: {StudentIds.ToPrettyString()}," +
                   $"\n{nameof(GroupId)}: {GroupId}," +
                   $"\n{nameof(CourseSectionId)}: {CourseSectionId}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(DueAt)}: {DueAt}," +
                   $"\n{nameof(AllDay)}: {AllDay}," +
                   $"\n{nameof(AllDayDate)}: {AllDayDate}," +
                   $"\n{nameof(UnlockAt)}: {UnlockAt}," +
                   $"\n{nameof(LockAt)}: {LockAt}").Indent(4) +
                   "\n}";
        }
    }
}