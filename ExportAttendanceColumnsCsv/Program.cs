using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppUtils;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Util;
using static UVACanvasAccess.ApiParts.Api.CourseEnrollmentType;

namespace ExportAttendanceColumnsCsv {
    
    internal static class Program {

        public static async Task Main(string[] args) {
            var home = new AppHome("export_attendance_columns_csv");
            
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
                        new TableSyntax("options") {
                            Items = {
                                {"include_hidden", false}
                            }
                        },
                        new TableSyntax("debug") {
                            Items = {
                                {"limit_to", -1}
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

            var includeHidden = config.GetTable("options")
                                      .Get<bool>("include_hidden");
            
            var courseLimit = config.GetTable("debug")
                                    .Get<long>("limit_to");
            
            var api = new Api(token, "https://uview.instructure.com/api/v1/");

            if (courseLimit > 0) {
                Console.WriteLine($"[DEBUG] Limited to course id {courseLimit}");
            }

            var table = new StringBuilder("course_id,column_name,student_id,enrollment_id,column_value\n");

            try {
                var courses = courseLimit <= 0 ? api.StreamCourses()
                                               : AsyncEnumerable.Repeat(await api.GetCourse(Convert.ToUInt64(courseLimit)), 1);

                await foreach (var course in courses) {
                    try {
                        var enrollments = api.StreamCourseEnrollments(course.Id, StudentEnrollment.Yield())
                                             .ToDictionaryAsync(e => e.UserId);
                        await foreach (var col in api.StreamCustomGradebookColumns(course.Id, includeHidden)) {
                            await foreach (var datum in api.StreamColumnEntries(col.Id, course.Id)) {
                                (await enrollments).TryGetValue(datum.UserId, out var thisEnrollment);
                                table.Append($"{course.Id}," +
                                             $"\"{col.Title}\"," +
                                             $"{datum.UserId}," +
                                             $"{thisEnrollment?.Id.ToString() ?? "NULL"}" +
                                             $",\"{datum.Content}\"\n");
                            }
                        }
                    } catch (Exception e) {
                        Console.WriteLine($"Threw up during course {course.Id}:\n{e}\nContinuing onwards.");
                    }
                }

                var now = DateTime.Now;
                var outPath = Path.Combine(home.NsDir, $"AttendanceColumns_{now.Year}_{now.Month}_{now.Day}.csv");
                File.WriteAllText(outPath, table.ToString());
                Console.WriteLine($"Wrote report to {outPath}");
            } catch (Exception e) {
                Console.WriteLine($"Threw up:\n{e}");
            }
        }
    }
}
