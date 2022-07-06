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
        
        /// <summary>
        /// Additional data which can be included in <see cref="Section"/> responses.
        /// </summary>
        [PublicAPI]
        [Flags]
        public enum SectionIncludes : byte {
            /// <summary>
            /// Include the students.
            /// </summary>
            [ApiRepresentation("students")]
            Students = 1 << 0,
            /// <summary>
            /// Include avatar URLs.
            /// </summary>
            [ApiRepresentation("avatar_url")]
            AvatarUrl = 1 << 1,
            /// <summary>
            /// Include the enrollments.
            /// </summary>
            [ApiRepresentation("enrollments")]
            Enrollments = 1 << 2,
            /// <summary>
            /// Include the count of students.
            /// </summary>
            [ApiRepresentation("total_students")]
            TotalStudents = 1 << 3,
            /// <summary>
            /// Include the passback status.
            /// </summary>
            [ApiRepresentation("passback_status")]
            PassbackStatus = 1 << 4,
            /// <summary>
            /// Include all optional data.
            /// </summary>
            Everything = byte.MaxValue
        }
        
        /// <summary>
        /// Streams all sections in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="includes">Optional data to include with each <see cref="Section"/> object.</param>
        /// <returns>The stream of sections.</returns>
        public async IAsyncEnumerable<Section> StreamCourseSections(ulong courseId, SectionIncludes includes = 0) {

            var i = BuildDuplicateKeyQueryString(includes.GetFlagsApiRepresentations()
                                                         .Select(s => ("include[]", s))
                                                         .ToArray());

            var response = await client.GetAsync($"courses/{courseId}/sections" + i);
            
            await foreach (var model in StreamDeserializePages<SectionModel>(response)) {
                yield return new Section(this, model);
            }
        }

        /// <summary>
        /// Gets a single section by course and section id.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="sectionId">The section id.</param>
        /// <returns>The section.</returns>
        public async Task<Section> GetSection(ulong courseId, ulong sectionId) {
            var response = await client.GetAsync($"courses/{courseId}/sections/{sectionId}");
            var model = JsonConvert.DeserializeObject<SectionModel>(await response.Content.ReadAsStringAsync());
            return new Section(this, model);
        }

        /// <summary>
        /// Cross-lists a section.
        /// </summary>
        /// <param name="sectionId">The section id.</param>
        /// <param name="targetCourseId">The course to cross-list with.</param>
        /// <returns>The section.</returns>
        public async Task<Section> CrossListSection(ulong sectionId, ulong targetCourseId) {
            var response = await client.PostAsync($"sections/{sectionId}/crosslist/{targetCourseId}", null);
            var model = JsonConvert.DeserializeObject<SectionModel>(await response.Content.ReadAsStringAsync());
            return new Section(this, model);
        }

        /// <summary>
        /// Removes the cross-listing of a section.
        /// </summary>
        /// <param name="sectionId">The section id.</param>
        /// <returns>The section.</returns>
        public async Task<Section> UnCrossListSection(ulong sectionId) {
            var response = await client.DeleteAsync($"sections/{sectionId}/crosslist");
            var model = JsonConvert.DeserializeObject<SectionModel>(await response.Content.ReadAsStringAsync());
            return new Section(this, model);
        }
    }
}
