using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    [PublicAPI]
    public class SisImportStatistics {
        private readonly Api _api;

        public ulong TotalStateChanges { get; }

        [CanBeNull]
        public SisImportStatistic Account { get; }

        [CanBeNull]
        public SisImportStatistic EnrollmentTerm { get; }

        [CanBeNull]
        public SisImportStatistic CommunicationChannel { get; }

        [CanBeNull]
        public SisImportStatistic AbstractCourse { get; }

        [CanBeNull]
        public SisImportStatistic Course { get; }

        [CanBeNull]
        public SisImportStatistic CourseSection { get; }

        [CanBeNull]
        public SisImportStatistic Enrollment { get; }

        [CanBeNull]
        public SisImportStatistic GroupCategory { get; }

        [CanBeNull]
        public SisImportStatistic Group { get; }

        [CanBeNull]
        public SisImportStatistic GroupMembership { get; }

        [CanBeNull]
        public SisImportStatistic Pseudonym { get; }

        [CanBeNull]
        public SisImportStatistic UserObserver { get; }

        [CanBeNull]
        public SisImportStatistic AccountUser { get; }

        internal SisImportStatistics(Api api, SisImportStatisticsModel model) {
            _api = api;
            TotalStateChanges = model.TotalStateChanges;
            Account = model.Account.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            EnrollmentTerm = model.EnrollmentTerm.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            CommunicationChannel = model.CommunicationChannel.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            AbstractCourse = model.AbstractCourse.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            Course = model.Course.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            CourseSection = model.CourseSection.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            Enrollment = model.Enrollment.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            GroupCategory = model.GroupCategory.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            Group = model.Group.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            GroupMembership = model.GroupMembership.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            Pseudonym = model.Pseudonym.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            UserObserver = model.UserObserver.ConvertIfNotNull(m => new SisImportStatistic(api, m));
            AccountUser = model.AccountUser.ConvertIfNotNull(m => new SisImportStatistic(api, m));
        }
    }
}
