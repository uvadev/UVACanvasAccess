using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppUtils;
using Tomlyn.Model;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Structures.Sections;
using static UVACanvasAccess.ApiParts.Api.AssignmentInclusions;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;

namespace DiscoverUngradedAssignments {
    internal static class Program {
        public static async Task Main(string[] args) {
            var home = new AppHome("discover_ungraded_assignments");
            
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
                        new TableSyntax("input") {
                            Items = {
                                {"user", -1},
                                {"ignore", new int[]{}}
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
            
            var input = config.GetTable("input");

            var userId = Convert.ToUInt64(input.Get<long>("user"));
            
            var ignoreIds = input.MaybeGet<TomlArray>("ignore")
                                ?.Select(Convert.ToUInt64)
                                 .ToHashSet() ?? new HashSet<ulong>();

            var sectionFilters = input.MaybeGet<TomlTableArray>("section_filters")
                                     ?.ToLookup(table => Convert.ToUInt64(table.Get<long>("course")), 
                                                table => table.Get<string>("section_name"));
            
            var api = new Api(token, "https://uview.instructure.com/api/v1/");

            try {
                var enrollments = api.StreamUserEnrollments(userId, new[] {TeacherEnrollment});
                
                await foreach (var taughtCourse in enrollments) {
                    if (ignoreIds.Contains(taughtCourse.CourseId)) {
                        continue;
                    }
                    
                    IEnumerable<Section> sections = null;
                    if (sectionFilters != null && sectionFilters.Contains(taughtCourse.CourseId)) {
                        sections = await api.StreamCourseSections(taughtCourse.CourseId)
                                            .Where(s => sectionFilters[taughtCourse.CourseId].Any(n => n == s.Name))
                                            .ToListAsync();
                    }
                    
                    await foreach (var assignment in api.StreamCourseAssignments(taughtCourse.CourseId,
                                                                                 Submission | AllDates)) {
                        await foreach (var submission in api.StreamSubmissionVersions(taughtCourse.CourseId,
                                                                                      assignment.Id)) {
                            if (sections != null) {
                                // need to make sure we want this student
                                var ok = await api.StreamUserEnrollments(submission.UserId)
                                                  .Where(e => e.CourseId == taughtCourse.CourseId)
                                                  .AnyAsync(e => e.CourseSectionId != null && sections.Any(s => s.Id == e.CourseSectionId));
                                if (!ok) {
                                    continue;
                                }
                            }

                            if (submission.Score == null) {
                                Console.WriteLine($"Ungraded Assignment: {assignment.Name} ({assignment.Id}) submitted by user {submission.UserId} " +
                                                  $"in course {taughtCourse.CourseId}.");
                            }
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Exception:\n{e}");
            }
        }
    }
}
