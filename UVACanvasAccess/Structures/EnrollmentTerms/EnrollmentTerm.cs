using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.EnrollmentTerms;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Structures.EnrollmentTerms.EnrollmentTermWorkflowState;

namespace UVACanvasAccess.Structures.EnrollmentTerms {
    
    [PublicAPI]
    public class EnrollmentTerm : IPrettyPrint {
        private readonly Api api;
        
        public ulong Id { get; }
        
        public string SisTermId { get; }
        
        public ulong? SisImportId { get; }
        
        public string Name { get; }
        
        public DateTime? StartAt { get; }
        
        public DateTime? EndAt { get; }
        
        public EnrollmentTermWorkflowState WorkflowState { get; }
        
        public Dictionary<Api.CourseEnrollmentType, EnrollmentTermDateOverride> Overrides { get; }

        internal EnrollmentTerm(Api api, EnrollmentTermModel model) {
            this.api = api;
            Id = model.Id;
            SisTermId = model.SisTermId;
            SisImportId = model.SisImportId;
            Name = model.Name;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            WorkflowState = model.WorkflowState?.ToApiRepresentedEnum<EnrollmentTermWorkflowState>() ?? Unknown;
            Overrides = model.Overrides?.KeyValSelect(kv => (kv.Item1.ToApiRepresentedEnum<Api.CourseEnrollmentType>().Expect(),
                                                             new EnrollmentTermDateOverride(kv.Item2))) 
                        ?? new Dictionary<Api.CourseEnrollmentType, EnrollmentTermDateOverride>();
        }
        
        public string ToPrettyString() {
            return "EnrollmentTerm {" +
                   ($"\n{nameof(Id)}: {Id}, " +
                    $"\n{nameof(SisTermId)}: {SisTermId}, " +
                    $"\n{nameof(SisImportId)}: {SisImportId}, " +
                    $"\n{nameof(Name)}: {Name}, " +
                    $"\n{nameof(StartAt)}: {StartAt}, " +
                    $"\n{nameof(EndAt)}: {EndAt}, " +
                    $"\n{nameof(WorkflowState)}: {WorkflowState}, " +
                    $"\n{nameof(Overrides)}: {Overrides.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }

    [PublicAPI]
    public enum EnrollmentTermWorkflowState {
        [ApiRepresentation("active")]
        Active,
        [ApiRepresentation("deleted")]
        Deleted,
        Unknown
    }

    [PublicAPI]
    public struct EnrollmentTermDateOverride : IPrettyPrint {
        public DateTime? StartAt { get; }
        public DateTime? EndAt { get; }

        internal EnrollmentTermDateOverride(EnrollmentTermDateOverrideModel model) {
            StartAt = model.StartAt;
            EndAt = model.EndAt;
        }

        public EnrollmentTermDateOverride(DateTime? start, DateTime? end) {
            StartAt = start;
            EndAt = end;
        }

        public string ToPrettyString() {
            return "EnrollmentTermDateOverride {" +
                   ($"\n{nameof(StartAt)}: {StartAt}," +
                    $"\n{nameof(EndAt)}: {EndAt}").Indent(4) + 
                   "\n}";
        }
    }
}
