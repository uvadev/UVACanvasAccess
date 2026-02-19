using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Builder for quiz question groups.
    /// </summary>
    [PublicAPI]
    public class QuizQuestionGroupBuilder {
        private readonly Api api;
        private readonly bool isEditing;
        private readonly ulong courseId;
        private readonly ulong quizId;
        private readonly ulong? groupId;
        
        private string name;
        private uint? pickCount;
        private decimal? questionPoints;

        internal QuizQuestionGroupBuilder(Api api, bool isEditing, ulong courseId, ulong quizId, ulong? groupId = null) {
            this.api = api;
            this.isEditing = isEditing;
            this.courseId = courseId;
            this.quizId = quizId;
            this.groupId = groupId;
        }
        
        public QuizQuestionGroupBuilder WithName([NotNull] string groupName) {
            name = groupName;
            return this;
        }
        
        public QuizQuestionGroupBuilder WithPickCount(uint pickCount) {
            this.pickCount = pickCount;
            return this;
        }
        
        public QuizQuestionGroupBuilder WithQuestionPoints(decimal points) {
            questionPoints = points;
            return this;
        }
        
        internal IEnumerable<(string, string)> ToParams() {
            var args = new List<(string, string)>();
            
            if (name != null) {
                args.Add(("quiz_groups[][name]", name));
            }
            
            if (pickCount != null) {
                args.Add(("quiz_groups[][pick_count]", pickCount.ToString()));
            }
            
            if (questionPoints != null) {
                args.Add(("quiz_groups[][question_points]", questionPoints.ToString()));
            }
            
            return args;
        }
        
        /// <summary>
        /// Performs the operation using the fields in this builder.
        /// </summary>
        /// <returns>The created or updated quiz question groups.</returns>
        public Task<IEnumerable<QuizQuestionGroup>> Post() {
            if (!isEditing) {
                return api.PostCreateQuizQuestionGroup(courseId, quizId, this);
            }
            
            Debug.Assert(groupId != null, nameof(groupId) + " != null");
            return api.PutUpdateQuizQuestionGroup(courseId, quizId, (ulong) groupId, this);

        }
    }
}
