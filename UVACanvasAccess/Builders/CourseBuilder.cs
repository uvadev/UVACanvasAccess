using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {
    
    [PublicAPI]
    public class CourseBuilder {
        private readonly Api _api;
        internal ulong? AccountId { get; }
        
        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        internal CourseBuilder(Api api, ulong? accountId) {
            _api = api;
            AccountId = accountId;
        }

        public CourseBuilder WithName(string name) {
            return PutArr("name", name);
        }

        public CourseBuilder WithCourseCode(string courseCode) {
            return PutArr("course_code", courseCode);
        }

        public CourseBuilder WithStartTime(DateTime start) {
            return PutArr("start_at", start.ToIso8601Date());
        }

        public CourseBuilder WithEndTime(DateTime end) {
            return PutArr("end_at", end.ToIso8601Date());
        }

        public CourseBuilder WithLicense(License license) {
            return PutArr("license", license.GetApiRepresentation());
        }
        
        public CourseBuilder AsPublic(bool @public = true) {
            return PutArr("is_public", @public.ToShortString());
        }

        public CourseBuilder AsPublicToAuthUsers(bool publicToAuth = true) {
            return PutArr("is_public_to_auth_users", publicToAuth.ToShortString());
        }

        public CourseBuilder WithPublicSyllabus(bool publicSyllabus = true) {
            return PutArr("public_syllabus", publicSyllabus.ToShortString());
        }

        public CourseBuilder WithPublicDescription(string description) {
            return PutArr("public_description", description);
        }

        public CourseBuilder WithStudentWikiEditing(bool allowed = true) {
            return PutArr("allow_student_wiki_edits", allowed.ToShortString());
        }

        public CourseBuilder WithWikiComments(bool allowed = true) {
            return PutArr("allow_wiki_comments", allowed.ToShortString());
        }

        public CourseBuilder WithStudentForumAttachments(bool allowed = true) {
            return PutArr("allow_student_forum_attachments", allowed.ToShortString());
        }

        public CourseBuilder WithOpenEnrollment(bool openEnrollment = true) {
            return PutArr("open_enrollment", openEnrollment.ToShortString());
        }

        public CourseBuilder WithSelfEnrollment(bool selfEnrollment = true) {
            return PutArr("self_enrollment", selfEnrollment.ToShortString());
        }

        public CourseBuilder WithDateRestrictedEnrollments(bool restrict = true) {
            return PutArr("restrict_enrollments_to_course_dates", restrict.ToShortString());
        }

        public CourseBuilder WithTermId(ulong termId) {
            return PutArr("term_id", termId.ToString());
        }

        public CourseBuilder WithSisId(string sis) {
            return PutArr("sis_course_id", sis);
        }

        public CourseBuilder WithIntegrationId(string integration) {
            return PutArr("integration_id", integration);
        }

        public CourseBuilder WithHiddenFinalGrades(bool hide = true) {
            return PutArr("hide_final_grades", hide.ToShortString());
        }

        public CourseBuilder WithAssignmentGroupWeight(bool weight = true) {
            return PutArr("apply_assignment_group_weights", weight.ToShortString());
        }

        public CourseBuilder WithTimeZone(string timeZone) {
            return PutArr("time_zone", timeZone);
        }

        public CourseBuilder OfferImmediately(bool offerNow = true) {
            return Put("offer", offerNow.ToShortString());
        }

        public CourseBuilder EnrollCurrentUser(bool enrollMe = true) {
            return Put("enroll_me", enrollMe.ToShortString());
        }

        public CourseBuilder WithDefaultView(CourseView defaultView) {
            return PutArr("default_view", defaultView.GetApiRepresentation());
        }

        public CourseBuilder WithSyllabusBody(string body) {
            return PutArr("syllabus_body", body);
        }

        public CourseBuilder WithGradingStandard(ulong standard) {
            return PutArr("grading_standard_id", standard.ToString());
        }

        public CourseBuilder WithCourseFormat(CourseFormat format) {
            return PutArr("course_format", format.GetApiRepresentation());
        }

        public CourseBuilder TryToRecoverFromSis(bool tryToRecover = true) {
            return Put("enable_sis_reactivation", tryToRecover.ToShortString());
        }

        public Task<Course> Post() {
            return _api.PostCreateCourse(this);
        }

        private CourseBuilder Put(string k, string v) {
            Fields[k] = v;
            return this;
        }
        
        private CourseBuilder PutArr(string k, string v) {
            Fields[$"course[{k}]"] = v;
            return this;
        }
    }
}
