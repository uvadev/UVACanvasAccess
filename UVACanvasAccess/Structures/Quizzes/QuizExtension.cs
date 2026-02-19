using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Quizzes;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Quizzes {
    
    /// <summary>
    /// Represents a quiz extension for a user.
    /// </summary>
    [PublicAPI]
    public class QuizExtension : IPrettyPrint {
        private readonly Api api;
        
        public ulong QuizId { get; }
        
        public ulong UserId { get; }
        
        public int? ExtraAttempts { get; }
        
        public int? ExtraTime { get; }
        
        public bool? ManuallyUnlocked { get; }
        
        public DateTime? EndAt { get; }

        internal QuizExtension(Api api, QuizExtensionModel model) {
            this.api = api;
            QuizId = model.QuizId;
            UserId = model.UserId;
            ExtraAttempts = model.ExtraAttempts;
            ExtraTime = model.ExtraTime;
            ManuallyUnlocked = model.ManuallyUnlocked;
            EndAt = model.EndAt;
        }

        public string ToPrettyString() {
            return "QuizExtension {" +
                   ($"\n{nameof(QuizId)}: {QuizId}," +
                    $"\n{nameof(UserId)}: {UserId}," +
                    $"\n{nameof(ExtraAttempts)}: {ExtraAttempts}," +
                    $"\n{nameof(ExtraTime)}: {ExtraTime}," +
                    $"\n{nameof(ManuallyUnlocked)}: {ManuallyUnlocked}," +
                    $"\n{nameof(EndAt)}: {EndAt}").Indent(4) +
                   "\n}";
        }
    }

    /// <summary>
    /// A quiz extension definition for setting extensions.
    /// </summary>
    [PublicAPI]
    public class QuizExtensionInput {
        
        public ulong UserId { get; }
        
        public int? ExtraAttempts { get; }
        
        public int? ExtraTime { get; }
        
        public bool? ManuallyUnlocked { get; }
        
        public DateTime? EndAt { get; }
        
        public bool? ExtendFromNow { get; }
        
        public bool? ExtendFromEndAt { get; }

        public QuizExtensionInput(ulong userId,
                                  int? extraAttempts = null,
                                  int? extraTime = null,
                                  bool? manuallyUnlocked = null,
                                  DateTime? endAt = null,
                                  bool? extendFromNow = null,
                                  bool? extendFromEndAt = null) {
            UserId = userId;
            ExtraAttempts = extraAttempts;
            ExtraTime = extraTime;
            ManuallyUnlocked = manuallyUnlocked;
            EndAt = endAt;
            ExtendFromNow = extendFromNow;
            ExtendFromEndAt = extendFromEndAt;
        }
    }
}
