using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a polymorphic answer value for a quiz submission question.
    /// </summary>
    [PublicAPI]
    public abstract class QuizSubmissionQuestionAnswerValue : IPrettyPrint {
        
        internal abstract JToken ToJson();
        
        internal static QuizSubmissionQuestionAnswerValue Create([CanBeNull] JToken answer, QuizQuestionType? questionType) {
            if (answer == null || answer.Type == JTokenType.Null) {
                return null;
            }
            
            if (questionType == null) {
                return new QuizSubmissionUnknownAnswerValue(answer.ToString(Newtonsoft.Json.Formatting.None));
            }
            
            switch (questionType) {
                case QuizQuestionType.EssayQuestion:
                case QuizQuestionType.ShortAnswerQuestion:
                    return new QuizSubmissionTextAnswerValue(answer.Value<string>());
                case QuizQuestionType.CalculatedQuestion:
                case QuizQuestionType.NumericalQuestion:
                    return CreateNumericAnswer(answer);
                case QuizQuestionType.MultipleChoiceQuestion:
                case QuizQuestionType.TrueFalseQuestion:
                    return new QuizSubmissionSingleAnswerIdValue(ParseUlong(answer));
                case QuizQuestionType.MultipleAnswersQuestion:
                    return CreateMultipleAnswersAnswer(answer);
                case QuizQuestionType.MatchingQuestion:
                    return CreateMatchingAnswer(answer);
                case QuizQuestionType.FillInMultipleBlanksQuestion:
                    return CreateFillInMultipleBlanksAnswer(answer);
                case QuizQuestionType.MultipleDropdownsQuestion:
                    return CreateMultipleDropdownsAnswer(answer);
                default:
                    return new QuizSubmissionUnknownAnswerValue(answer.ToString(Newtonsoft.Json.Formatting.None));
            }
        }
        
        private static QuizSubmissionQuestionAnswerValue CreateNumericAnswer(JToken answer) {
            if (answer.Type == JTokenType.Integer || answer.Type == JTokenType.Float) {
                return new QuizSubmissionNumericAnswerValue(answer.Value<decimal?>());
            }
            
            var text = answer.ToString();
            if (decimal.TryParse(text, out var value)) {
                return new QuizSubmissionNumericAnswerValue(value);
            }
            
            return new QuizSubmissionNumericAnswerValue(null, text);
        }
        
        private static QuizSubmissionQuestionAnswerValue CreateMultipleAnswersAnswer(JToken answer) {
            if (answer.Type != JTokenType.Array) {
                return new QuizSubmissionUnknownAnswerValue(answer.ToString(Newtonsoft.Json.Formatting.None));
            }
            
            var ids = answer.Select(ParseUlong).Where(v => v != null).Select(v => v.Value);
            return new QuizSubmissionMultipleAnswersAnswerValue(ids);
        }
        
        private static QuizSubmissionQuestionAnswerValue CreateMatchingAnswer(JToken answer) {
            if (answer.Type != JTokenType.Array) {
                return new QuizSubmissionUnknownAnswerValue(answer.ToString(Newtonsoft.Json.Formatting.None));
            }
            
            var matches = answer
                .OfType<JObject>()
                .Select(o => new QuizSubmissionMatchingAnswerPair(
                    ParseUlong(o["answer_id"]),
                    ParseUlong(o["match_id"])
                ));
            
            return new QuizSubmissionMatchingAnswerValue(matches);
        }
        
        private static QuizSubmissionQuestionAnswerValue CreateFillInMultipleBlanksAnswer(JToken answer) {
            if (answer.Type != JTokenType.Object) {
                return new QuizSubmissionUnknownAnswerValue(answer.ToString(Newtonsoft.Json.Formatting.None));
            }
            
            var obj = (JObject) answer;
            var dict = obj.Properties().ToDictionary(p => p.Name, p => p.Value.ToString());
            return new QuizSubmissionFillInMultipleBlanksAnswerValue(dict);
        }
        
        private static QuizSubmissionQuestionAnswerValue CreateMultipleDropdownsAnswer(JToken answer) {
            if (answer.Type != JTokenType.Object) {
                return new QuizSubmissionUnknownAnswerValue(answer.ToString(Newtonsoft.Json.Formatting.None));
            }
            
            var obj = (JObject) answer;
            var dict = obj.Properties()
                          .Select(p => (p.Name, Value: ParseUlong(p.Value)))
                          .Where(p => p.Value != null)
                          .ToDictionary(p => p.Name, p => p.Value ?? 0);
            return new QuizSubmissionMultipleDropdownsAnswerValue(dict);
        }
        
        protected static ulong? ParseUlong(JToken token) {
            if (token == null || token.Type == JTokenType.Null) {
                return null;
            }
            
            if (token.Type == JTokenType.Integer) {
                return token.Value<ulong>();
            }
            
            return ulong.TryParse(token.ToString(), out var value) ? value
                                                                   : (ulong?) null;
        }
        
        public abstract string ToPrettyString();
    }
    
    /// <summary>
    /// Represents an unknown answer value.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionUnknownAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        [CanBeNull]
        public string RawJson { get; }
        
        public QuizSubmissionUnknownAnswerValue([CanBeNull] string rawJson) {
            RawJson = rawJson;
        }
        
        internal override JToken ToJson() {
            return RawJson == null ? JValue.CreateNull()
                                   : JToken.Parse(RawJson);
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionUnknownAnswerValue {" +
                   ($"\n{nameof(RawJson)}: {RawJson}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents a text answer value.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionTextAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        [CanBeNull]
        public string Text { get; }
        
        public QuizSubmissionTextAnswerValue([CanBeNull] string text) {
            Text = text;
        }
        
        internal override JToken ToJson() {
            return Text == null ? JValue.CreateNull() 
                                : new JValue(Text);
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionTextAnswerValue {" +
                   ($"\n{nameof(Text)}: {Text}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents a numeric answer value.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionNumericAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        public decimal? Value { get; }
        
        [CanBeNull]
        public string TextValue { get; }
        
        public QuizSubmissionNumericAnswerValue(decimal? value, string textValue = null) {
            Value = value;
            TextValue = textValue;
        }
        
        internal override JToken ToJson() {
            if (Value != null) {
                return new JValue(Value.Value);
            }
            
            return TextValue == null ? JValue.CreateNull()
                                     : new JValue(TextValue);
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionNumericAnswerValue {" +
                   ($"\n{nameof(Value)}: {Value}," +
                    $"\n{nameof(TextValue)}: {TextValue}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents a single answer id.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionSingleAnswerIdValue : QuizSubmissionQuestionAnswerValue {
        
        public ulong? AnswerId { get; }
        
        public QuizSubmissionSingleAnswerIdValue(ulong? answerId) {
            AnswerId = answerId;
        }
        
        internal override JToken ToJson() {
            return AnswerId == null ? JValue.CreateNull()
                                    : new JValue(AnswerId.Value);
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionSingleAnswerIdValue {" +
                   ($"\n{nameof(AnswerId)}: {AnswerId}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents multiple answer ids.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionMultipleAnswersAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        [CanBeNull]
        public IEnumerable<ulong> AnswerIds { get; }
        
        public QuizSubmissionMultipleAnswersAnswerValue([CanBeNull] IEnumerable<ulong> answerIds) {
            AnswerIds = answerIds;
        }
        
        internal override JToken ToJson() {
            return AnswerIds == null ? (JToken) JValue.CreateNull()
                                     : new JArray(AnswerIds.Select(v => new JValue(v)));
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionMultipleAnswersAnswerValue {" +
                   ($"\n{nameof(AnswerIds)}: {AnswerIds?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents a matching answer pair.
    /// </summary>
    [PublicAPI]
    public readonly struct QuizSubmissionMatchingAnswerPair : IPrettyPrint {
        
        public ulong? AnswerId { get; }
        
        public ulong? MatchId { get; }
        
        public QuizSubmissionMatchingAnswerPair(ulong? answerId, ulong? matchId) {
            AnswerId = answerId;
            MatchId = matchId;
        }
        
        public string ToPrettyString() {
            return "QuizSubmissionMatchingAnswerPair {" +
                   ($"\n{nameof(AnswerId)}: {AnswerId}," +
                    $"\n{nameof(MatchId)}: {MatchId}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents matching question answers.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionMatchingAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        [CanBeNull]
        public IEnumerable<QuizSubmissionMatchingAnswerPair> Matches { get; }
        
        public QuizSubmissionMatchingAnswerValue([CanBeNull] IEnumerable<QuizSubmissionMatchingAnswerPair> matches) {
            Matches = matches;
        }
        
        internal override JToken ToJson() {
            if (Matches == null) {
                return JValue.CreateNull();
            }
            
            return new JArray(Matches.Select(m => new JObject {
                ["answer_id"] = m.AnswerId,
                ["match_id"] = m.MatchId
            }));
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionMatchingAnswerValue {" +
                   ($"\n{nameof(Matches)}: {Matches?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents fill-in-multiple-blanks answers.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionFillInMultipleBlanksAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        [CanBeNull]
        public IReadOnlyDictionary<string, string> Answers { get; }
        
        public QuizSubmissionFillInMultipleBlanksAnswerValue([CanBeNull] IReadOnlyDictionary<string, string> answers) {
            Answers = answers;
        }
        
        internal override JToken ToJson() {
            if (Answers == null) {
                return JValue.CreateNull();
            }
            
            var obj = new JObject();
            foreach (var kvp in Answers) {
                obj[kvp.Key] = kvp.Value;
            }
            return obj;
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionFillInMultipleBlanksAnswerValue {" +
                   ($"\n{nameof(Answers)}: {Answers?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
    
    /// <summary>
    /// Represents multiple dropdown answers.
    /// </summary>
    [PublicAPI]
    public class QuizSubmissionMultipleDropdownsAnswerValue : QuizSubmissionQuestionAnswerValue {
        
        [CanBeNull]
        public IReadOnlyDictionary<string, ulong> AnswerIds { get; }
        
        public QuizSubmissionMultipleDropdownsAnswerValue([CanBeNull] IReadOnlyDictionary<string, ulong> answerIds) {
            AnswerIds = answerIds;
        }
        
        internal override JToken ToJson() {
            if (AnswerIds == null) {
                return JValue.CreateNull();
            }
            
            var obj = new JObject();
            foreach (var kvp in AnswerIds) {
                obj[kvp.Key] = kvp.Value;
            }
            return obj;
        }
        
        public override string ToPrettyString() {
            return "QuizSubmissionMultipleDropdownsAnswerValue {" +
                   ($"\n{nameof(AnswerIds)}: {AnswerIds?.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
}
