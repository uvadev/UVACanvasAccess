namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AvatarModel {
        public string type { get; set; }
        public string url { get; set; }
        public string token { get; set; }
        public string display_name { get; set; }
        public ulong id { get; set; }
        public string content_type { get; set; }
        public string filename { get; set; }
        public ulong size { get; set; }

        public override string ToString() {
            return $"{nameof(type)}: {type}," +
                   $"\n{nameof(url)}: {url}," +
                   $"\n{nameof(token)}: {token}," +
                   $"\n{nameof(display_name)}: {display_name}," +
                   $"\n{nameof(id)}: {id}," +
                   $"\n{nameof(content_type)}: {content_type}," +
                   $"\n{nameof(filename)}: {filename}," +
                   $"\n{nameof(size)}: {size}";
        }
    }
}