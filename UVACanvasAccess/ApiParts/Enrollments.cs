using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UVACanvasAccess.Model.Enrollments;
using UVACanvasAccess.Structures.Enrollments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        private Task<HttpResponseMessage> RawCreateEnrollment(ulong courseId, 
                                                              ulong userId, 
                                                              string enrollmentType,
                                                              ulong? roleId,
                                                              string enrollmentState,
                                                              ulong? courseSectionId,
                                                              bool? limitPrivilegesToSection,
                                                              bool? notify,
                                                              string selfEnrollmentCode,
                                                              bool? selfEnrolled,
                                                              ulong? associatedUserId) {
            var args = new[] {
                                 ("user_id", userId.ToString()),
                                 ("type", enrollmentType),
                                 ("role_id", roleId?.ToString()),
                                 ("enrollment_state", enrollmentState),
                                 ("course_section_id", courseSectionId?.ToString()),
                                 ("limit_privileges_to_course_section", limitPrivilegesToSection?.ToShortString()),
                                 ("notify", notify?.ToShortString()),
                                 ("self_enrollment_code", selfEnrollmentCode),
                                 ("self_enrolled", selfEnrolled?.ToShortString()),
                                 ("associated_user_id", associatedUserId?.ToString())
                             }.KeySelect(k => $"enrollment[{k}]");

            return _client.PostAsync($"courses/{courseId}/enrollments", BuildHttpArguments(args));
        }
        
        // todo replace some of these string params with enums

        public async Task<Enrollment> CreateEnrollment(ulong courseId,
                                                       ulong userId,
                                                       string enrollmentType,
                                                       ulong? roleId = null,
                                                       string enrollmentState = null,
                                                       ulong? courseSectionId = null,
                                                       bool? limitPrivilegesToSection = null,
                                                       bool? notify = null,
                                                       string selfEnrollmentCode = null,
                                                       bool? selfEnrolled = null,
                                                       ulong? associatedUserId = null) {
            
            var response = await RawCreateEnrollment(courseId,
                                                     userId,
                                                     enrollmentType,
                                                     roleId,
                                                     enrollmentState,
                                                     courseSectionId,
                                                     limitPrivilegesToSection,
                                                     notify,
                                                     selfEnrollmentCode,
                                                     selfEnrolled,
                                                     associatedUserId);
            var model = JsonConvert.DeserializeObject<EnrollmentModel>(await response.Content.ReadAsStringAsync());
            return new Enrollment(this, model);
        }
    }
}