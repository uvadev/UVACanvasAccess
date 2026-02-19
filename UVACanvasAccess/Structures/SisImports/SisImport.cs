using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// Represents a SIS import job.
    /// </summary>
    [PublicAPI]
    public class SisImport : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The import id.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// When the import was created.
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        /// When the import finished, if at all.
        /// </summary>
        public DateTime? EndedAt { get; }
        
        /// <summary>
        /// When the import was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; }
        
        /// <summary>
        /// The import state.
        /// </summary>
        public SisImportState WorkflowState { get; }

        /// <summary>
        /// The import data.
        /// </summary>
        public SisImportData Data { get; }

        /// <summary>
        /// The import statistics.
        /// </summary>
        public SisImportStatistics Statistics { get; }

        /// <summary>
        /// The progress of the import, out of 100.
        /// </summary>
        public long? Progress { get; }

        /// <summary>
        /// The error attachment.
        /// </summary>
        [Enigmatic]
        public object ErrorsAttachment { get; }

        /// <summary>
        /// The user who initiated the import.
        /// </summary>
        [CanBeNull]
        public User User { get; }

        /// <summary>
        /// Any warnings raised after the import has completed.
        /// </summary>
        [CanBeNull]
        public IEnumerable<SisImportMessage> ProcessingWarnings { get; }

        /// <summary>
        /// Any errors raised after the import has completed.
        /// </summary>
        [CanBeNull]
        public IEnumerable<SisImportMessage> ProcessingErrors { get; }

        /// <summary>
        /// Whether the import was run in batch mode.
        /// </summary>
        /// <remarks>
        /// Normally, when the import file is missing an entry currently
        /// present in Canvas, the entry is left alone. In batch mode, it will be deleted instead.
        /// </remarks>
        public bool? BatchMode { get; }

        /// <summary>
        /// If running in batch mode, the term id the batch was limited to.
        /// </summary>
        public long? BatchModeTermId { get; }

        /// <summary>
        /// Whether multi-term batch mode is enabled.
        /// </summary>
        public bool? MultiTermBatchMode { get; }

        /// <summary>
        /// Whether the import will skip any deletes.
        /// </summary>
        public bool? SkipDeletes { get; }
        
        /// <summary>
        /// Whether the import will override SIS stickiness.
        /// </summary>
        public bool? OverrideSisStickiness { get; }

        /// <summary>
        /// Whether the import add override SIS stickiness.
        /// </summary>
        public bool? AddSisStickiness { get; }
        
        /// <summary>
        /// Whether the import will remove SIS stickiness.
        /// </summary>
        public bool? ClearSisStickiness { get; }
        
        /// <summary>
        /// Whether a diffing job failed due to an exceeded threshold.
        /// </summary>
        public bool? DiffingThresholdExceeded { get; }
        
        /// <summary>
        /// The id of the dataset that this SIS batch diffs against.
        /// </summary>
        public string DiffingDataSetIdentifier { get; }

        /// <summary>
        /// The id of the import that this SIS batch diffs against.
        /// </summary>
        public ulong? DiffedAgainstImportId { get; }

        /// <summary>
        /// Attached files for processing.
        /// </summary>
        public IEnumerable<object> CsvAttachments { get; }

        internal SisImport(Api api, SisImportModel model) {
            this.api = api;
            Id = model.Id;
            CreatedAt = model.CreatedAt;
            EndedAt = model.EndedAt;
            UpdatedAt = model.UpdatedAt;
            WorkflowState = model.WorkflowState.ToApiRepresentedEnum<SisImportState>() ?? SisImportState.Invalid;
            Data = model.Data.ConvertIfNotNull(d => new SisImportData(api, d));
            Statistics = model.Statistics.ConvertIfNotNull(s => new SisImportStatistics(api, s));
            Progress = model.Progress;
            ErrorsAttachment = model.ErrorsAttachment;
            User = model.User.ConvertIfNotNull(u => new User(api, u));
            ProcessingWarnings = model.ProcessingWarnings.SelectNotNull(m => new SisImportMessage(m));
            ProcessingErrors = model.ProcessingErrors.SelectNotNull(m => new SisImportMessage(m));
            BatchMode = model.BatchMode;
            BatchModeTermId = model.BatchModeTermId;
            MultiTermBatchMode = model.MultiTermBatchMode;
            SkipDeletes = model.SkipDeletes;
            OverrideSisStickiness = model.OverrideSisStickiness;
            AddSisStickiness = model.AddSisStickiness;
            ClearSisStickiness = model.ClearSisStickiness;
            DiffingDataSetIdentifier = model.DiffingDataSetIdentifier;
            DiffedAgainstImportId = model.DiffedAgainstImportId;
            CsvAttachments = model.CsvAttachments;
            DiffingThresholdExceeded = model.DiffingThresholdExceeded;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "SisImport {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(EndedAt)}: {EndedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(Data)}: {Data?.ToPrettyString()}," +
                   $"\n{nameof(Statistics)}: {Statistics?.ToPrettyString()}," +
                   $"\n{nameof(Progress)}: {Progress}," +
                   $"\n{nameof(ErrorsAttachment)}: {ErrorsAttachment}," +
                   $"\n{nameof(User)}: {User?.ToPrettyString()}," +
                   $"\n{nameof(ProcessingWarnings)}: {ProcessingWarnings?.ToPrettyString()}," +
                   $"\n{nameof(ProcessingErrors)}: {ProcessingErrors?.ToPrettyString()}," +
                   $"\n{nameof(BatchMode)}: {BatchMode}," +
                   $"\n{nameof(BatchModeTermId)}: {BatchModeTermId}," +
                   $"\n{nameof(MultiTermBatchMode)}: {MultiTermBatchMode}," +
                   $"\n{nameof(SkipDeletes)}: {SkipDeletes}," +
                   $"\n{nameof(OverrideSisStickiness)}: {OverrideSisStickiness}," +
                   $"\n{nameof(AddSisStickiness)}: {AddSisStickiness}," +
                   $"\n{nameof(ClearSisStickiness)}: {ClearSisStickiness}," +
                   $"\n{nameof(DiffingDataSetIdentifier)}: {DiffingDataSetIdentifier}," +
                   $"\n{nameof(DiffedAgainstImportId)}: {DiffedAgainstImportId}," +
                   $"\n{nameof(CsvAttachments)}: {CsvAttachments?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
}
