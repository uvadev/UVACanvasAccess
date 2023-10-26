using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.ExternalTools;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.ExternalTools {
    
    [PublicAPI]
    public class ExternalTool : IPrettyPrint {
        private readonly Api api;
        
        public ulong Id { get; }
        
        public string Domain { get; }

        public string Url { get; }

        [Undocumented]
        [UndocumentedRange]
        public string WorkflowState { get; }

        public string ConsumerKey { get; }

        public string Name { get; }

        public string Description { get; }

        public DateTime? CreatedAt { get; }

        public DateTime? UpdatedAt { get; }

        public ExternalToolPrivacyLevel? PrivacyLevel { get; }
 
        public Dictionary<string, string> CustomFields { get; }

        public bool? IsRceFavorite { get; }

        [CanBeNull]
        public AccountNavigationLocation AccountNavigation { get; }

        [CanBeNull]
        public CourseHomeSubNavigationLocation CourseHomeSubNavigation { get; }

        [CanBeNull]
        public CourseNavigationLocation CourseNavigation { get; }

        [CanBeNull]
        public EditorButtonLocation EditorButton { get; }

        [CanBeNull]
        public HomeworkSubmissionLocation HomeworkSubmission { get; }

        [CanBeNull]
        public MigrationSelectionLocation MigrationSelection { get; }

        [CanBeNull]
        public ResourceSelectionLocation ResourceSelection { get; }

        [CanBeNull]
        public LinkSelectionLocation LinkSelection { get; }

        [CanBeNull]
        public ToolConfigurationLocation ToolConfiguration { get; }

        [CanBeNull]
        public UserNavigationLocation UserNavigation { get; }

        public uint? SelectionWidth { get; }

        public uint? SelectionHeight { get; }

        public string IconUrl { get; }

        public bool? NotSelectable { get; }
        
        [CanBeNull]
        [UndocumentedType("Observed to be a string.")]
        public string DeploymentId { get; }

        internal ExternalTool(Api api, ExternalToolModel model) {
            this.api = api;
            Id = model.Id;
            Domain = model.Domain;
            Url = model.Url;
            WorkflowState = model.WorkflowState;
            ConsumerKey = model.ConsumerKey;
            Name = model.Name;
            Description = model.Description;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
            PrivacyLevel = model.PrivacyLevel?.ToApiRepresentedEnum<ExternalToolPrivacyLevel>();
            CustomFields = model.CustomFields;
            IsRceFavorite = model.IsRceFavorite;
            if (model.AccountNavigation != null) {
                AccountNavigation = new AccountNavigationLocation(api, model.AccountNavigation);
            }
            if (model.CourseHomeSubNavigation != null) {
                CourseHomeSubNavigation = new CourseHomeSubNavigationLocation(api, model.CourseHomeSubNavigation);
            }
            if (model.CourseNavigation != null) {
                CourseNavigation = new CourseNavigationLocation(api, model.CourseNavigation);
            }
            if (model.EditorButton != null) {
                EditorButton = new EditorButtonLocation(api, model.EditorButton);
            }
            if (model.HomeworkSubmission != null) {
                HomeworkSubmission = new HomeworkSubmissionLocation(api, model.HomeworkSubmission);
            }
            if (model.MigrationSelection != null) {
                MigrationSelection = new MigrationSelectionLocation(api, model.MigrationSelection);
            }
            if (model.ResourceSelection != null) {
                ResourceSelection = new ResourceSelectionLocation(api, model.ResourceSelection);
            }
            if (model.LinkSelection != null) {
                LinkSelection = new LinkSelectionLocation(api, model.LinkSelection);
            }
            if (model.ToolConfiguration != null) {
                ToolConfiguration = new ToolConfigurationLocation(api, model.ToolConfiguration);
            }
            if (model.UserNavigation != null) {
                UserNavigation = new UserNavigationLocation(api, model.UserNavigation);
            }
            SelectionWidth = model.SelectionWidth;
            SelectionHeight = model.SelectionHeight;
            IconUrl = model.IconUrl;
            NotSelectable = model.NotSelectable;
        }

        public string ToPrettyString() {
            return "ExternalTool {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Domain)}: {Domain}," +
                   $"\n{nameof(Url)}: {Url}," +
                   $"\n{nameof(WorkflowState)}: {WorkflowState}," +
                   $"\n{nameof(ConsumerKey)}: {ConsumerKey}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(Description)}: {Description}," +
                   $"\n{nameof(CreatedAt)}: {CreatedAt}," +
                   $"\n{nameof(UpdatedAt)}: {UpdatedAt}," +
                   $"\n{nameof(PrivacyLevel)}: {PrivacyLevel}," +
                   $"\n{nameof(CustomFields)}: {CustomFields}," +
                   $"\n{nameof(IsRceFavorite)}: {IsRceFavorite}," +
                   $"\n{nameof(AccountNavigation)}: {AccountNavigation}," +
                   $"\n{nameof(CourseHomeSubNavigation)}: {CourseHomeSubNavigation}," +
                   $"\n{nameof(CourseNavigation)}: {CourseNavigation}," +
                   $"\n{nameof(EditorButton)}: {EditorButton}," +
                   $"\n{nameof(HomeworkSubmission)}: {HomeworkSubmission}," +
                   $"\n{nameof(MigrationSelection)}: {MigrationSelection}," +
                   $"\n{nameof(ResourceSelection)}: {ResourceSelection}," +
                   $"\n{nameof(LinkSelection)}: {LinkSelection}," +
                   $"\n{nameof(ToolConfiguration)}: {ToolConfiguration}," +
                   $"\n{nameof(UserNavigation)}: {UserNavigation}," +
                   $"\n{nameof(SelectionWidth)}: {SelectionWidth}," +
                   $"\n{nameof(SelectionHeight)}: {SelectionHeight}," +
                   $"\n{nameof(IconUrl)}: {IconUrl}," +
                   $"\n{nameof(NotSelectable)}: {NotSelectable}," +
                   $"\n{nameof(DeploymentId)}: {DeploymentId}").Indent(4) + 
                   "\n}";
        }
    }
}
