using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.SisImports;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.SisImports {
    
    /// <summary>
    /// Counts for which types of items were added, deleted, and changed in a <see cref="SisImport">SIS import</see>.
    /// </summary>
    [PublicAPI]
    public class SisImportStatistics : IPrettyPrint {
        private readonly Api api;

        /// <summary>
        /// The total count of all state changes.
        /// </summary>
        public ulong TotalStateChanges { get; }

        /// <summary>
        /// Counts for account items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic Account { get; }

        /// <summary>
        /// Counts for term items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic EnrollmentTerm { get; }

        /// <summary>
        /// Counts for communication channel items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic CommunicationChannel { get; }

        /// <summary>
        /// Counts for abstract course items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic AbstractCourse { get; }

        /// <summary>
        /// Counts for course items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic Course { get; }

        /// <summary>
        /// Counts for section items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic CourseSection { get; }

        /// <summary>
        /// Counts for enrollment items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic Enrollment { get; }

        /// <summary>
        /// Counts for group category items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic GroupCategory { get; }

        /// <summary>
        /// Counts for group items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic Group { get; }

        /// <summary>
        /// Counts for group memberships.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic GroupMembership { get; }

        /// <summary>
        /// Counts for pseudonym items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic Pseudonym { get; }

        /// <summary>
        /// Counts for account items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic UserObserver { get; }

        /// <summary>
        /// Counts for account items.
        /// </summary>
        [CanBeNull]
        public SisImportStatistic AccountUser { get; }

        internal SisImportStatistics(Api api, SisImportStatisticsModel model) {
            this.api = api;
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

        /// <inheritdoc />
        public string ToPrettyString() {
            return "SisImportStatistics {" + 
                   ($"\n{nameof(TotalStateChanges)}: {TotalStateChanges}," +
                   $"\n{nameof(Account)}: {Account}," +
                   $"\n{nameof(EnrollmentTerm)}: {EnrollmentTerm}," +
                   $"\n{nameof(CommunicationChannel)}: {CommunicationChannel}," +
                   $"\n{nameof(AbstractCourse)}: {AbstractCourse}," +
                   $"\n{nameof(Course)}: {Course}," +
                   $"\n{nameof(CourseSection)}: {CourseSection}," +
                   $"\n{nameof(Enrollment)}: {Enrollment}," +
                   $"\n{nameof(GroupCategory)}: {GroupCategory}," +
                   $"\n{nameof(Group)}: {Group}," +
                   $"\n{nameof(GroupMembership)}: {GroupMembership}," +
                   $"\n{nameof(Pseudonym)}: {Pseudonym}," +
                   $"\n{nameof(UserObserver)}: {UserObserver}," +
                   $"\n{nameof(AccountUser)}: {AccountUser}").Indent(4) +
                   "\n}";
        }
    }
}
