using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Discussions;
using UVACanvasAccess.Model.Submissions;

namespace UVACanvasAccess.Model.Assignments {
    
    internal class AssignmentModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [JsonProperty("due_at")]
        public DateTime? DueAt { get; set; }
        
        [JsonProperty("lock_at")]
        public DateTime? LockAt { get; set; }
        
        [JsonProperty("unlock_at")]
        public DateTime? UnlockAt { get; set; }
        
        [JsonProperty("has_overrides")]
        public bool HasOverrides { get; set; }
        
        [CanBeNull]
        [JsonProperty("all_dates")]
        public IEnumerable<AssignmentDateModel> AllDates { get; set; }
        
        [JsonProperty("course_id")]
        public ulong CourseId { get; set; }
        
        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
        
        [JsonProperty("submissions_download_url")]
        public string SubmissionsDownloadUrl { get; set; }
        
        [JsonProperty("assignment_group_id")]
        public ulong AssignmentGroupId { get; set; }
        
        [JsonProperty("due_date_required")]
        public bool DueDateRequired { get; set; }
        
        [CanBeNull]
        [JsonProperty("allowed_extensions")]
        public IEnumerable<string> AllowedExtensions { get; set; }
        
        [JsonProperty("max_name_length")]
        public uint MaxNameLength { get; set; }
        
        [JsonProperty("turnitin_enabled")]
        public bool? TurnitinEnabled { get; set; }
        
        [JsonProperty("vericite_enabled")]
        public bool? VeriCiteEnabled { get; set; } 
        
        [CanBeNull]
        [JsonProperty("turnitin_settings")]
        public TurnitinSettingsModel TurnitinSettings { get; set; }
        
        [JsonProperty("grade_group_students_individually")]
        public bool GradeGroupStudentsIndividually { get; set; }
        
        [CanBeNull]
        [JsonProperty("external_tool_tag_attributes")]
        public ExternalToolTagAttributesModel ExternalToolTagAttributes { get; set; }
        
        [JsonProperty("peer_reviews")]
        public bool PeerReviews { get; set; }
        
        [JsonProperty("automatic_peer_reviews")]
        public bool AutomaticPeerReviews { get; set; }
        
        [JsonProperty("peer_review_count")]
        public uint? PeerReviewCount { get; set; }
        
        [JsonProperty("peer_reviews_assign_at")]
        public DateTime? PeerReviewsAssignAt { get; set; }
        
        [JsonProperty("intra_group_peer_reviews")]
        public bool? IntraGroupPeerReviews { get; set; }
        
        [JsonProperty("group_category_id")]
        public ulong? GroupCategoryId { get; set; }
        
        [JsonProperty("needs_grading_count")]
        public uint? NeedsGradingCount { get; set; }
        
        [CanBeNull]
        [JsonProperty("needs_grading_count_be_section")]
        public IEnumerable<NeedsGradingCountModel> NeedsGradingCountBySection { get; set; }
        
        [JsonProperty("position")]
        public ulong Position { get; set; }
        
        [JsonProperty("post_to_sis")]
        public bool? PostToSis { get; set; }
        
        [CanBeNull]
        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }
        
        [CanBeNull]
        [JsonProperty("integration_data")]
        public object IntegrationData { get; set; }
        
        [JsonProperty("muted")]
        public bool? Muted { get; set; }
        
        [JsonProperty("points_possible")]
        public uint PointsPossible { get; set; }
        
        [JsonProperty("submission_types")]
        public IEnumerable<string> SubmissionTypes { get; set; }
        
        [JsonProperty("has_submitted_submissions")]
        public bool HasSubmittedSubmissions { get; set; }
        
        [JsonProperty("grading_type")]
        public string GradingType { get; set; }

        [JsonProperty("grading_standard_id")]
        public ulong? GradingStandardId { get; set; }

        [JsonProperty("published")]
        public bool Published { get; set; }
        
        [JsonProperty("unpublishable")]
        public bool Unpublishable { get; set; }
        
        [JsonProperty("only_visible_to_overrides")]
        public bool OnlyVisibleToOverrides { get; set; }
        
        [JsonProperty("locked_for_user")]
        public bool LockedForUser { get; set; }
        
        [CanBeNull] 
        [JsonProperty("lock_info")]
        public LockInfoModel LockInfo { get; set; }
        
        [CanBeNull] 
        [JsonProperty("lock_explanation")]
        public string LockExplanation { get; set; }
        
        [JsonProperty("quiz_id")]
        public ulong? QuizId { get; set; }
        
        [JsonProperty("anonymous_submissions")]
        public bool? AnonymousSubmissions { get; set; }
        
        [CanBeNull]
        [JsonProperty("discussion_topic")]
        public DiscussionTopicModel DiscussionTopic { get; set; }
        
        [JsonProperty("freeze_on_copy")]
        public bool? FreezeOnCopy { get; set; }
        
        [JsonProperty("frozen")]
        public bool? Frozen { get; set; }
        
        [CanBeNull]
        [JsonProperty("frozen_attributes")]
        public IEnumerable<string> FrozenAttributes { get; set; }
        
        [CanBeNull]
        [JsonProperty("submission")]
        public SubmissionModel Submission { get; set; }
        
        [JsonProperty("use_rubric_for_grading")]
        public bool? UseRubricForGrading { get; set; }
        
        [CanBeNull]
        [JsonProperty("rubric_settings")]
        public object RubricSettings { get; set; } // again, docs give no concrete type.
        
        [CanBeNull]
        [JsonProperty("rubric")]
        public IEnumerable<RubricCriteriaModel> Rubric { get; set; } 
        
        [CanBeNull]
        [JsonProperty("assignment_visibility")]
        public IEnumerable<ulong> AssignmentVisibility { get; set; }
        
        [CanBeNull]
        [JsonProperty("overrides")]
        public IEnumerable<AssignmentOverrideModel> Overrides { get; set; }
        
        [JsonProperty("omit_from_final_grade")]
        public bool? OmitFromFinalGrade { get; set; }
        
        [JsonProperty("moderated_grading")]
        public bool ModeratedGrading { get; set; }
        
        [JsonProperty("grader_count")]
        public uint GraderCount { get; set; }
        
        [JsonProperty("final_grader_id")]
        public ulong? FinalGraderId { get; set; }
        
        [JsonProperty("grader_comments_visible_to_graders")]
        public bool? GraderCommentsVisibleToGraders { get; set; }
        
        [JsonProperty("graders_anonymous_to_graders")]
        public bool? GradersAnonymousToGraders { get; set; }
        
        [JsonProperty("grader_names_anonymous_to_final_grader")]
        public bool? GraderNamesVisibleToFinalGrader { get; set; }
        
        [JsonProperty("anonymous_grading")]
        public bool? AnonymousGrading { get; set; }
        
        [JsonProperty("allowed_attempts")]
        public int AllowedAttempts { get; set; }
    }
}