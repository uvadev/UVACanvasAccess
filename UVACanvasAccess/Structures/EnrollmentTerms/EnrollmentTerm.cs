using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.EnrollmentTerms;
using UVACanvasAccess.Util;
using static UVACanvasAccess.Structures.EnrollmentTerms.EnrollmentTermWorkflowState;

namespace UVACanvasAccess.Structures.EnrollmentTerms {
    
    /// <summary>
    /// Represents an enrollment term, or simply term.
    /// </summary>
    [PublicAPI]
    public class EnrollmentTerm : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The term id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The term SIS id.
        /// </summary>
        public string SisTermId { get; }
        
        /// <summary>
        /// The SIS import id.
        /// </summary>
        public ulong? SisImportId { get; }
        
        /// <summary>
        /// The name of the term.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// When the term begins.
        /// </summary>
        public DateTime? StartAt { get; }
        
        /// <summary>
        /// When the term ends.
        /// </summary>
        public DateTime? EndAt { get; }
        
        /// <summary>
        /// The grading period group id.
        /// </summary>
        public ulong? GradingPeriodGroupId { get; }
        
        /// <summary>
        /// The state of the term.
        /// </summary>
        public EnrollmentTermWorkflowState WorkflowState { get; }
        
        /// <summary>
        /// Date overrides for specific enrollment types, if any.
        /// </summary>
        public Dictionary<Api.CourseEnrollmentType, EnrollmentTermDateOverride> Overrides { get; }

        internal EnrollmentTerm(Api api, EnrollmentTermModel model) {
            this.api = api;
            Id = model.Id;
            SisTermId = model.SisTermId;
            SisImportId = model.SisImportId;
            Name = model.Name;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
            GradingPeriodGroupId = model.GradingPeriodGroupId;
            WorkflowState = model.WorkflowState?.ToApiRepresentedEnum<EnrollmentTermWorkflowState>() ?? Unknown;
            Overrides = model.Overrides?.KeyValSelect(kv => (kv.Item1.ToApiRepresentedEnum<Api.CourseEnrollmentType>().Expect(),
                                                             new EnrollmentTermDateOverride(kv.Item2))) 
                        ?? new Dictionary<Api.CourseEnrollmentType, EnrollmentTermDateOverride>();
        }
        
        /// <inheritdoc />
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

    /// <summary>
    /// The state of a <see cref="EnrollmentTerm"/>.
    /// </summary>
    [PublicAPI]
    public enum EnrollmentTermWorkflowState {
        /// <summary>
        /// The term is active.
        /// </summary>
        [ApiRepresentation("active")]
        Active,
        /// <summary>
        /// The term is deleted.
        /// </summary>
        [ApiRepresentation("deleted")]
        Deleted,
        Unknown
    }

    /// <summary>
    /// Represents a date override for a specific enrollment type within a term.
    /// </summary>
    [PublicAPI]
    public struct EnrollmentTermDateOverride : IPrettyPrint {
        
        /// <summary>
        /// The new start date.
        /// </summary>
        public DateTime? StartAt { get; }
        
        /// <summary>
        /// The new end date.
        /// </summary>
        public DateTime? EndAt { get; }

        internal EnrollmentTermDateOverride(EnrollmentTermDateOverrideModel model) {
            StartAt = model.StartAt;
            EndAt = model.EndAt;
        }

        internal EnrollmentTermDateOverride(DateTime? start, DateTime? end) {
            StartAt = start;
            EndAt = end;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "EnrollmentTermDateOverride {" +
                   ($"\n{nameof(StartAt)}: {StartAt}," +
                    $"\n{nameof(EndAt)}: {EndAt}").Indent(4) + 
                   "\n}";
        }
    }
}
