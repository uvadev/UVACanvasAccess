using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    /// <summary>
    /// Represents department-level participation data/page views.
    /// </summary>
    [PublicAPI]
    public readonly struct DepartmentParticipation : IPrettyPrint {
        
        /// <summary>
        /// Page views grouped by date.
        /// </summary>
        public Dictionary<DateTime, DateEntry> ByDate { get; }
        
        /// <summary>
        /// Page views grouped by category.
        /// </summary>
        public Categories ByCategory { get; }

        internal DepartmentParticipation(DepartmentParticipationModel dpm) {
            ByCategory = new Categories(dpm.ByCategory);
            ByDate = dpm.ByDate
                        .GroupBy(m => m.Date)
                        .OrderBy(g => g.Key)
                        .ToDictionary(g => g.Key, g => g.ToList())
                        .ValSelect(v => v.First())
                        .ValSelect(v => new DateEntry(v.Views, v.Participations));
        }

        /// <inheritdoc/>
        public string ToPrettyString() {
            return "DepartmentParticipation {" +
                   ($"\n{nameof(ByDate)}: {ByDate.ToPrettyString()}," +
                    $"\n{nameof(ByCategory)}: {ByCategory.ToPrettyString()}").Indent(4) +
                   "\n}";
        }

        /// <summary>
        /// A date-grouped entry in a <see cref="DepartmentParticipation"/>.
        /// </summary>
        [PublicAPI]
        public class DateEntry : IPrettyPrint {
            
            /// <summary>
            /// The amount of views on this date.
            /// </summary>
            public ulong Views { get; }
            
            /// <summary>
            /// The amount of participations on this date.
            /// </summary>
            public ulong Participations { get; }

            internal DateEntry(ulong views, ulong participations) {
                Views = views;
                Participations = participations;
            }

            /// <inheritdoc/>
            public string ToPrettyString() {
                return "DateEntry {" +
                       ($"\n{nameof(Views)}: {Views}," +
                       $"\n{nameof(Participations)}: {Participations}").Indent(4) + 
                       "\n}";
            }
        }

        /// <summary>
        /// A category-grouped entry in a <see cref="DepartmentParticipation"/>.
        /// </summary>
        [PublicAPI]
        public class Categories : IPrettyPrint {
            
            /// <summary>
            /// The amount of views in the announcements category.
            /// </summary>
            public ulong? Announcements { get; }
            
            /// <summary>
            /// The amount of views in the assignments category.
            /// </summary>
            public ulong? Assignments { get; }
            
            /// <summary>
            /// The amount of views in the collaborations category.
            /// </summary>
            public ulong? Collaborations { get; }
            
            /// <summary>
            /// The amount of views in the conferences category.
            /// </summary>
            public ulong? Conferences { get; }
            
            /// <summary>
            /// The amount of views in the discussions category.
            /// </summary>
            public ulong? Discussions { get; }
            
            /// <summary>
            /// The amount of views in the files category.
            /// </summary>
            public ulong? Files { get; }
            
            /// <summary>
            /// The amount of views in the general category.
            /// </summary>
            public ulong? General { get; }
            
            /// <summary>
            /// The amount of views in the grades category.
            /// </summary>
            public ulong? Grades { get; }
            
            /// <summary>
            /// The amount of views in the groups category.
            /// </summary>
            public ulong? Groups { get; }
            
            /// <summary>
            /// The amount of views in the modules category.
            /// </summary>
            public ulong? Modules { get; }
            
            /// <summary>
            /// The amount of views in the other category.
            /// </summary>
            public ulong? Other { get; }
            
            /// <summary>
            /// The amount of views in the pages category.
            /// </summary>
            public ulong? Pages { get; }
            
            /// <summary>
            /// The amount of views in the quizzes category.
            /// </summary>
            public ulong? Quizzes { get; }

            internal Categories(IEnumerable<DepartmentParticipationCategoryEntryModel> entries) {

                Dictionary<string, ulong> dict = entries.GroupBy(e => e.Category)
                                                        .ToDictionary(g => g.Key, g => g.ToList())
                                                        .ValSelect(l => l.First().Views);
                
                if (dict.TryGetValue("announcements", out var n)) {
                    Announcements = n;
                }

                if (dict.TryGetValue("assignments", out n)) {
                    Assignments = n;
                }

                if (dict.TryGetValue("collaborations", out n)) {
                    Collaborations = n;
                }

                if (dict.TryGetValue("conferences", out n)) {
                    Conferences = n;
                }

                if (dict.TryGetValue("discussions", out n)) {
                    Discussions = n;
                }

                if (dict.TryGetValue("files", out n)) {
                    Files = n;
                }

                if (dict.TryGetValue("general", out n)) {
                    General = n;
                }

                if (dict.TryGetValue("grades", out n)) {
                    Grades = n;
                }

                if (dict.TryGetValue("groups", out n)) {
                    Groups = n;
                }

                if (dict.TryGetValue("modules", out n)) {
                    Modules = n;
                }

                if (dict.TryGetValue("other", out n)) {
                    Other = n;
                }

                if (dict.TryGetValue("pages", out n)) {
                    Pages = n;
                }

                if (dict.TryGetValue("quizzes", out n)) {
                    Quizzes = n;
                }
            }

            /// <inheritdoc/>
            public string ToPrettyString() {
                return "ByCategory {" +
                       ($"\n{nameof(Announcements)}: {Announcements}," +
                        $"\n{nameof(Assignments)}: {Assignments}," +
                        $"\n{nameof(Collaborations)}: {Collaborations}," +
                        $"\n{nameof(Conferences)}: {Conferences}," +
                        $"\n{nameof(Discussions)}: {Discussions}," +
                        $"\n{nameof(Files)}: {Files}," +
                        $"\n{nameof(General)}: {General}," +
                        $"\n{nameof(Grades)}: {Grades}," +
                        $"\n{nameof(Groups)}: {Groups}," +
                        $"\n{nameof(Modules)}: {Modules}," +
                        $"\n{nameof(Other)}: {Other}," +
                        $"\n{nameof(Pages)}: {Pages}," +
                        $"\n{nameof(Quizzes)}: {Quizzes}").Indent(4) + 
                        "\n}";
            }
        }
    }
}
