using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppUtils;
using Tomlyn.Model;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Courses;
using UVACanvasAccess.Util;
using static System.DayOfWeek;

namespace RollingAttendanceColumns {
    
    internal static class Program {

        public static async Task Main(string[] args) {
            var home = new AppHome("rolling_attendance_columns");
            
            Console.WriteLine($"Using config path: {home.ConfigPath}");
            
            if (!home.ConfigPresent()) {
                Console.WriteLine("Need to generate a config file.");
                
                home.CreateConfig(new DocumentSyntax {
                    Tables = {
                        new TableSyntax("tokens") {
                            Items = {
                                {"token", "PUT_TOKEN_HERE"}
                            }
                        },
                        new TableSyntax("debug") {
                            Items = {
                                {"limit_to", -1}
                            }
                        },
                        new TableSyntax("filter") {
                            Items = {
                                {"new_column_terms", new string[]{}}
                            }
                        }
                    }
                });

                Console.WriteLine("Created a new config file. Please go put in your token and input path.");
                return;
            }
            
            Console.WriteLine("Found config file.");

            var config = home.GetConfig();
            Debug.Assert(config != null, nameof(config) + " != null");
            
            var token = config.GetTable("tokens")
                              .Get<string>("token");

            var courseLimit = config.GetTable("debug")
                                    .Get<long>("limit_to");

            var filterTerms = config.GetTable("filter")
                                    .Get<TomlArray>("new_column_terms")
                                    .Cast<string>()
                                    .ToHashSet();
            
            var termWhitelist = filterTerms.Count > 0;

            var api = new Api(token, "https://uview.instructure.com/api/v1/");

            if (courseLimit > 0) {
                Console.WriteLine($"[DEBUG] Limited to course id {courseLimit}");
            }

            if (termWhitelist) {
                Console.WriteLine("[FILTER] New column creation is limited to the following sections:");
                foreach (var s in filterTerms) {
                    Console.WriteLine($"         - {s}");
                }
            } else {
                Console.WriteLine("[FILTER] No section filter!");
            }

            try {
                var termLookup = await api.StreamEnrollmentTerms()
                                          .ToLookupAsync(term => term.Name);
                
                IAsyncEnumerable<Course> courses;
                if (courseLimit > 0) {
                    courses = AsyncEnumerable.Repeat(await api.GetCourse(Convert.ToUInt64(courseLimit), includes: Api.IndividualLevelCourseIncludes.Term), 1);
                } else {
                    var terms = filterTerms.Select(termName => termLookup[termName].FirstOrDefault())
                                           .Where(t => t != default);

                    var streams = 
                        terms.Select(term => api.StreamCourses(includes: Api.AccountLevelCourseIncludes.Term, enrollmentTermId: term.Id));
                    
                    courses = Extensions.Merge(streams.ToArray());
                }

                var nextMonday = NextWeekday(DateTime.Today, Monday);
                var nextMondayStr = FormatColumnName(nextMonday);
                
                var lastMonday = NextWeekday(DateTime.Today.AddDays(-7), Monday);
                var lastMondayStr = FormatColumnName(lastMonday);

                Console.WriteLine($"The new column will be called {nextMondayStr}");
                Console.WriteLine($"The old column (if it exists) is called {lastMondayStr}");
                
                await foreach (var course in courses) {
                    try {
                        var old = await api.StreamCustomGradebookColumns(course.Id)
                                           .FirstOrDefaultAsync(col => col.Title == lastMondayStr);

                        if (old != null) {
                            await api.UpdateCustomColumn(old.Id, course.Id, hidden: true);
                            Console.WriteLine($"[Course {course.Id}] Hid old column id {old.Id}");
                        }

                        if (termWhitelist) {
                            var t = course.Term?.Name;
                            if (t == null || !filterTerms.Contains(t)) {
                                Console.WriteLine($"[Course {course.Id}] Skipping new column creation (term {t ?? "default"} not in whitelist)");
                                continue;
                            }
                        }

                        var preexisting = await api.StreamCustomGradebookColumns(course.Id)
                                                   .FirstOrDefaultAsync(col => col.Title == nextMondayStr);

                        if (preexisting != null) {
                            Console.WriteLine($"[Course {course.Id}] Column id {preexisting.Id} already exists, skipping");
                        } else {
                            var c = await api.CreateCustomColumn(course.Id, nextMondayStr);
                            Console.WriteLine($"[Course {course.Id}] Created new column id {c.Id}");
                        }
                    } catch (Exception e) {
                        Console.WriteLine($"Threw up during course {course.Id}:\n{e}\nContinuing onwards.");
                    }
                }

                Console.WriteLine("Done.");
            } catch (Exception e) {
                Console.WriteLine($"Threw up:\n{e}");
            }
        }
        
        private static DateTime NextWeekday(DateTime from, DayOfWeek day) => from.AddDays(((int) day - (int) from.DayOfWeek + 7) % 7);

        private static string FormatColumnName(DateTime date) {
            var m = date.Month.ToString().PadLeft(2, '0');
            var d = date.Day.ToString().PadLeft(2, '0');
            return $"Attendance_{m}_{d}_{date.Year}";
        }
    }
}
