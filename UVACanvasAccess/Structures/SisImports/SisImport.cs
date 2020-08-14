using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Structures.Users;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    [PublicAPI]
    public class SisImport : IPrettyPrint {
        private readonly Api _api;
        
        public ulong Id { get; }

        public DateTime? CreatedAt { get; }

        public DateTime? EndedAt { get; }
        
        public DateTime? UpdatedAt { get; }
        
        public SisImportState WorkflowState { get; }

        public SisImportData Data { get; }

        public SisImportStatistics Statistics { get; }

        public long? Progress { get; }

        public object ErrorsAttachment { get; }

        [CanBeNull]
        public User User { get; }

        [CanBeNull]
        public IEnumerable<IEnumerable<string>> ProcessingWarnings { get; }

        [CanBeNull]
        public IEnumerable<IEnumerable<string>> ProcessingErrors { get; }

        public bool? BatchMode { get; }

        public long? BatchModeTermId { get; }

        public bool? MultiTermBatchMode { get; }

        public bool? SkipDeletes { get; }
        
        public bool? OverrideSisStickiness { get; }

        public bool? AddSisStickiness { get; }
        
        public bool? ClearSisStickiness { get; }
        
        public string DiffingDataSetIdentifier { get; }

        public ulong? DiffedAgainstImportId { get; }

        public IEnumerable<object> CsvAttachments { get; }

        internal SisImport(Api api, SisImportModel model) {
            _api = api;
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
            ProcessingWarnings = model.ProcessingWarnings;
            ProcessingErrors = model.ProcessingErrors;
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
        }

        public string ToPrettyString() {
            return "SisImport {" +
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(EndedAt)}: {EndedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(Data)}: {Data}," +
                   $"\n{nameof(Statistics)}: {Statistics}," +
                   $"\n{nameof(Progress)}: {Progress}," +
                   $"\n{nameof(ErrorsAttachment)}: {ErrorsAttachment}," +
                   $"\n{nameof(User)}: {User?.ToPrettyString()}," +
                   $"\n{nameof(ProcessingWarnings)}: {ProcessingWarnings}," +
                   $"\n{nameof(ProcessingErrors)}: {ProcessingErrors}," +
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
