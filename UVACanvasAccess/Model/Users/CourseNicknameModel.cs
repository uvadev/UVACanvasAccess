namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CourseNicknameModel {
        public ulong course_id { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }

        public override string ToString() {
            return $"{nameof(course_id)}: {course_id}," +
                   $"\n{nameof(name)}: {name}," +
                   $"\n{nameof(nickname)}: {nickname}";
        }
    }
}