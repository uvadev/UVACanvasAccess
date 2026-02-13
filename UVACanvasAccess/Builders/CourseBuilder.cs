using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.BlueprintCourses;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Builders {
    
    /// <summary>
    /// Used to create or edit courses using the builder pattern.
    /// When all desired fields are set, call <see cref="Post"/> to execute the operation.
    /// </summary>
    [PublicAPI]
    public class CourseBuilder {
        private readonly Api api;
        private readonly bool isEditing;
        private readonly ulong? id;
        internal ulong? AccountId { get; }
        
        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        internal CourseBuilder(Api api, bool isEditing, ulong? accountId, ulong? id = null) {
            this.api = api;
            this.isEditing = isEditing;
            this.id = id;
            AccountId = accountId;
        }

        /// <summary>
        /// The name of this course.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithName(string name) {
            return PutArr("name", name);
        }

        /// <summary>
        /// The course code of this course.
        /// </summary>
        /// <param name="courseCode"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithCourseCode(string courseCode) {
            return PutArr("course_code", courseCode);
        }

        /// <summary>
        /// The start date of this course.
        /// </summary>
        /// <param name="start"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithStartDate(DateTime start) {
            return PutArr("start_at", start.ToIso8601Date());
        }

        /// <summary>
        /// The end date of this course.
        /// </summary>
        /// <param name="end"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithEndDate(DateTime end) {
            return PutArr("end_at", end.ToIso8601Date());
        }

        /// <summary>
        /// The license type for this course.
        /// </summary>
        /// <param name="license"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithLicense(License license) {
            return PutArr("license", license.GetApiRepresentation());
        }
        
        /// <summary>
        /// Make the course visible to all unauthenticated or authenticated users.
        /// </summary>
        /// <param name="public"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder AsPublic(bool @public = true) {
            return PutArr("is_public", @public.ToShortString());
        }

        /// <summary>
        /// Make the course visible to all authenticated users.
        /// </summary>
        /// <param name="publicToAuth"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder AsPublicToAuthUsers(bool publicToAuth = true) {
            return PutArr("is_public_to_auth_users", publicToAuth.ToShortString());
        }

        /// <summary>
        /// Make the syllabus public.
        /// </summary>
        /// <param name="publicSyllabus"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithPublicSyllabus(bool publicSyllabus = true) {
            return PutArr("public_syllabus", publicSyllabus.ToShortString());
        }

        /// <summary>
        /// Make the syllabus public to authenticated users only.
        /// </summary>
        /// <param name="publicSyllabusToAuth"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithPublicSyllabusToAuthUsers(bool publicSyllabusToAuth = true) {
            return PutArr("public_syllabus_to_auth", publicSyllabusToAuth.ToShortString());
        }

        /// <summary>
        /// The course's public description.
        /// </summary>
        /// <param name="description"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithPublicDescription(string description) {
            return PutArr("public_description", description);
        }

        /// <summary>
        /// Allow students to edit the wiki.
        /// </summary>
        /// <param name="allowed"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithStudentWikiEditing(bool allowed = true) {
            return PutArr("allow_student_wiki_edits", allowed.ToShortString());
        }

        /// <summary>
        /// Allow wiki comments.
        /// </summary>
        /// <param name="allowed"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithWikiComments(bool allowed = true) {
            return PutArr("allow_wiki_comments", allowed.ToShortString());
        }

        /// <summary>
        /// Allow students to attach files to forum posts.
        /// </summary>
        /// <param name="allowed"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithStudentForumAttachments(bool allowed = true) {
            return PutArr("allow_student_forum_attachments", allowed.ToShortString());
        }

        /// <summary>
        /// Allow open enrollment.
        /// </summary>
        /// <param name="openEnrollment"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithOpenEnrollment(bool openEnrollment = true) {
            return PutArr("open_enrollment", openEnrollment.ToShortString());
        }

        /// <summary>
        /// Allow self enrollment.
        /// </summary>
        /// <param name="selfEnrollment"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithSelfEnrollment(bool selfEnrollment = true) {
            return PutArr("self_enrollment", selfEnrollment.ToShortString());
        }

        /// <summary>
        /// Only allow enrollments between this course's start and end dates.
        /// </summary>
        /// <param name="restrict"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithDateRestrictedEnrollments(bool restrict = true) {
            return PutArr("restrict_enrollments_to_course_dates", restrict.ToShortString());
        }

        /// <summary>
        /// Move this course to another account.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithAccountId(ulong accountId) {
            return isEditing ? PutArr("account_id", accountId.ToString())
                             : this;
        }

        /// <summary>
        /// The unique term id to create this course in.
        /// </summary>
        /// <param name="termId"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithTermId(ulong termId) {
            return PutArr("term_id", termId.ToString());
        }

        /// <summary>
        /// The course's SIS id.
        /// </summary>
        /// <param name="sis"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithSisId(string sis) {
            return PutArr("sis_course_id", sis);
        }

        /// <summary>
        /// The course's integration id.
        /// </summary>
        /// <param name="integration"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithIntegrationId(string integration) {
            return PutArr("integration_id", integration);
        }

        /// <summary>
        /// Hide final grades in the student summary.
        /// </summary>
        /// <param name="hide"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithHiddenFinalGrades(bool hide = true) {
            return PutArr("hide_final_grades", hide.ToShortString());
        }

        /// <summary>
        /// Apply assigment group weighting to final grades.
        /// </summary>
        /// <param name="weight"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithAssignmentGroupWeight(bool weight = true) {
            return PutArr("apply_assignment_group_weights", weight.ToShortString());
        }

        /// <summary>
        /// The time zone of this course.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithTimeZone(string timeZone) {
            return PutArr("time_zone", timeZone);
        }

        /// <summary>
        /// Set the storage quota for this course, in megabytes.
        /// </summary>
        /// <param name="quotaMb"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithStorageQuotaMb(int quotaMb) {
            return isEditing ? PutArr("storage_quota_mb", quotaMb.ToString())
                             : this;
        }

        /// <summary>
        /// Make this course available to students immediately.
        /// </summary>
        /// <param name="offerNow"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder OfferImmediately(bool offerNow = true) {
            return Put("offer", offerNow.ToShortString());
        }

        /// <summary>
        /// Enroll the current user immediately as a teacher in this course.
        /// </summary>
        /// <param name="enrollMe"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder EnrollCurrentUser(bool enrollMe = true) {
            return Put("enroll_me", enrollMe.ToShortString());
        }

        /// <summary>
        /// Whether to skip applying the account's course template to this course.
        /// </summary>
        /// <param name="skipTemplate"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder SkipCourseTemplate(bool skipTemplate = true) {
            return Put("skip_course_template", skipTemplate.ToShortString());
        }

        /// <summary>
        /// The course's default view.
        /// </summary>
        /// <param name="defaultView"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithDefaultView(CourseView defaultView) {
            return PutArr("default_view", defaultView.GetApiRepresentation());
        }

        /// <summary>
        /// The course syllabus.
        /// </summary>
        /// <param name="body"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithSyllabusBody(string body) {
            return PutArr("syllabus_body", body);
        }

        /// <summary>
        /// Whether to the course summary on the syllabus page.
        /// </summary>
        /// <param name="showSummary"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithSyllabusCourseSummary(bool showSummary = true) {
            return isEditing ? PutArr("syllabus_course_summary", showSummary.ToShortString())
                             : this;
        }

        /// <summary>
        /// The grading standard for this course.
        /// </summary>
        /// <param name="standard"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithGradingStandard(ulong standard) {
            return PutArr("grading_standard_id", standard.ToString());
        }

        /// <summary>
        /// The grade passback setting for this course.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithGradePassbackSetting(GradePassbackSetting setting) {
            return PutArr("grade_passback_setting", setting.GetApiRepresentation());
        }

        /// <summary>
        /// The course format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithCourseFormat(CourseFormat format) {
            return PutArr("course_format", format.GetApiRepresentation());
        }

        /// <summary>
        /// Whether to require grades to be posted manually in this course.
        /// </summary>
        /// <param name="postManually"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder WithManualGradePosting(bool postManually = true) {
            return PutArr("post_manually", postManually.ToShortString());
        }

        /// <summary>
        /// Sets a course image using a file id in the course.
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithImageId(ulong imageId) {
            return isEditing ? PutArr("image_id", imageId.ToString())
                             : this;
        }

        /// <summary>
        /// Sets a course image using a URL.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithImageUrl(string imageUrl) {
            return isEditing ? PutArr("image_url", imageUrl)
                             : this;
        }

        /// <summary>
        /// Remove the course image.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithRemoveImage(bool remove = true) {
            return isEditing ? PutArr("remove_image", remove.ToShortString())
                             : this;
        }

        /// <summary>
        /// Remove the course banner image.
        /// </summary>
        /// <param name="remove"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithRemoveBannerImage(bool remove = true) {
            return isEditing ? PutArr("remove_banner_image", remove.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets this course as a blueprint course.
        /// </summary>
        /// <param name="blueprint"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithBlueprint(bool blueprint = true) {
            return isEditing ? PutArr("blueprint", blueprint.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets a blueprint restriction for this course.
        /// </summary>
        /// <param name="restriction"></param>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithBlueprintRestriction(BlueprintRestrictionType restriction, bool enabled = true) {
            return isEditing ? PutArr($"blueprint_restrictions][{restriction.GetApiRepresentation()}",
                                      enabled.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets blueprint restrictions for this course.
        /// </summary>
        /// <param name="restrictions"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithBlueprintRestrictions(BlueprintRestrictionTypes restrictions) {
            if (!isEditing) {
                return this;
            }

            foreach (var restriction in restrictions.GetFlagsApiRepresentations()) {
                PutArr($"blueprint_restrictions][{restriction}", true.ToShortString());
            }

            return this;
        }

        /// <summary>
        /// Use object-type specific blueprint restrictions.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithUseBlueprintRestrictionsByObjectType(bool enabled = true) {
            return isEditing ? PutArr("use_blueprint_restrictions_by_object_type", enabled.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets a blueprint restriction by object type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="restriction"></param>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithBlueprintRestrictionByObjectType(BlueprintAssetType objectType,
                                                                  BlueprintRestrictionType restriction,
                                                                  bool enabled = true) {
            return isEditing ? PutArr($"blueprint_restrictions_by_object_type][{objectType.GetApiRepresentation()}][{restriction.GetApiRepresentation()}",
                                      enabled.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets blueprint restrictions by object type.
        /// </summary>
        /// <param name="restrictionsByObjectType"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithBlueprintRestrictionsByObjectType(
            Dictionary<BlueprintAssetType, BlueprintRestrictionTypes> restrictionsByObjectType) {
            if (!isEditing) {
                return this;
            }

            foreach (var objectType in restrictionsByObjectType) {
                foreach (var restriction in objectType.Value.GetFlagsApiRepresentations()) {
                    PutArr($"blueprint_restrictions_by_object_type][{objectType.Key.GetApiRepresentation()}][{restriction}",
                           true.ToShortString());
                }
            }

            return this;
        }

        /// <summary>
        /// Sets this course as a homeroom course.
        /// </summary>
        /// <param name="homeroom"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithHomeroomCourse(bool homeroom = true) {
            return isEditing ? PutArr("homeroom_course", homeroom.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sync enrollments from the homeroom course.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithSyncEnrollmentsFromHomeroom(bool setting = true) {
            return isEditing ? PutArr("sync_enrollments_from_homeroom", setting.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets the homeroom course id for enrollment sync.
        /// </summary>
        /// <param name="homeroomCourseId"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithHomeroomCourseId(string homeroomCourseId) {
            return isEditing ? PutArr("homeroom_course_id", homeroomCourseId)
                             : this;
        }

        /// <summary>
        /// Enable or disable this course as a template.
        /// </summary>
        /// <param name="template"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithTemplate(bool template = true) {
            return isEditing ? PutArr("template", template.ToShortString())
                             : this;
        }

        /// <summary>
        /// Sets the course color (hex).
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithCourseColor(string hexColor) {
            return isEditing ? PutArr("course_color", hexColor)
                             : this;
        }

        /// <summary>
        /// Set a friendly name for the course.
        /// </summary>
        /// <param name="friendlyName"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithFriendlyName(string friendlyName) {
            return isEditing ? PutArr("friendly_name", friendlyName)
                             : this;
        }

        /// <summary>
        /// Enable or disable Course Pacing.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithCoursePacingEnabled(bool enabled = true) {
            return isEditing ? PutArr("enable_course_paces", enabled.ToShortString())
                             : this;
        }

        /// <summary>
        /// Enable or disable conditional release.
        /// </summary>
        /// <param name="enabled"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithConditionalRelease(bool enabled = true) {
            return isEditing ? PutArr("conditional_release", enabled.ToShortString())
                             : this;
        }

        /// <summary>
        /// Try to recover a deleted course from SIS with a matching SIS id before creating this course.
        /// </summary>
        /// <param name="tryToRecover"></param>
        /// <returns>This builder.</returns>
        public CourseBuilder TryToRecoverFromSis(bool tryToRecover = true) {
            return Put("enable_sis_reactivation", tryToRecover.ToShortString());
        }

        /// <summary>
        /// Override SIS stickiness.
        /// </summary>
        /// <param name="overrideStickiness"></param>
        /// <returns>This builder.</returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder WithOverrideSisStickiness(bool overrideStickiness = true) {
            return isEditing ? Put("override_sis_stickiness", overrideStickiness.ToShortString())
                             : this;
        }

        /// <summary>
        /// Perform the action indicated by <paramref name="action"/> when editing a course;
        /// publish, unpublish, conclude, delete, or undelete the course.
        /// </summary>
        /// <param name="action">What <see cref="CourseEditAction">action</see> to take.</param>
        /// <returns></returns>
        /// <remarks>Has no effect when creating a course.</remarks>
        public CourseBuilder TakeAction(CourseEditAction action) {
            return isEditing ? PutArr("event", action.GetApiRepresentation()) 
                             : this;
        }

        /// <summary>
        /// Commit the operation using the fields in this builder.
        /// </summary>
        /// <returns>The newly created or edited course.</returns>
        public Task<Course> Post() {
            if (!isEditing) {
                return api.PostCreateCourse(this);
            }

            Debug.Assert(id != null, nameof(id) + " != null");
            return api.PutEditCourse((ulong) id, this);
        }

        private CourseBuilder Put(string k, string v) {
            Fields[k] = v;
            return this;
        }
        
        private CourseBuilder PutArr(string k, string v) {
            Fields[$"course[{k}]"] = v;
            return this;
        }

        /// <summary>
        /// Actions that can be taken when editing a course.
        /// </summary>
        [PublicAPI]
        public enum CourseEditAction : byte {
            /// <summary>
            /// Make the course invisible to students.
            /// Depending on the state of the course, this action may not be possible.
            /// </summary>
            /// <remarks>This is sometimes called 'claim' internally.</remarks>
            [ApiRepresentation("claim")]
            Unpublish,
            /// <summary>
            /// Make the course visible to students.
            /// Depending on the state of the course, this action may irreversible.
            /// </summary>
            /// <remarks>This is sometimes called 'offer' internally.</remarks>
            [ApiRepresentation("offer")]
            Publish,
            /// <summary>
            /// Conclude the course.
            /// </summary>
            [ApiRepresentation("conclude")]
            Conclude,
            /// <summary>
            /// Delete the course.
            /// </summary>
            [ApiRepresentation("delete")]
            Delete,
            /// <summary>
            /// Undelete the course. This action may not be possible.
            /// </summary>
            [ApiRepresentation("undelete")]
            Undelete
        } 

        /// <summary>
        /// Passback settings for grade sync.
        /// </summary>
        [PublicAPI]
        public enum GradePassbackSetting : byte {
            /// <summary>
            /// Unset the grade passback setting.
            /// </summary>
            [ApiRepresentation("")]
            Unset,
            /// <summary>
            /// Enable nightly sync.
            /// </summary>
            [ApiRepresentation("nightly_sync")]
            NightlySync,
            /// <summary>
            /// Disable grade passback.
            /// </summary>
            [ApiRepresentation("disabled")]
            Disabled
        }

    }
}
