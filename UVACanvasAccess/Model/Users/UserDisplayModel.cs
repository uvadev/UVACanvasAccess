namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UserDisplayModel {
        public ulong id { get; set; }
        public string short_name { get; set; }
        public string avatar_image_url { get; set; }
        public string html_url { get; set; }

        public override string ToString() {
            return $"{nameof(id)}: {id}," +
                   $"\n{nameof(short_name)}: {short_name}," +
                   $"\n{nameof(avatar_image_url)}: {avatar_image_url}," +
                   $"\n{nameof(html_url)}: {html_url}";
        }
    }
}