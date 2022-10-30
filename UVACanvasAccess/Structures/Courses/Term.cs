using System;
using JetBrains.Annotations;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Courses;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Courses {
    
    /// <summary>
    /// Represents an enrollment term.
    /// </summary>
    [PublicAPI]
    public class Term : IPrettyPrint {
        private readonly Api api;
        
        /// <summary>
        /// The term id.
        /// </summary>
        public ulong Id { get; }
        
        /// <summary>
        /// The term name.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// When the term begins.
        /// </summary>
        public DateTime? StartAt { get; }
        
        /// <summary>
        /// When the term ends.
        /// </summary>
        public DateTime? EndAt { get; }

        internal Term(Api api, TermModel model) {
            this.api = api;
            Id = model.Id;
            Name = model.Name;
            StartAt = model.StartAt;
            EndAt = model.EndAt;
        }

        /// <inheritdoc />
        public string ToPrettyString() {
            return "Term {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(StartAt)}: {StartAt}," +
                   $"\n{nameof(EndAt)}: {EndAt}").Indent(4) + 
                   "\n}";
        }
    }
}