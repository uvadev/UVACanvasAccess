using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Roles {

    public static class RolePermissionsExtensions {

        internal static IEnumerable<string> GetRepresentations(this AccountRolePermissions e) {
            return e.GetFlags()
                    .Select(f => f.GetApiRepresentation());
        }
        
        internal static IEnumerable<string> GetRepresentations(this GeneralRolePermissions e) {
            return e.GetFlags()
                    .Select(f => f.GetApiRepresentation());
        }
        
    }

    [Flags]
    public enum AccountRolePermissions : uint {
        [ApiRepresentation("become_user")]
        BecomeUser = 1 << 0,
        [ApiRepresentation("import_sis")]
        ImportSis = 1 << 1,
        [ApiRepresentation("manage_account_memberships")]
        ManageAccountMemberships = 1 << 2,
        [ApiRepresentation("manage_account_settings")]
        ManageAccountSettings = 1 << 3,
        [ApiRepresentation("manage_alerts")]
        ManageAlerts = 1 << 4,
        [ApiRepresentation("manage_catalog")]
        ManageCatalog = 1 << 5,
        [ApiRepresentation("manage_courses")]
        ManageCourses = 1 << 6,
        [ApiRepresentation("manage_developer_keys")]
        ManageDeveloperKeys = 1 << 7,
        [ApiRepresentation("manage_future_flags")]
        ManageFeatureFlags = 1 << 8,
        [ApiRepresentation("manage_global_outcomes")]
        ManageGlobalOutcomes = 1 << 9,
        [ApiRepresentation("manage_jobs")]
        ManageJobs = 1 << 10,
        [ApiRepresentation("manage_master_courses")]
        ManageMasterCourses = 1 << 11,
        [ApiRepresentation("manage_role_overrides")]
        ManageRoleOverrides = 1 << 12,
        [ApiRepresentation("manage_storage_quotas")]
        ManageStorageQuotas = 1 << 13,
        [ApiRepresentation("manage_sis")]
        ManageSis = 1 << 14,
        [ApiRepresentation("manage_site_settings")]
        ManageSiteSettings = 1 << 15,
        [ApiRepresentation("manage_user_logins")]
        ManageUserLogins = 1 << 16,
        [ApiRepresentation("manage_user_observers")]
        ManageUserObservers = 1 << 17,
        [ApiRepresentation("read_course_content")]
        ReadCourseContent = 1 << 18,
        [ApiRepresentation("read_course_list")]
        ReadCourseList = 1 << 19,
        [ApiRepresentation("read_messages")]
        ReadMessages = 1 << 20,
        [ApiRepresentation("reset_any_mfa")]
        ResetAnyMfa = 1 << 21,
        [ApiRepresentation("site_admin")]
        SiteAdmin = 1 << 22,
        [ApiRepresentation("view_course_changes")]
        ViewCourseChanges = 1 << 23,
        [ApiRepresentation("view_grade_changes")]
        ViewGradeChanges = 1 << 24,
        [ApiRepresentation("view_jobs")]
        ViewJobs = 1 << 25,
        [ApiRepresentation("view_notifications")]
        ViewNotifications = 1 << 26,
        [ApiRepresentation("view_quiz_answer_audits")]
        ViewQuizAnswerAudits = 1 << 27,
        [ApiRepresentation("view_statistics")]
        ViewStatistics = 1 << 28,
        [ApiRepresentation("undelete_courses")]
        UndeleteCourses = 1 << 29
    }

    [Flags]
    public enum GeneralRolePermissions : ulong {
        [ApiRepresentation("change_course_state")]
        ChangeCourseState = 1 << 0,
        [ApiRepresentation("create_collaborations")]
        CreateCollaborations = 1 << 1,
        [ApiRepresentation("create_conferences")]
        CreateConferences = 1 << 2,
        [ApiRepresentation("create_forum")]
        CreateForum = 1 << 3,
        [ApiRepresentation("create_observer_pairing_code")]
        GenerateObserverPairingCode = 1 << 4,
        [ApiRepresentation("import_outcomes")]
        ImportOutcomes = 1 << 5,
        [ApiRepresentation("lti_add_edit")]
        LtiAddEdit = 1 << 6,
        [ApiRepresentation("manage_admin_users")]
        ManageAdminUsers = 1 << 7,
        [ApiRepresentation("manage_assignments")]
        ManageAssignments = 1 << 8,
        [ApiRepresentation("manage_calendar")]
        ManageCalendar = 1 << 9,
        [ApiRepresentation("manage_content")]
        ManageContent = 1 << 10,
        [ApiRepresentation("manage_files")]
        ManageFiles = 1 << 11,
        [ApiRepresentation("manage_grades")]
        ManageGrades = 1 << 12,
        [ApiRepresentation("manage_groups")]
        ManageGroups = 1 << 13,
        [ApiRepresentation("manage_interaction_alerts")]
        ManageInteractionAlerts = 1 << 14,
        [ApiRepresentation("manage_outcomes")]
        ManageOutcomes = 1 << 15,
        [ApiRepresentation("manage_sections")]
        ManageSections = 1 << 16,
        [ApiRepresentation("manage_students")]
        ManageStudents = 1 << 17,
        [ApiRepresentation("manage_user_notes")]
        ManageUserNotes = 1 << 18,
        [ApiRepresentation("manage_rubrics")]
        ManageRubrics = 1 << 19,
        [ApiRepresentation("manage_wiki")]
        ManageWiki = 1 << 20,
        [ApiRepresentation("moderate_forum")]
        ModerateForum = 1 << 21,
        [ApiRepresentation("post_to_forum")]
        PostToForum = 1 << 22,
        [ApiRepresentation("read_announcements")]
        ReadAnnouncements = 1 << 23,
        [ApiRepresentation("read_email_addresses")]
        ReadEmailAddresses = 1 << 24,
        [ApiRepresentation("read_forum")]
        ReadForum = 1 << 25,
        [ApiRepresentation("read_question_banks")]
        ReadQuestionBanks = 1 << 26,
        [ApiRepresentation("read_reports")]
        ReadReports = 1 << 27,
        [ApiRepresentation("read_roster")]
        ReadRoster = 1 << 28,
        [ApiRepresentation("read_sis")]
        ReadSis = 1 << 29,
        [ApiRepresentation("select_final_grade")]
        SelectFinalGrade = 1 << 30,
        [ApiRepresentation("send_messages")]
        SendMessages = 1L << 31,
        [ApiRepresentation("send_messages_all")]
        SendMessagesAll = 1L << 32,
        [ApiRepresentation("view_all_grades")]
        ViewAllGrades = 1L << 33,
        [ApiRepresentation("view_audit_trail")]
        ViewAuditTrail = 1L << 34,
        [ApiRepresentation("view_group_pages")]
        ViewGroupPages = 1L << 35,
        [ApiRepresentation("view_user_logins")]
        ViewUserLogins = 1L << 36
    }
    
    // ReSharper disable MemberCanBePrivate.Global
    public readonly struct RolePermissionsSet {
        
        public GeneralRolePermissions GeneralAllowed { get; }
        public GeneralRolePermissions GeneralDenied { get; }
        public AccountRolePermissions AccountAllowed { get; }
        public AccountRolePermissions AccountDenied { get; }
        public GeneralRolePermissions GeneralLocked { get; }
        public AccountRolePermissions AccountLocked { get; }

        public RolePermissionsSet(GeneralRolePermissions generalAllowed = default,
                                  GeneralRolePermissions generalDenied = default, 
                                  AccountRolePermissions accountAllowed = default,
                                  AccountRolePermissions accountDenied = default, 
                                  GeneralRolePermissions generalLocked = default, 
                                  AccountRolePermissions accountLocked = default) {
            GeneralAllowed = generalAllowed;
            GeneralDenied = generalDenied;
            AccountAllowed = accountAllowed;
            AccountDenied = accountDenied;
            GeneralLocked = generalLocked;
            AccountLocked = accountLocked;
        }

        [Pure]
        internal IEnumerable<(string, string)> GetAsParams() {
            IEnumerable<(string, string)> allowed = GeneralAllowed.GetFlags()
                                                                  .Select(f => f.GetApiRepresentation())
                                                                  .Concat(AccountAllowed.GetFlags()
                                                                                        .Select(f => f.GetApiRepresentation()))
                                                                  .Select(a => (($"permissions[{a}][explicit]", "1"), 
                                                                                ($"permissions[{a}][enabled]",  "1")))
                                                                  .Interleave();
            IEnumerable<(string, string)> denied = GeneralDenied.GetFlags()
                                                                .Select(f => f.GetApiRepresentation())
                                                                .Concat(AccountDenied.GetFlags()
                                                                                     .Select(f => f.GetApiRepresentation()))
                                                                .Select(a => (($"permissions[{a}][explicit]", "1"), 
                                                                              ($"permissions[{a}][enabled]",  "0")))
                                                                .Interleave();

            IEnumerable<(string, string)> locked = GeneralLocked.GetFlags()
                                                                .Select(f => f.GetApiRepresentation())
                                                                .Concat(AccountLocked.GetFlags()
                                                                                     .Select(f => f.GetApiRepresentation()))
                                                                .Select(a => ($"permissions[{a}][locked]", "1"));
            
            return allowed.Concat(denied).Concat(locked);
        }
    }
}