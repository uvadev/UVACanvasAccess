using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Accounts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Structures.Accounts;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Structures.Roles;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [Flags]
        public enum AccountIncludes : byte {
            Default = 0,
            [ApiRepresentation("lti_guid")]
            LtiGuid = 1 << 0,
            [ApiRepresentation("registration_settings")]
            RegistrationSettings = 1 << 1,
            [ApiRepresentation("services")]
            Services = 1 << 2
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListAccounts([NotNull] IEnumerable<string> includes) {
            return _client.GetAsync("accounts" + BuildQueryString(includes.Select(i => ("include[]", i)).ToArray()));
        }

        public async Task<IEnumerable<Account>> ListAccounts(AccountIncludes includes = AccountIncludes.Default) {
            var response = await RawListAccounts(includes.GetFlags().Select(f => f.GetApiRepresentation()));

            var models = await AccumulateDeserializePages<AccountModel>(response);

            return from model in models
                   select new Account(this, model);
        }

        private Task<HttpResponseMessage> RawGetAccount(string id) {
            return _client.GetAsync($"accounts/{id}");
        }

        public async Task<Account> GetAccount(ulong accountId) {
            var response = await RawGetAccount(accountId.ToString());

            var model = JsonConvert.DeserializeObject<AccountModel>(await response.Content.ReadAsStringAsync());
            return new Account(this, model);
        }

        private Task<HttpResponseMessage> RawGetAccountPermissions(string id, [NotNull] IEnumerable<string> permissions) {
            var url = $"accounts/{id}/permissions";
            return _client.GetAsync(url + BuildDuplicateKeyQueryString(permissions.Select(p => ("permissions[]", p)).ToArray()));
        }

        public async Task<BasicAccountPermissionsSet> GetAccountPermissions(AccountRolePermissions checkedPermissions, 
                                                                            ulong? accountId = null) {
            var response = await RawGetAccountPermissions(accountId?.ToString() ?? "self",
                                                          checkedPermissions.GetFlags()
                                                                            .Select(p => p.GetApiRepresentation()));
            
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(await response.Content.ReadAsStringAsync());

            AccountRolePermissions allowed = default, 
                                   denied = default;

            foreach (var (k, v) in dictionary) {
                AccountRolePermissions? permission = k.ToApiRepresentedEnum<AccountRolePermissions>();
                if (permission == null) {
                    Console.Error.WriteLine("WARNING: encountered unknown permission type: " + k);
                    continue;
                }
                
                if (v) {
                    allowed |= permission.Value;
                } else {
                    denied |= permission.Value;
                }
            }
            
            return new BasicAccountPermissionsSet(allowed, denied);
        }

        private Task<HttpResponseMessage> RawGetTermsOfService(string id) {
            return _client.GetAsync($"accounts/{id}/terms_of_service");
        }

        public async Task<TermsOfService> GetTermsOfService(ulong? accountId = null) {
            var response = await RawGetTermsOfService(accountId?.ToString() ?? "self");

            var model = JsonConvert.DeserializeObject<TermsOfServiceModel>(await response.Content.ReadAsStringAsync());
            return new TermsOfService(this, model);
        }

        private Task<HttpResponseMessage> RawGetHelpLinks(string id) {
            return _client.GetAsync($"accounts/{id}/help_links");
        }

        public async Task<HelpLinks> GetHelpLinks(ulong? accountId = null) {
            var response = await RawGetHelpLinks(accountId?.ToString() ?? "self");

            var model = JsonConvert.DeserializeObject<HelpLinksModel>(await response.Content.ReadAsStringAsync());
            return new HelpLinks(this, model);
        }

        [Flags]
        [PublicAPI]
        public enum CourseEnrollmentTypes : byte {
            [ApiRepresentation("teacher")]
            Teacher = 1 << 0,
            [ApiRepresentation("students")]
            Students = 1 << 1,
            [ApiRepresentation("ta")]
            Ta = 1 << 2,
            [ApiRepresentation("observer")]
            Observer = 1 << 3,
            [ApiRepresentation("designer")]
            Designer = 1 << 4
        }

        [Flags]
        [PublicAPI]
        public enum CourseStates : byte {
            [ApiRepresentation("created")]
            Created = 1 << 0,
            [ApiRepresentation("claimed")]
            Claimed = 1 << 1,
            [ApiRepresentation("available")]
            Available = 1 << 2,
            [ApiRepresentation("completed")]
            Completed = 1 << 3,
            [ApiRepresentation("deleted")]
            Deleted = 1 << 4,
            [ApiRepresentation("all")]
            All = 1 << 5
        }
        
        [Flags]
        [PublicAPI]
        public enum AccountLevelCourseIncludes : byte {
            [ApiRepresentation("syllabus_body")]
            SyllabusBody = 1 << 0,
            [ApiRepresentation("term")]
            Term = 1 << 1,
            [ApiRepresentation("course_progress")]
            CourseProgress = 1 << 2,
            [ApiRepresentation("storage_quota_used_mb")]
            StorageQuotaUsedMb = 1 << 3,
            [ApiRepresentation("total_students")]
            TotalStudents = 1 << 4,
            [ApiRepresentation("teachers")]
            Teachers = 1 << 5,
            [ApiRepresentation("account_name")]
            AccountName = 1 << 6,
            [ApiRepresentation("concluded")]
            Concluded = 1 << 7,
            Everything = byte.MaxValue
        }

        [PublicAPI]
        public enum CourseSort : byte {
            [ApiRepresentation("course_name")]
            CourseName,
            [ApiRepresentation("sis_course_id")]
            SisCourseId,
            [ApiRepresentation("teacher")]
            Teacher,
            [ApiRepresentation("account_name")]
            AccountName
        }

        [PublicAPI]
        public enum CourseSearchBy : byte {
            [ApiRepresentation("course")]
            Course,
            [ApiRepresentation("teacher")]
            Teacher
        }

        [PaginatedResponse]
        private Task<HttpResponseMessage> RawListCourses([NotNull] string accountId,
                                                         [CanBeNull] string searchTerm,
                                                         bool? withEnrollmentsOnly,
                                                         bool? published,
                                                         bool? completed,
                                                         bool? blueprint,
                                                         bool? blueprintAssociated,
                                                         ulong? enrollmentTermId,
                                                         [CanBeNull] IEnumerable<ulong> byTeachers,
                                                         [CanBeNull] IEnumerable<ulong> bySubaccounts,
                                                         CourseEnrollmentTypes? enrollmentTypes,
                                                         CourseStates? states,
                                                         AccountLevelCourseIncludes? includes,
                                                         CourseSort? sort,
                                                         CourseSearchBy? searchBy,
                                                         Order? order) {
            var url = $"accounts/{accountId}/courses";
            var argsList = new List<(string, string)> {
                                                          ("with_enrollments", withEnrollmentsOnly?.ToShortString()),
                                                          ("published", published?.ToShortString()),
                                                          ("completed", published?.ToShortString()),
                                                          ("blueprint", blueprint?.ToShortString()),
                                                          ("blueprint_associated", blueprintAssociated?.ToShortString()),
                                                          ("enrollment_term_id", enrollmentTermId?.ToString()),
                                                          ("search_term", searchTerm),
                                                          ("sort", sort?.GetApiRepresentation()),
                                                          ("order", order?.GetApiRepresentation()),
                                                          ("search_by", searchBy?.GetApiRepresentation())
                                                      };

            byTeachers?.Select(id => ("by_teachers[]", id.ToString()))
                       .Peek(t => argsList.Add(t));
            
            bySubaccounts?.Select(id => ("by_subaccounts[]", id.ToString()))
                          .Peek(t => argsList.Add(t));
            
            enrollmentTypes?.GetFlagsApiRepresentations()
                            .Select(r => ("enrollment_type[]", r))
                            .Peek(t => argsList.Add(t));

            states?.GetFlagsApiRepresentations()
                   .Select(r => ("state[]", r))
                   .Peek(t => argsList.Add(t));

            includes?.GetFlagsApiRepresentations()
                     .Select(r => ("include[]", r))
                     .Peek(t => argsList.Add(t));

            var query = BuildDuplicateKeyQueryString(argsList.ToArray());

            return _client.GetAsync(url + query);
        }
        
        /// <summary>
        /// Returns a list of courses associated with the account.
        /// 
        /// <paramref name="accountId"/>, when omitted, defaults to <c>self</c>.
        /// All other parameters are optional and serve to narrow the search or change the included data.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="searchTerm">
        /// A search term, searching either by course or teacher name as according to <paramref name="searchBy"/>.
        /// </param>
        /// <param name="withEnrollmentsOnly">
        /// If true, only include courses with at least 1 enrollment. If false, only include courses with no enrollments.
        /// If omitted, no-op.
        /// </param>
        /// <param name="published">
        /// If true, only include courses that are published. If false, only include courses that are not published.
        /// If omitted, no-op.
        /// </param>
        /// <param name="completed">
        /// If true, only include courses that are completed. If false, only include courses that are not completed.
        /// If omitted, no-op.
        /// </param>
        /// <param name="blueprint">
        /// If true, only include courses that are blueprints. If false, only include courses that are not blueprints.
        /// If omitted, no-op.
        /// </param>
        /// <param name="blueprintAssociated">
        /// If true, only include courses that inherit from a blueprint.
        /// If false, only include courses that do not inherit from a blueprint..
        /// If omitted, no-op.
        /// </param>
        /// <param name="enrollmentTermId">If present, only include courses from this term.</param>
        /// <param name="byTeachers">If present, only include courses taught by these teachers.</param>
        /// <param name="bySubaccounts">If present, only include course associated with these subaccounts.</param>
        /// <param name="enrollmentTypes">
        /// If present, only include courses that have at least user enrolled with one of these enrollment types.
        /// </param>
        /// <param name="states">
        /// If present, only include courses that are in one of these states.
        /// If omitted, all states except <see cref="CourseStates.Deleted"/> are included.
        /// </param>
        /// <param name="includes">Additional data to include in the returned objects.</param>
        /// <param name="sort">The column to sort by.</param>
        /// <param name="searchBy">The column that <paramref name="searchTerm"/> searches by.</param>
        /// <param name="order">The order of results when sorted by <paramref name="sort"/>.</param>
        /// <returns>The list of courses.</returns>
        public async Task<IEnumerable<Course>> ListCourses(ulong? accountId = null,
                                                           string searchTerm = null,
                                                           bool? withEnrollmentsOnly = null,
                                                           bool? published = null,
                                                           bool? completed = null,
                                                           bool? blueprint = null,
                                                           bool? blueprintAssociated = null,
                                                           ulong? enrollmentTermId = null,
                                                           IEnumerable<ulong> byTeachers = null,
                                                           IEnumerable<ulong> bySubaccounts = null,
                                                           CourseEnrollmentTypes? enrollmentTypes = null,
                                                           CourseStates? states = null,
                                                           AccountLevelCourseIncludes? includes = null,
                                                           CourseSort? sort = null,
                                                           CourseSearchBy? searchBy = null,
                                                           Order? order = null) {
            
            var response = await RawListCourses(accountId?.ToString() ?? "self", searchTerm, withEnrollmentsOnly, 
                                                published, completed, blueprint, blueprintAssociated, enrollmentTermId,
                                                byTeachers, bySubaccounts, enrollmentTypes, states, includes, sort,
                                                searchBy, order);

            List<CourseModel> models = await AccumulateDeserializePages<CourseModel>(response);

            return from model in models
                   select new Course(this, model);
        }
    }
}