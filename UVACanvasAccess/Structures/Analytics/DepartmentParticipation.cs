using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UVACanvasAccess.Model.Analytics;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Analytics {
    
    [PublicAPI]
    public readonly struct DepartmentParticipation : IPrettyPrint {
        public Dictionary<DateTime, DateEntry> ByDate { get; }
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

        public string ToPrettyString() {
            return "DepartmentParticipation {" +
                   ($"\n{nameof(ByDate)}: {ByDate.ToPrettyString()}," +
                    $"\n{nameof(ByCategory)}: {ByCategory.ToPrettyString()}").Indent(4) +
                   "\n}";
        }

        [PublicAPI]
        public class DateEntry : IPrettyPrint {
            public ulong Views { get; }
            
            public ulong Participations { get; }

            internal DateEntry(ulong views, ulong participations) {
                Views = views;
                Participations = participations;
            }

            public string ToPrettyString() {
                return "DateEntry {" +
                       ($"\n{nameof(Views)}: {Views}," +
                       $"\n{nameof(Participations)}: {Participations}").Indent(4) + 
                       "\n}";
            }
        }

        [PublicAPI]
        public class Categories : IPrettyPrint {
            public ulong? Announcements { get; }
            public ulong? Assignments { get; }
            public ulong? Collaborations { get; }
            public ulong? Conferences { get; }
            public ulong? Discussions { get; }
            public ulong? Files { get; }
            public ulong? General { get; }
            public ulong? Grades { get; }
            public ulong? Groups { get; }
            public ulong? Modules { get; }
            public ulong? Other { get; }
            public ulong? Pages { get; }
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
