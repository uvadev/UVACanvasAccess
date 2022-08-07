using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Assignments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Assignments {
    
    /// <summary>
    /// Represents TurnItIn settings.
    /// </summary>
    [PublicAPI]
    public class TurnitinSettings : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// When the originality report will be visible to students.
        /// </summary>
        public string OriginalityReportVisibility { get; }

        public string SPaperCheck { get; }

        public string InternetCheck { get; }
        
        public string JournalCheck { get; }
        
        public string ExcludeBiblio { get; }

        public string ExcludeQuoted { get; }

        public string ExcludeSmallMatchesType { get; }

        public uint ExcludeSmallMatchesValue { get; }

        internal TurnitinSettings(Api api, TurnitinSettingsModel model) {
            this.api = api;
            OriginalityReportVisibility = model.OriginalityReportVisibility;
            SPaperCheck = model.SPaperCheck;
            InternetCheck = model.InternetCheck;
            JournalCheck = model.JournalCheck;
            ExcludeBiblio = model.ExcludeBiblio;
            ExcludeQuoted = model.ExcludeQuoted;
            ExcludeSmallMatchesType = model.ExcludeSmallMatchesType;
            ExcludeSmallMatchesValue = model.ExcludeSmallMatchesValue;
        }

        public string ToPrettyString() {
            return "TurnitinSettings {" +
                   ($"\n{nameof(OriginalityReportVisibility)}: {OriginalityReportVisibility}," +
                   $"\n{nameof(SPaperCheck)}: {SPaperCheck}," +
                   $"\n{nameof(InternetCheck)}: {InternetCheck}," +
                   $"\n{nameof(JournalCheck)}: {JournalCheck}," +
                   $"\n{nameof(ExcludeBiblio)}: {ExcludeBiblio}," +
                   $"\n{nameof(ExcludeQuoted)}: {ExcludeQuoted}," +
                   $"\n{nameof(ExcludeSmallMatchesType)}: {ExcludeSmallMatchesType}," +
                   $"\n{nameof(ExcludeSmallMatchesValue)}: {ExcludeSmallMatchesValue}").Indent(4) + 
                   "\n}";
        }

        internal TurnitinSettingsModel ToModel() {
            return new TurnitinSettingsModel {
                                                 OriginalityReportVisibility = OriginalityReportVisibility,
                                                 SPaperCheck = SPaperCheck,
                                                 InternetCheck = InternetCheck,
                                                 JournalCheck = JournalCheck,
                                                 ExcludeBiblio = ExcludeBiblio,
                                                 ExcludeQuoted = ExcludeQuoted,
                                                 ExcludeSmallMatchesType = ExcludeSmallMatchesType,
                                                 ExcludeSmallMatchesValue = ExcludeSmallMatchesValue 
                                             };
        }
    }
}