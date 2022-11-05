using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Enrollments;
using UVACanvasAccess.Util;
using static UVACanvasAccess.ApiParts.Api;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentState;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentRoleTypes;

namespace UVACanvasAccess.Structures.Users {
    
    /// <summary>
    /// Represents a user.
    /// </summary>
    [PublicAPI]
    public class User : IPrettyPrint, IAppointmentGroupParticipant {
        private readonly Api api;
        
        /// <summary>
        /// The user's unique id.
        /// </summary>
        public ulong Id { get; }

        private string name;
        public string Name {
            get => name;
            set {
                var _ = api.EditUser(new[] {("name", value)}, Id).Result;
                name = value;
            }
        }

        private string sortableName;
        public string SortableName {
            get => sortableName;
            set {
                var _ = api.EditUser(new[] {("sortable_name", value)}, Id).Result;
                sortableName = value; 
            }
        }

        private string shortName;
        public string ShortName {
            get => shortName;
            set {
                var _ = api.EditUser(new[] {("short_name", value)}, Id).Result;
                shortName = value;
            }
        }
        
        public string SisUserId { get; private set; }
        public ulong? SisImportId { get; private set; }
        public string IntegrationId { get; private set; }
        public string LoginId { get; private set; }
        public string AvatarUrl { get; private set; }
        
        [Enigmatic]
        public IEnumerable<Enrollment> Enrollments { get; private set; }
        public string Email { get; private set; }
        public string Locale { get; private set; }
        public string EffectiveLocale { get; private set; }
        public DateTime? LastLogin { get; }

        private string _timeZone;
        public string TimeZone {
            get => _timeZone;
            set {
                var _ = api.EditUser(new[] {("time_zone", value)}).Result;
                _timeZone = value;
            } 
        }

        private string _bio;
        public string Bio {
            get => _bio;
            set {
                var _ = api.EditUser(new[] {("bio", value)}, Id).Result;
                _bio = value;
            }
        }
        
        public Dictionary<string, bool> Permissions { get; private set; }

        internal User(Api api, UserModel model) {
            this.api = api;
            Id = model.Id;
            name = model.Name;
            sortableName = model.SortableName;
            shortName = model.ShortName;
            SisUserId = model.SisUserId;
            SisImportId = model.SisImportId;
            IntegrationId = model.IntegrationId;
            LoginId = model.LoginId;
            AvatarUrl = model.AvatarUrl;
            Enrollments = model.Enrollments.SelectNotNull(m => new Enrollment(api, m));
            Email = model.Email;
            Locale = model.Locale;
            EffectiveLocale = model.EffectiveLocale;
            LastLogin = model.LastLogin;
            _timeZone = model.TimeZone;
            _bio = model.Bio;
            Permissions = model.Permissions ?? new Dictionary<string, bool>();
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "User {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(SortableName)}: {SortableName}," +
                   $"\n{nameof(ShortName)}: {ShortName}," +
                   $"\n{nameof(SisUserId)}: {SisUserId}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(LoginId)}: {LoginId}," +
                   $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                   $"\n{nameof(Enrollments)}: {Enrollments.ToPrettyString()}," +
                   $"\n{nameof(Email)}: {Email}," +
                   $"\n{nameof(Locale)}: {Locale}," +
                   $"\n{nameof(EffectiveLocale)}: {EffectiveLocale}," +
                   $"\n{nameof(LastLogin)}: {LastLogin}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Bio)}: {Bio}," +
                   $"\n{nameof(Permissions)}: {Permissions.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
        
        /// <summary>
        /// Returns this user's profile.
        /// </summary>
        /// <returns>The profile.</returns>
        public Task<Profile> GetProfile() {
            return api.GetUserProfile(Id);
        }

        /// <summary>
        /// Streams this user's page view history. Page views are returned in descending order; newest to oldest.
        /// </summary>
        /// <param name="startTime">The beginning of the date-time range to retrieve page views from. Defaults to unbounded.</param>
        /// <param name="endTime">The end of the date-time range to retrieve page views from. Defaults to unbounded.</param>
        /// <returns>The stream of page views.</returns>
        public IAsyncEnumerable<PageView> StreamPageViews(DateTime? startTime = null, DateTime? endTime = null) {
            return api.StreamUserPageViews(Id, startTime, endTime);
        }

        /// <summary>
        /// Returns whether or not this user is a teacher.
        /// Specifically, whether or not this user has enrolled with the Teacher role in at least one course.
        /// </summary>
        /// <param name="currentCoursesOnly">
        /// If true, this user is only considered a teacher if he is enrolled as a teacher in a currently active
        /// course.
        /// </param>
        /// <returns>Whether or not this user is a teacher.</returns>
        public async ValueTask<bool> IsTeacher(bool currentCoursesOnly = false) {
            var state = currentCoursesOnly ? new[] {Active} 
                                           : new CourseEnrollmentState[]{};
            return !await api.StreamUserEnrollments(Id, new[] {TeacherEnrollment}, state).IsEmptyAsync();
        }

        /// <summary>
        /// Streams all enrollments for this user.
        /// </summary>
        /// <param name="types">(Optional) The set of enrollment types to filter by.</param>
        /// <param name="states">(Optional) The set of enrollment states to filter by.</param>
        /// <param name="includes">(Optional) Data to include in the result.</param>
        /// <returns>The stream of enrollments.</returns>
        public IAsyncEnumerable<Enrollment> StreamEnrollments(IEnumerable<CourseEnrollmentRoleTypes> types = null,
                                                              IEnumerable<CourseEnrollmentState> states = null,
                                                              CourseEnrollmentIncludes? includes = null) {
            return api.StreamUserEnrollments(Id, types, states, includes);
        }
    }
}