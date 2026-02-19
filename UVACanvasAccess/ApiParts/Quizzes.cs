using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Structures.Files;
using UVACanvasAccess.Structures.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.ApiParts {
    public partial class Api {
        
        /// <summary>
        /// Lists the quizzes in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="searchTerm">(Optional) The partial quiz title to search for.</param>
        /// <returns>The list of quizzes.</returns>
        public async Task<IEnumerable<Quiz>> ListCourseQuizzes(ulong courseId, [CanBeNull] string searchTerm = null) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes" + 
                                                  BuildQueryString(("search_term", searchTerm)));

            var models = await AccumulateDeserializePages<QuizModel>(response);
            return models.Select(m => new Quiz(this, m));
        }

        /// <summary>
        /// Streams the quizzes in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="searchTerm">(Optional) The partial quiz title to search for.</param>
        /// <returns>The stream of quizzes.</returns>
        public async IAsyncEnumerable<Quiz> StreamCourseQuizzes(ulong courseId, [CanBeNull] string searchTerm = null) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes" + 
                                                  BuildQueryString(("search_term", searchTerm)));

            await foreach (var model in StreamDeserializePages<QuizModel>(response)) {
                yield return new Quiz(this, model);
            }
        }

        /// <summary>
        /// Gets a single quiz by id.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="quizId">The quiz id.</param>
        /// <returns>The quiz.</returns>
        public async Task<Quiz> GetQuiz(ulong courseId, ulong quizId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new Quiz(this, model);
        }

        internal async Task<Quiz> PostCreateQuiz(ulong courseId, [NotNull] QuizBuilder builder) {
            var response = await client.PostAsync($"courses/{courseId}/quizzes", 
                                                   BuildHttpArguments(builder.ToParams(includeNotifyOfUpdate: false)));
            var model = JsonConvert.DeserializeObject<QuizModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new Quiz(this, model);
        }

        /// <summary>
        /// Returns a <see cref="QuizBuilder"/> for creating a quiz in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="title">The quiz title.</param>
        /// <returns>The builder.</returns>
        public QuizBuilder CreateQuiz(ulong courseId, [NotNull] string title) {
            return new QuizBuilder(this, false, courseId).WithTitle(title);
        }

        internal async Task<Quiz> PutUpdateQuiz(ulong courseId, ulong quizId, [NotNull] QuizBuilder builder) {
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}",
                                                  BuildHttpArguments(builder.ToParams(includeNotifyOfUpdate: true)));
            var model = JsonConvert.DeserializeObject<QuizModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new Quiz(this, model);
        }

        /// <summary>
        /// Returns a <see cref="QuizBuilder"/> for updating a quiz in a course.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="quizId">The quiz id.</param>
        /// <returns>The builder.</returns>
        public QuizBuilder UpdateQuiz(ulong courseId, ulong quizId) {
            return new QuizBuilder(this, true, courseId, quizId);
        }

        /// <summary>
        /// Deletes a quiz.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="quizId">The quiz id.</param>
        /// <returns>The deleted quiz.</returns>
        public async Task<Quiz> DeleteQuiz(ulong courseId, ulong quizId) {
            var response = await client.DeleteAsync($"courses/{courseId}/quizzes/{quizId}");
            var model = JsonConvert.DeserializeObject<QuizModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new Quiz(this, model);
        }

        /// <summary>
        /// Reorders quiz questions and question groups within a quiz.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="quizId">The quiz id.</param>
        /// <param name="order">The ordered list of quiz items.</param>
        public async Task ReorderQuizItems(ulong courseId, ulong quizId, [NotNull] IEnumerable<QuizReorderItem> order) {
            var args = order.Select(item => (("order[][id]", item.Id.ToString()),
                                             ("order[][type]", item.Type.GetApiRepresentation())))
                            .Interleave();

            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/reorder",
                                                   BuildHttpArguments(args));
            response.AssertSuccess();
        }

        /// <summary>
        /// Validates a quiz access code.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="quizId">The quiz id.</param>
        /// <param name="accessCode">The access code.</param>
        /// <returns>True if the access code is valid; otherwise false.</returns>
        public async Task<bool> ValidateQuizAccessCode(ulong courseId, ulong quizId, [NotNull] string accessCode) {
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/validate_access_code",
                                                   BuildHttpArguments(new[] { ("access_code", accessCode) }));
            var payload = await response.AssertSuccess().Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(payload);
        }

        /// <summary>
        /// Lists the questions in a quiz.
        /// </summary>
        public async Task<IEnumerable<QuizQuestion>> ListQuizQuestions(ulong courseId, ulong quizId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/questions" + BuildQueryString());
            var models = await AccumulateDeserializePages<QuizQuestionModel>(response);
            return models.Select(m => QuizQuestion.Create(this, m));
        }

        /// <summary>
        /// Streams the questions in a quiz.
        /// </summary>
        public async IAsyncEnumerable<QuizQuestion> StreamQuizQuestions(ulong courseId, ulong quizId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/questions" + BuildQueryString());

            await foreach (var model in StreamDeserializePages<QuizQuestionModel>(response)) {
                yield return QuizQuestion.Create(this, model);
            }
        }

        /// <summary>
        /// Gets a single quiz question.
        /// </summary>
        public async Task<QuizQuestion> GetQuizQuestion(ulong courseId, ulong quizId, ulong questionId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/questions/{questionId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizQuestionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return QuizQuestion.Create(this, model);
        }
        
        internal async Task<QuizQuestion> PostCreateQuizQuestion(ulong courseId, ulong quizId, [NotNull] QuizQuestionBuilder builder) {
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/questions",
                                                   BuildHttpJsonBody(builder.ToJson()));
            var model = JsonConvert.DeserializeObject<QuizQuestionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return QuizQuestion.Create(this, model);
        }
        
        /// <summary>
        /// Returns a <see cref="CalculatedQuizQuestionBuilder"/> for creating a calculated quiz question.
        /// </summary>
        public CalculatedQuizQuestionBuilder CreateCalculatedQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (CalculatedQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.CalculatedQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns an <see cref="EssayQuizQuestionBuilder"/> for creating an essay quiz question.
        /// </summary>
        public EssayQuizQuestionBuilder CreateEssayQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (EssayQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.EssayQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="FileUploadQuizQuestionBuilder"/> for creating a file upload quiz question.
        /// </summary>
        public FileUploadQuizQuestionBuilder CreateFileUploadQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (FileUploadQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.FileUploadQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="FillInMultipleBlanksQuizQuestionBuilder"/> for creating a fill-in-multiple-blanks quiz question.
        /// </summary>
        public FillInMultipleBlanksQuizQuestionBuilder CreateFillInMultipleBlanksQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (FillInMultipleBlanksQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.FillInMultipleBlanksQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MatchingQuizQuestionBuilder"/> for creating a matching quiz question.
        /// </summary>
        public MatchingQuizQuestionBuilder CreateMatchingQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MatchingQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.MatchingQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MultipleAnswersQuizQuestionBuilder"/> for creating a multiple-answers quiz question.
        /// </summary>
        public MultipleAnswersQuizQuestionBuilder CreateMultipleAnswersQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MultipleAnswersQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.MultipleAnswersQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MultipleChoiceQuizQuestionBuilder"/> for creating a multiple-choice quiz question.
        /// </summary>
        public MultipleChoiceQuizQuestionBuilder CreateMultipleChoiceQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MultipleChoiceQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.MultipleChoiceQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MultipleDropdownsQuizQuestionBuilder"/> for creating a multiple-dropdowns quiz question.
        /// </summary>
        public MultipleDropdownsQuizQuestionBuilder CreateMultipleDropdownsQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MultipleDropdownsQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.MultipleDropdownsQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="NumericalQuizQuestionBuilder"/> for creating a numerical quiz question.
        /// </summary>
        public NumericalQuizQuestionBuilder CreateNumericalQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (NumericalQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.NumericalQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="ShortAnswerQuizQuestionBuilder"/> for creating a short-answer quiz question.
        /// </summary>
        public ShortAnswerQuizQuestionBuilder CreateShortAnswerQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (ShortAnswerQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.ShortAnswerQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="TextOnlyQuizQuestionBuilder"/> for creating a text-only quiz question.
        /// </summary>
        public TextOnlyQuizQuestionBuilder CreateTextOnlyQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (TextOnlyQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.TextOnlyQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="TrueFalseQuizQuestionBuilder"/> for creating a true/false quiz question.
        /// </summary>
        public TrueFalseQuizQuestionBuilder CreateTrueFalseQuizQuestion(ulong courseId, ulong quizId, [NotNull] string questionName, [NotNull] string questionText) {
            return (TrueFalseQuizQuestionBuilder) QuizQuestionBuilder.Create(this, false, courseId, quizId, null, QuizQuestionType.TrueFalseQuestion, questionName, questionText);
        }
        
        internal async Task<QuizQuestion> PutUpdateQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] QuizQuestionBuilder builder) {
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}/questions/{questionId}",
                                                  BuildHttpJsonBody(builder.ToJson()));
            var model = JsonConvert.DeserializeObject<QuizQuestionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return QuizQuestion.Create(this, model);
        }
        
        /// <summary>
        /// Returns a <see cref="CalculatedQuizQuestionBuilder"/> for updating a calculated quiz question.
        /// </summary>
        public CalculatedQuizQuestionBuilder UpdateCalculatedQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (CalculatedQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.CalculatedQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns an <see cref="EssayQuizQuestionBuilder"/> for updating an essay quiz question.
        /// </summary>
        public EssayQuizQuestionBuilder UpdateEssayQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (EssayQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.EssayQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="FileUploadQuizQuestionBuilder"/> for updating a file upload quiz question.
        /// </summary>
        public FileUploadQuizQuestionBuilder UpdateFileUploadQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (FileUploadQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.FileUploadQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="FillInMultipleBlanksQuizQuestionBuilder"/> for updating a fill-in-multiple-blanks quiz question.
        /// </summary>
        public FillInMultipleBlanksQuizQuestionBuilder UpdateFillInMultipleBlanksQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (FillInMultipleBlanksQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.FillInMultipleBlanksQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MatchingQuizQuestionBuilder"/> for updating a matching quiz question.
        /// </summary>
        public MatchingQuizQuestionBuilder UpdateMatchingQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MatchingQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.MatchingQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MultipleAnswersQuizQuestionBuilder"/> for updating a multiple-answers quiz question.
        /// </summary>
        public MultipleAnswersQuizQuestionBuilder UpdateMultipleAnswersQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MultipleAnswersQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.MultipleAnswersQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MultipleChoiceQuizQuestionBuilder"/> for updating a multiple-choice quiz question.
        /// </summary>
        public MultipleChoiceQuizQuestionBuilder UpdateMultipleChoiceQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MultipleChoiceQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.MultipleChoiceQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="MultipleDropdownsQuizQuestionBuilder"/> for updating a multiple-dropdowns quiz question.
        /// </summary>
        public MultipleDropdownsQuizQuestionBuilder UpdateMultipleDropdownsQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (MultipleDropdownsQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.MultipleDropdownsQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="NumericalQuizQuestionBuilder"/> for updating a numerical quiz question.
        /// </summary>
        public NumericalQuizQuestionBuilder UpdateNumericalQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (NumericalQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.NumericalQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="ShortAnswerQuizQuestionBuilder"/> for updating a short-answer quiz question.
        /// </summary>
        public ShortAnswerQuizQuestionBuilder UpdateShortAnswerQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (ShortAnswerQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.ShortAnswerQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="TextOnlyQuizQuestionBuilder"/> for updating a text-only quiz question.
        /// </summary>
        public TextOnlyQuizQuestionBuilder UpdateTextOnlyQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (TextOnlyQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.TextOnlyQuestion, questionName, questionText);
        }

        /// <summary>
        /// Returns a <see cref="TrueFalseQuizQuestionBuilder"/> for updating a true/false quiz question.
        /// </summary>
        public TrueFalseQuizQuestionBuilder UpdateTrueFalseQuizQuestion(ulong courseId, ulong quizId, ulong questionId, [NotNull] string questionName, [NotNull] string questionText) {
            return (TrueFalseQuizQuestionBuilder) QuizQuestionBuilder.Create(this, true, courseId, quizId, questionId, QuizQuestionType.TrueFalseQuestion, questionName, questionText);
        }
        
        /// <summary>
        /// Deletes a quiz question.
        /// </summary>
        public async Task<QuizQuestion> DeleteQuizQuestion(ulong courseId, ulong quizId, ulong questionId) {
            var response = await client.DeleteAsync($"courses/{courseId}/quizzes/{quizId}/questions/{questionId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizQuestionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return QuizQuestion.Create(this, model);
        }

        /// <summary>
        /// Gets a quiz question group.
        /// </summary>
        public async Task<QuizQuestionGroup> GetQuizQuestionGroup(ulong courseId, ulong quizId, ulong groupId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/groups/{groupId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizQuestionGroupModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizQuestionGroup(this, model);
        }
        
        internal async Task<IEnumerable<QuizQuestionGroup>> PostCreateQuizQuestionGroup(ulong courseId, ulong quizId, [NotNull] QuizQuestionGroupBuilder builder) {
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/groups",
                                                   BuildHttpArguments(builder.ToParams()));
            var model = JsonConvert.DeserializeObject<QuizQuestionGroupListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return model.QuizGroups.Select(g => new QuizQuestionGroup(this, g));
        }
        
        /// <summary>
        /// Returns a <see cref="QuizQuestionGroupBuilder"/> for creating a quiz question group.
        /// </summary>
        public QuizQuestionGroupBuilder CreateQuizQuestionGroup(ulong courseId, ulong quizId) {
            return new QuizQuestionGroupBuilder(this, false, courseId, quizId);
        }
        
        internal async Task<IEnumerable<QuizQuestionGroup>> PutUpdateQuizQuestionGroup(ulong courseId, ulong quizId, ulong groupId, [NotNull] QuizQuestionGroupBuilder builder) {
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}/groups/{groupId}",
                                                  BuildHttpArguments(builder.ToParams()));
            var model = JsonConvert.DeserializeObject<QuizQuestionGroupListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return model.QuizGroups.Select(g => new QuizQuestionGroup(this, g));
        }
        
        /// <summary>
        /// Returns a <see cref="QuizQuestionGroupBuilder"/> for updating a quiz question group.
        /// </summary>
        public QuizQuestionGroupBuilder UpdateQuizQuestionGroup(ulong courseId, ulong quizId, ulong groupId) {
            return new QuizQuestionGroupBuilder(this, true, courseId, quizId, groupId);
        }
        
        /// <summary>
        /// Deletes a quiz question group.
        /// </summary>
        public async Task<QuizQuestionGroup> DeleteQuizQuestionGroup(ulong courseId, ulong quizId, ulong groupId) {
            var response = await client.DeleteAsync($"courses/{courseId}/quizzes/{quizId}/groups/{groupId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizQuestionGroupModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizQuestionGroup(this, model);
        }
        
        /// <summary>
        /// Reorders the questions within a question group.
        /// </summary>
        public async Task ReorderQuizQuestionGroupQuestions(ulong courseId, ulong quizId, ulong groupId, [NotNull] IEnumerable<ulong> questionIds) {
            var args = questionIds.Select(id => (("order[][id]", id.ToString()),
                                                 ("order[][type]", "question")))
                                  .Interleave();

            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/groups/{groupId}/reorder",
                                                   BuildHttpArguments(args));
            response.AssertSuccess();
        }
        
        /// <summary>
        /// Gets assignment overrides for a quiz.
        /// </summary>
        public async Task<QuizAssignmentOverrides> GetQuizAssignmentOverrides(ulong courseId, ulong quizId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/assignment_overrides" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizAssignmentOverridesResponseModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizAssignmentOverrides(this, model.QuizAssignmentOverrides);
        }
        
        /// <summary>
        /// Gets assignment overrides for a quiz, scoped to a specific assignment override id.
        /// </summary>
        public async Task<QuizAssignmentOverrides> GetQuizAssignmentOverrides(ulong courseId, ulong quizId, ulong assignmentOverrideId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/assignment_overrides/{assignmentOverrideId}" + BuildQueryString());
            var model = JsonConvert.DeserializeObject<QuizAssignmentOverridesResponseModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizAssignmentOverrides(this, model.QuizAssignmentOverrides);
        }
        
        /// <summary>
        /// Sets quiz extensions.
        /// </summary>
        public async Task<IEnumerable<QuizExtension>> SetQuizExtensions(ulong courseId, 
                                                                        ulong quizId, 
                                                                        [NotNull] IEnumerable<QuizExtensionInput> extensions) {
            var payload = new JObject {
                ["quiz_extensions"] = new JArray(extensions.Select(e => {
                    var obj = new JObject {
                        ["user_id"] = e.UserId
                    };
                    if (e.ExtraAttempts != null) {
                        obj["extra_attempts"] = e.ExtraAttempts;
                    }
                    if (e.ExtraTime != null) {
                        obj["extra_time"] = e.ExtraTime;
                    }
                    if (e.ManuallyUnlocked != null) {
                        obj["manually_unlocked"] = e.ManuallyUnlocked;
                    }
                    if (e.EndAt != null) {
                        obj["end_at"] = e.EndAt;
                    }
                    if (e.ExtendFromNow != null) {
                        obj["extend_from_now"] = e.ExtendFromNow;
                    }
                    if (e.ExtendFromEndAt != null) {
                        obj["extend_from_end_at"] = e.ExtendFromEndAt;
                    }
                    return obj;
                }))
            };
            
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/extensions",
                                                   BuildHttpJsonBody(payload));
            var models = JsonConvert.DeserializeObject<IEnumerable<QuizExtensionModel>>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return models.Select(m => new QuizExtension(this, m));
        }
        
        /// <summary>
        /// Lists quiz IP filters.
        /// </summary>
        public async Task<IEnumerable<QuizIpFilter>> ListQuizIpFilters(ulong courseId, ulong quizId) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/ip_filters" + BuildQueryString());
            var models = JsonConvert.DeserializeObject<IEnumerable<QuizIpFilterModel>>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return models.Select(m => new QuizIpFilter(this, m));
        }
        
        /// <summary>
        /// Lists quiz reports.
        /// </summary>
        public async Task<IEnumerable<QuizReport>> ListQuizReports(ulong courseId,
                                                                   ulong quizId,
                                                                   bool? includeAllVersions = null,
                                                                   QuizReportInclude? includes = null) {
            var args = new List<(string, string)> {
                ("includes_all_versions", includeAllVersions?.ToShortString())
            };
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(f => ("include[]", f)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/reports" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            var models = await AccumulateDeserializePages<QuizReportModel>(response);
            return models.Select(m => new QuizReport(this, m));
        }
        
        /// <summary>
        /// Creates a quiz report.
        /// </summary>
        public async Task<QuizReport> CreateQuizReport(ulong courseId,
                                                       ulong quizId,
                                                       QuizReportType reportType,
                                                       bool? includeAllVersions = null,
                                                       QuizReportInclude? includes = null) {
            var args = new List<(string, string)> {
                ("quiz_report[report_type]", reportType.GetApiRepresentation()),
                ("quiz_report[includes_all_versions]", includeAllVersions?.ToShortString())
            };
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(f => ("include[]", f)));
            }
            
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/reports",
                                                   BuildHttpArguments(args));
            var model = JsonConvert.DeserializeObject<QuizReportModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizReport(this, model);
        }
        
        /// <summary>
        /// Gets a quiz report.
        /// </summary>
        public async Task<QuizReport> GetQuizReport(ulong courseId, ulong quizId, ulong reportId, QuizReportInclude? includes = null) {
            var args = new List<(string, string)>();
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(f => ("include[]", f)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/reports/{reportId}" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            var model = JsonConvert.DeserializeObject<QuizReportModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizReport(this, model);
        }
        
        /// <summary>
        /// Aborts a quiz report.
        /// </summary>
        public async Task AbortQuizReport(ulong courseId, ulong quizId, ulong reportId) {
            var response = await client.DeleteAsync($"courses/{courseId}/quizzes/{quizId}/reports/{reportId}" + BuildQueryString());
            response.AssertSuccess();
        }
        
        /// <summary>
        /// Gets quiz statistics.
        /// </summary>
        public async Task<QuizStatisticsResponse> GetQuizStatistics(ulong courseId, ulong quizId, bool? allVersions = null) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/statistics" +
                                                  BuildQueryString(("all_versions", allVersions?.ToShortString())));
            var model = JsonConvert.DeserializeObject<QuizStatisticsListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizStatisticsResponse(this, model);
        }
        
        /// <summary>
        /// Lists quiz submission events.
        /// </summary>
        public async Task<IEnumerable<QuizSubmissionEvent>> ListQuizSubmissionEvents(ulong courseId, 
                                                                                     ulong quizId,
                                                                                     ulong? quizSubmissionId = null,
                                                                                     uint? attempt = null) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submission_events" +
                                                  BuildQueryString(("quiz_submission_id", quizSubmissionId?.ToString()),
                                                                   ("attempt", attempt?.ToString())));
            var model = JsonConvert.DeserializeObject<QuizSubmissionEventListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return model.QuizSubmissionEvents.Select(e => new QuizSubmissionEvent(this, e));
        }
        
        /// <summary>
        /// Creates quiz submission events.
        /// </summary>
        public async Task<IEnumerable<QuizSubmissionEvent>> CreateQuizSubmissionEvents(ulong courseId, 
                                                                                       ulong quizId,
                                                                                       ulong quizSubmissionId,
                                                                                       uint? attempt,
                                                                                       [NotNull] IEnumerable<QuizSubmissionEventInput> events) {
            var payload = new JObject {
                ["quiz_submission_id"] = quizSubmissionId
            };
            
            if (attempt != null) {
                payload["quiz_submission_attempt"] = attempt;
            }
            
            payload["quiz_submission_events"] = new JArray(events.Select(e => {
                var obj = new JObject {
                    ["event_type"] = e.EventType
                };
                if (e.EventData != null) {
                    obj["event_data"] = e.EventData;
                }
                if (e.ClientTimestamp != null) {
                    obj["client_timestamp"] = e.ClientTimestamp;
                }
                return obj;
            }));
            
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/submission_events",
                                                   BuildHttpJsonBody(payload));
            var model = JsonConvert.DeserializeObject<QuizSubmissionEventListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return model.QuizSubmissionEvents.Select(e => new QuizSubmissionEvent(this, e));
        }

        /// <summary>
        /// Uploads a file for the current user's quiz submission.
        /// </summary>
        /// <param name="courseId">The course id.</param>
        /// <param name="quizId">The quiz id.</param>
        /// <param name="file">The file bytes.</param>
        /// <param name="filePath">The file path (used for the file name and content type).</param>
        /// <param name="onDuplicate">(Optional) How to handle duplicate names.</param>
        /// <param name="contentType">(Optional) The file MIME type.</param>
        /// <returns>The uploaded file.</returns>
        public Task<CanvasFile> UploadQuizSubmissionFile(ulong courseId,
                                                         ulong quizId,
                                                         [NotNull] byte[] file,
                                                         [NotNull] string filePath,
                                                         string onDuplicate = null,
                                                         string contentType = null) {
            return UploadFile($"courses/{courseId}/quizzes/{quizId}/submissions/self/files",
                              file,
                              Path.GetFileNameWithoutExtension(filePath),
                              Path.GetFileName(filePath),
                              onDuplicate: onDuplicate,
                              contentType: contentType);
        }
        
        /// <summary>
        /// Lists quiz submission questions.
        /// </summary>
        public async Task<IEnumerable<QuizSubmissionQuestion>> ListQuizSubmissionQuestions(ulong courseId,
                                                                                           ulong quizId,
                                                                                           ulong submissionId,
                                                                                           QuizSubmissionQuestionInclude? includes = null) {
            var args = new List<(string, string)>();
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(i => ("include[]", i)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/questions" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            var model = JsonConvert.DeserializeObject<QuizSubmissionQuestionListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return model.QuizSubmissionQuestions.Select(q => new QuizSubmissionQuestion(this, q));
        }
        
        /// <summary>
        /// Answers quiz submission questions.
        /// </summary>
        public async Task<IEnumerable<QuizSubmissionQuestion>> AnswerQuizSubmissionQuestions(ulong courseId,
                                                                                             ulong quizId,
                                                                                             ulong submissionId,
                                                                                             uint attempt,
                                                                                             [NotNull] string validationToken,
                                                                                             [NotNull] IEnumerable<QuizSubmissionQuestionAnswer> answers,
                                                                                             string accessCode = null) {
            var payload = new JObject {
                ["attempt"] = attempt,
                ["validation_token"] = validationToken,
                ["access_code"] = accessCode,
                ["quiz_questions"] = new JArray(answers.Select(a => new JObject {
                    ["id"] = a.QuestionId,
                    ["answer"] = a.Answer.ToJson()
                }))
            };
            
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/questions",
                                                  BuildHttpJsonBody(payload));
            var model = JsonConvert.DeserializeObject<QuizSubmissionQuestionListModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return model.QuizSubmissionQuestions.Select(q => new QuizSubmissionQuestion(this, q));
        }
        
        /// <summary>
        /// Formats a quiz submission answer for display.
        /// </summary>
        public async Task<QuizSubmissionFormattedAnswer> GetQuizSubmissionFormattedAnswer(ulong courseId,
                                                                                          ulong quizId,
                                                                                          ulong submissionId,
                                                                                          ulong questionId,
                                                                                          [NotNull] string answer) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/questions/{questionId}/formatted_answer" +
                                                  BuildQueryString(("answer", answer)));
            var model = JsonConvert.DeserializeObject<QuizSubmissionFormattedAnswerModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmissionFormattedAnswer(this, model);
        }
        
        /// <summary>
        /// Flags a quiz submission question.
        /// </summary>
        public async Task<QuizSubmissionQuestion> FlagQuizSubmissionQuestion(ulong courseId,
                                                                             ulong quizId,
                                                                             ulong submissionId,
                                                                             ulong questionId,
                                                                             uint attempt,
                                                                             [NotNull] string validationToken,
                                                                             string accessCode = null) {
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/questions/{questionId}/flag",
                                                  BuildHttpArguments(new[] {
                                                      ("attempt", attempt.ToString()),
                                                      ("validation_token", validationToken),
                                                      ("access_code", accessCode)
                                                  }));
            var model = JsonConvert.DeserializeObject<QuizSubmissionQuestionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmissionQuestion(this, model);
        }
        
        /// <summary>
        /// Unflags a quiz submission question.
        /// </summary>
        public async Task<QuizSubmissionQuestion> UnflagQuizSubmissionQuestion(ulong courseId,
                                                                               ulong quizId,
                                                                               ulong submissionId,
                                                                               ulong questionId,
                                                                               uint attempt,
                                                                               [NotNull] string validationToken,
                                                                               string accessCode = null) {
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/questions/{questionId}/unflag",
                                                  BuildHttpArguments(new[] {
                                                      ("attempt", attempt.ToString()),
                                                      ("validation_token", validationToken),
                                                      ("access_code", accessCode)
                                                  }));
            var model = JsonConvert.DeserializeObject<QuizSubmissionQuestionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmissionQuestion(this, model);
        }
        
        /// <summary>
        /// Messages users from the quiz submission user list.
        /// </summary>
        public async Task MessageQuizSubmissionUsers(ulong courseId, ulong quizId, [NotNull] QuizSubmissionUserListMessage message) {
            var args = new[] {
                ("recipients", message.Recipients.GetApiRepresentation()),
                ("subject", message.Subject),
                ("body", message.Body)
            };
            
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/submission_users/message",
                                                   BuildHttpArguments(args));
            response.AssertSuccess();
        }

        /// <summary>
        /// Additional data to include when retrieving quiz submissions.
        /// </summary>
        [PublicAPI]
        [System.Flags]
        public enum QuizSubmissionInclude {
            [ApiRepresentation("submission")]
            Submission = 1 << 0,
            [ApiRepresentation("quiz")]
            Quiz = 1 << 1,
            [ApiRepresentation("user")]
            User = 1 << 2
        }
        
        /// <summary>
        /// Lists quiz submissions.
        /// </summary>
        public async Task<IEnumerable<QuizSubmission>> ListQuizSubmissions(ulong courseId,
                                                                           ulong quizId,
                                                                           QuizSubmissionInclude? includes = null) {
            var args = new List<(string, string)>();
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(i => ("include[]", i)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submissions" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            var models = await AccumulateDeserializePages<QuizSubmissionModel>(response);
            return models.Select(m => new QuizSubmission(this, m));
        }
        
        /// <summary>
        /// Streams quiz submissions.
        /// </summary>
        public async IAsyncEnumerable<QuizSubmission> StreamQuizSubmissions(ulong courseId,
                                                                            ulong quizId,
                                                                            QuizSubmissionInclude? includes = null) {
            var args = new List<(string, string)>();
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(i => ("include[]", i)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submissions" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            
            await foreach (var model in StreamDeserializePages<QuizSubmissionModel>(response)) {
                yield return new QuizSubmission(this, model);
            }
        }
        
        /// <summary>
        /// Gets a single quiz submission.
        /// </summary>
        public async Task<QuizSubmission> GetQuizSubmission(ulong courseId,
                                                            ulong quizId,
                                                            ulong submissionId,
                                                            QuizSubmissionInclude? includes = null) {
            var args = new List<(string, string)>();
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(i => ("include[]", i)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            var model = JsonConvert.DeserializeObject<QuizSubmissionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmission(this, model);
        }
        
        /// <summary>
        /// Gets the current user's quiz submission.
        /// </summary>
        public async Task<QuizSubmission> GetQuizSubmissionForCurrentUser(ulong courseId,
                                                                          ulong quizId,
                                                                          QuizSubmissionInclude? includes = null) {
            var args = new List<(string, string)>();
            
            if (includes != null) {
                args.AddRange(includes.GetFlagsApiRepresentations().Select(i => ("include[]", i)));
            }
            
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submission" +
                                                  BuildDuplicateKeyQueryString(args.ToArray()));
            var model = JsonConvert.DeserializeObject<QuizSubmissionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmission(this, model);
        }
        
        /// <summary>
        /// Starts a quiz submission.
        /// </summary>
        public async Task<QuizSubmission> StartQuizSubmission(ulong courseId, ulong quizId, string accessCode = null) {
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/submissions",
                                                   BuildHttpArguments(new[] { ("access_code", accessCode) }));
            var model = JsonConvert.DeserializeObject<QuizSubmissionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmission(this, model);
        }
        
        /// <summary>
        /// Updates quiz submission scores and comments.
        /// </summary>
        public async Task<QuizSubmission> UpdateQuizSubmissionScores(ulong courseId,
                                                                     ulong quizId,
                                                                     ulong submissionId,
                                                                     uint attempt,
                                                                     [NotNull] string validationToken,
                                                                     [NotNull] IEnumerable<QuizSubmissionScoreUpdate> updates,
                                                                     string accessCode = null) {
            var questions = new JObject();
            
            foreach (var update in updates) {
                var q = new JObject();
                if (update.Score != null) {
                    q["score"] = update.Score;
                }
                if (update.Comment != null) {
                    q["comment"] = update.Comment;
                }
                questions[update.QuestionId.ToString()] = q;
            }
            
            var payload = new JObject {
                ["quiz_submissions"] = new JObject {
                    ["attempt"] = attempt,
                    ["validation_token"] = validationToken,
                    ["access_code"] = accessCode,
                    ["questions"] = questions
                }
            };
            
            var response = await client.PutAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}",
                                                  BuildHttpJsonBody(payload));
            var model = JsonConvert.DeserializeObject<QuizSubmissionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmission(this, model);
        }
        
        /// <summary>
        /// Completes a quiz submission.
        /// </summary>
        public async Task<QuizSubmission> CompleteQuizSubmission(ulong courseId,
                                                                 ulong quizId,
                                                                 ulong submissionId,
                                                                 uint attempt,
                                                                 [NotNull] string validationToken,
                                                                 string accessCode = null) {
            var response = await client.PostAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/complete",
                                                   BuildHttpArguments(new[] {
                                                       ("attempt", attempt.ToString()),
                                                       ("validation_token", validationToken),
                                                       ("access_code", accessCode)
                                                   }));
            var model = JsonConvert.DeserializeObject<QuizSubmissionModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmission(this, model);
        }
        
        /// <summary>
        /// Gets a quiz submission's remaining time.
        /// </summary>
        public async Task<QuizSubmissionTime> GetQuizSubmissionTime(ulong courseId,
                                                                    ulong quizId,
                                                                    ulong submissionId,
                                                                    uint attempt,
                                                                    [NotNull] string validationToken,
                                                                    string accessCode = null) {
            var response = await client.GetAsync($"courses/{courseId}/quizzes/{quizId}/submissions/{submissionId}/time" +
                                                  BuildQueryString(("attempt", attempt.ToString()),
                                                                   ("validation_token", validationToken),
                                                                   ("access_code", accessCode)));
            var model = JsonConvert.DeserializeObject<QuizSubmissionTimeModel>(await response.AssertSuccess().Content.ReadAsStringAsync());
            return new QuizSubmissionTime(this, model);
        }
    }
}
