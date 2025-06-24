using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Quizzes;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {

        [PublicAPI]
        public struct QuizLevelAccommodations {
            [JsonProperty("user_id")]
            public ulong UserId { get; }
            
            [JsonProperty("extra_time", NullValueHandling = NullValueHandling.Ignore)]
            public uint? ExtraTime { get; }
            
            [JsonProperty("extra_attempts", NullValueHandling = NullValueHandling.Ignore)]
            public uint? ExtraAttempts { get; }
            
            [JsonProperty("reduce_choices_enabled", NullValueHandling = NullValueHandling.Ignore)]
            public bool? ReduceChoices { get; }
            
            public QuizLevelAccommodations(ulong userId,
                                           uint? extraTime = null,
                                           uint? extraAttempts = null,
                                           bool? reduceChoices = null) {
                UserId = userId;
                ExtraTime = extraTime;
                ExtraAttempts = extraAttempts;
                ReduceChoices = reduceChoices;
            }
        }
        
        [PublicAPI]
        public struct CourseLevelQuizAccommodations {
            [JsonProperty("user_id")]
            public ulong UserId { get; }
            
            [JsonProperty("extra_time", NullValueHandling = NullValueHandling.Ignore)]
            public uint? ExtraTime { get; }
            
            [JsonProperty("apply_to_in_progress_quiz_sessions", NullValueHandling = NullValueHandling.Ignore)]
            public bool? ApplyToInProgressSessions { get; }
            
            [JsonProperty("reduce_choices_enabled", NullValueHandling = NullValueHandling.Ignore)]
            public bool? ReduceChoices { get; }
            
            public CourseLevelQuizAccommodations(ulong userId, 
                                                 uint? extraTime = null,
                                                 bool? applyToInProgressSessions = null,
                                                 bool? reduceChoices = null) {
                UserId = userId;
                ExtraTime = extraTime;
                ApplyToInProgressSessions = applyToInProgressSessions;
                ReduceChoices = reduceChoices;
            }
        }
        
        public async Task<QuizAccommodationsResponse> SetQuizLevelAccommodations(ulong courseId,
                                                                                 ulong assignmentId,
                                                                                 IEnumerable<QuizLevelAccommodations> accommodations) {
            var args = HttpContentFromJson(JArray.FromObject(accommodations));
            
            var response = await client.PostAsync($"../quiz/v1/courses/{courseId}/quizzes/{assignmentId}/accommodations" + BuildQueryString(), args);
            var model = JsonConvert.DeserializeObject<QuizAccommodationsResponseModel>(await response.Content.ReadAsStringAsync());
            return new QuizAccommodationsResponse(this, model);
        }
        
        public async Task<QuizAccommodationsResponse> SetCourseLevelQuizAccommodations(ulong courseId,
                                                                                       IEnumerable<CourseLevelQuizAccommodations> accommodations) {
            var args = HttpContentFromJson(JArray.FromObject(accommodations));
            Console.WriteLine(JArray.FromObject(accommodations).ToString());
            
            var response = await client.PostAsync($"../quiz/v1/courses/{courseId}/accommodations" + BuildQueryString(), args);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            var model = JsonConvert.DeserializeObject<QuizAccommodationsResponseModel>(await response.Content.ReadAsStringAsync());
            return new QuizAccommodationsResponse(this, model);
        }
    }
}
