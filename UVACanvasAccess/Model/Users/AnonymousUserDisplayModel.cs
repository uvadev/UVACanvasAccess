namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AnonymousUserDisplayModel {
        public string anonymous_id { get; set; }
        public string avatar_image_url { get; set; }

        public override string ToString() {
            return $"{nameof(anonymous_id)}: {anonymous_id}," +
                   $"\n{nameof(avatar_image_url)}: {avatar_image_url}";
        }
    }
}