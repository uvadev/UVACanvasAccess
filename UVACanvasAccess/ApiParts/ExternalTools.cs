using System.Collections.Generic;
using JetBrains.Annotations;
using UVACanvasAccess.Model.ExternalTools;
using UVACanvasAccess.Structures.ExternalTools;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        [PublicAPI]
        public enum ExternalToolContextType : byte {
            [ApiRepresentation("groups")]
            Group,
            [ApiRepresentation("courses")]
            Course,
            [ApiRepresentation("accounts")]
            Account
        }

        [PublicAPI]
        public enum ExternalToolPlacementType { 
            [ApiRepresentation("account_navigation")]
            AccountNavigation,
            
            [ApiRepresentation("course_home_sub_navigation")]
            CourseHomeSubNavigation,
            
            [ApiRepresentation("course_navigation")]
            CourseNavigation,
            
            [ApiRepresentation("editor_button")]
            EditorButton,
            
            [ApiRepresentation("homework_submission")]
            HomeworkSubmission,
            
            [ApiRepresentation("migration_selection")]
            MigrationSelection,
            
            [ApiRepresentation("resource_selection")]
            ResourceSelection,
            
            [ApiRepresentation("link_selection")]
            LinkSelection,

            [ApiRepresentation("tool_configuration")]
            ToolConfiguration,
            
            [ApiRepresentation("user_navigation")]
            UserNavigation
        }
        
        public async IAsyncEnumerable<ExternalTool> StreamExternalTools(ulong contextId,
                                                                        ExternalToolContextType contextType,
                                                                        string searchTerm = null,
                                                                        bool? filterSelectable = null,
                                                                        bool? includeParents = null,
                                                                        ExternalToolPlacementType? filterPlacement = null) {
            var baseUrl = $"{contextType.GetApiRepresentation()}/{contextId}/external_tools";
            var args = new[] {
                ("search_term", searchTerm),
                ("selectable", filterSelectable?.ToShortString()),
                ("include_parents", includeParents?.ToShortString()),
                ("placement", filterPlacement?.GetApiRepresentation())
            };
            
            var response = await client.GetAsync(baseUrl + BuildDuplicateKeyQueryString(args));

            await foreach (var tool in StreamDeserializePages<ExternalToolModel>(response)) {
                yield return new ExternalTool(this, tool);
            }
        }
    }
}
