using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;

namespace UVACanvasAccess.Structures.SisImports {
    
    [PublicAPI]
    public class SisImportCounts {
        private readonly Api _api;
        
        public ulong Accounts { get; }
        
        public ulong Terms { get; }
        
        public ulong AbstractCourses { get; }

        public ulong Courses { get; }

        public ulong Sections { get; }
        
        public ulong CrossLists { get; }
        
        public ulong Users { get; }

        public ulong Enrollments { get; }

        public ulong Groups { get; }

        public ulong GroupMemberships { get; }

        public ulong GradePublishingResults { get; }
        
        public ulong BatchCoursesDeleted { get; }

        public ulong BatchSectionsDeleted { get; }

        public ulong BatchEnrollmentsDeleted { get; }
        
        public ulong Errors { get; }
        
        public ulong Warnings { get; }

        internal SisImportCounts(Api api, SisImportCountsModel model) {
            _api = api;
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
    }
}
