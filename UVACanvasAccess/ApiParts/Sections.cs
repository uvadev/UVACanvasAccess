using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
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

            var response = await client.GetAsync($"courses/{courseId}/sections" + i);
            
            await foreach (var model in StreamDeserializePages<SectionModel>(response)) {
                yield return new Section(this, model);
            }
        }

        public async Task<Section> GetSection(ulong courseId, ulong sectionId) {
            var response = await client.GetAsync($"courses/{courseId}/sections/{sectionId}");
            var model = JsonConvert.DeserializeObject<SectionModel>(await response.Content.ReadAsStringAsync());
            return new Section(this, model);
        }

        public async Task<Section> CrossListSection(ulong sectionId, ulong targetCourseId) {
            var response = await client.PostAsync($"sections/{sectionId}/crosslist/{targetCourseId}", null);
            var model = JsonConvert.DeserializeObject<SectionModel>(await response.Content.ReadAsStringAsync());
            return new Section(this, model);
        }

        public async Task<Section> UnCrossListSection(ulong sectionId) {
            var response = await client.DeleteAsync($"sections/{sectionId}/crosslist");
            var model = JsonConvert.DeserializeObject<SectionModel>(await response.Content.ReadAsStringAsync());
            return new Section(this, model);
        }
    }
}
