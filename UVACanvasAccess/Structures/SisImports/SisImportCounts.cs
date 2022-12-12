using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// Counts of processed rows per import category in a <see cref="SisImport">SIS import</see>.
    /// </summary>
    [PublicAPI]
    public class SisImportCounts : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The number of rows processed in an accounts import.
        /// </summary>
        public ulong Accounts { get; }
        
        /// <summary>
        /// The number of rows processed in a terms import.
        /// </summary>
        public ulong Terms { get; }
        
        /// <summary>
        /// The number of rows processed in an abstract courses import.
        /// </summary>
        public ulong AbstractCourses { get; }
        
        /// <summary>
        /// The number of rows processed in a courses import.
        /// </summary>
        public ulong Courses { get; }

        /// <summary>
        /// The number of rows processed in a sections import.
        /// </summary>
        public ulong Sections { get; }
        
        /// <summary>
        /// The number of rows processed in a cross-listing import.
        /// </summary>
        public ulong CrossLists { get; }
        
        /// <summary>
        /// The number of rows processed in a users import.
        /// </summary>
        public ulong Users { get; }

        /// <summary>
        /// The number of rows processed in an enrollments import.
        /// </summary>
        public ulong Enrollments { get; }

        /// <summary>
        /// The number of rows processed in a groups import.
        /// </summary>
        public ulong Groups { get; }

        /// <summary>
        /// The number of rows processed in a group memberships import.
        /// </summary>
        public ulong GroupMemberships { get; }

        /// <summary>
        /// The number of rows processed in a grade publishing results import.
        /// </summary>
        public ulong GradePublishingResults { get; }
        
        /// <summary>
        /// If the import ran in batch mode, the number of courses deleted as a result.
        /// </summary>
        public ulong BatchCoursesDeleted { get; }

        /// <summary>
        /// If the import ran in batch mode, the number of sections deleted as a result.
        /// </summary>
        public ulong BatchSectionsDeleted { get; }

        /// <summary>
        /// If the import ran in batch mode, the number of enrollments deleted as a result.
        /// </summary>
        public ulong BatchEnrollmentsDeleted { get; }
        
        /// <summary>
        /// The number of errors.
        /// </summary>
        public ulong Errors { get; }
        
        /// <summary>
        /// The number of warnings.
        /// </summary>
        public ulong Warnings { get; }

        internal SisImportCounts(Api api, SisImportCountsModel model) {
            this.api = api;
            Accounts = model.Accounts;
            Terms = model.Terms;
            AbstractCourses = model.AbstractCourses;
            Courses = model.Courses;
            Sections = model.Sections;
            CrossLists = model.CrossLists;
            Users = model.Users;
            Enrollments = model.Enrollments;
            Groups = model.Groups;
            GroupMemberships = model.GroupMemberships;
            GradePublishingResults = model.GradePublishingResults;
            BatchCoursesDeleted = model.BatchCoursesDeleted ?? 0;
            BatchSectionsDeleted = model.BatchSectionsDeleted ?? 0;
            BatchEnrollmentsDeleted = model.BatchEnrollmentsDeleted ?? 0;
            Errors = model.Errors;
            Warnings = model.Warnings;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "SisImportCounts {" +
                   ($"\n{nameof(Accounts)}: {Accounts}," +
                   $"\n{nameof(Terms)}: {Terms}," +
                   $"\n{nameof(AbstractCourses)}: {AbstractCourses}," +
                   $"\n{nameof(Courses)}: {Courses}," +
                   $"\n{nameof(Sections)}: {Sections}," +
                   $"\n{nameof(CrossLists)}: {CrossLists}," +
                   $"\n{nameof(Users)}: {Users}," +
                   $"\n{nameof(Enrollments)}: {Enrollments}," +
                   $"\n{nameof(Groups)}: {Groups}," +
                   $"\n{nameof(GroupMemberships)}: {GroupMemberships}," +
                   $"\n{nameof(GradePublishingResults)}: {GradePublishingResults}," +
                   $"\n{nameof(BatchCoursesDeleted)}: {BatchCoursesDeleted}," +
                   $"\n{nameof(BatchSectionsDeleted)}: {BatchSectionsDeleted}," +
                   $"\n{nameof(BatchEnrollmentsDeleted)}: {BatchEnrollmentsDeleted}," +
                   $"\n{nameof(Errors)}: {Errors}," +
                   $"\n{nameof(Warnings)}: {Warnings}").Indent(4) +
                   "\n}";
        }
    }
}
