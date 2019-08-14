using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Sections;
using UVACanvasAccess.Structures.Sections;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [PublicAPI]
        [Flags]
        public enum SectionIncludes : byte {
            [ApiRepresentation("students")]
            Students = 1 << 0,
            [ApiRepresentation("avatar_url")]
            AvatarUrl = 1 << 1,
            [ApiRepresentation("enrollments")]
            Enrollments = 1 << 2,
            [ApiRepresentation("total_students")]
            TotalStudents = 1 << 3,
            [ApiRepresentation("passback_status")]
            PassbackStatus = 1 << 4,
            Everything = byte.MaxValue
        }
        
        public async IAsyncEnumerable<Section> StreamCourseSections(ulong courseId, SectionIncludes includes = 0) {

            var i = BuildDuplicateKeyQueryString(includes.GetFlagsApiRepresentations()
                                                         .Select(s => ("include[]", s))
                                                         .ToArray());

            var response = await _client.GetAsync($"courses/{courseId}/sections" + i);
            
            await foreach (var model in StreamDeserializePages<SectionModel>(response)) {
                yield return new Section(this, model);
            }
        }
    }
}
