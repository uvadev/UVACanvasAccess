using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AppUtils;
using Tomlyn.Syntax;
using UVACanvasAccess.ApiParts;

namespace ManageAttendanceColumns {
    
    internal static class Program {

        public static async Task Main(string[] args) {
            var home = new AppHome("manage_attendance_columns_csv");
            
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
            
            var api = new Api(token, "https://uview.instructure.com/api/v1/");

            if (courseLimit > 0) {
                Console.WriteLine($"[DEBUG] Limited to course id {courseLimit}");
            }
            
            try {
                var courses = courseLimit <= 0 ? api.StreamCourses()
                                               : AsyncEnumerable.Repeat(await api.GetCourse(Convert.ToUInt64(courseLimit)), 1);
                
                await foreach (var course in courses) {
                    try {
                        await foreach (var col in api.StreamCustomGradebookColumns(course.Id, true)
                                                     .Where(col => col.Title.Contains("Attendance_"))) {
                            var unhidden = await api.UpdateCustomColumn(col.Id, course.Id, hidden: false);
                            Console.WriteLine($"Course {course.Id} - Unhid {unhidden.Title}");
                        }
                    } catch (Exception e) {
                        Console.WriteLine($"Threw up during course {course.Id}:\n{e}\nContinuing onwards.");
                    }
                }
            } catch (Exception e) {
                Console.WriteLine($"Threw up:\n{e}");
            }
        }
    }
}
