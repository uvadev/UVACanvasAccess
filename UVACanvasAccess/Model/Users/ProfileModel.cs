namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProfileModel {
        public ulong id { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string sortable_name { get; set; }
        public string title { get; set; }
        public string bio { get; set; }
        public string primary_email { get; set; }
        public string login_id { get; set; }
        public string sis_user_id { get; set; }
        public string lti_user_id { get; set; }
        public string avatar_url { get; set; }
        public object calendar { get; set; }
        public string time_zone { get; set; }
        public string locale { get; set; }

        public override string ToString() {
            return $"{nameof(id)}: {id}," +
                   $"\n{nameof(name)}: {name}," +
                   $"\n{nameof(short_name)}: {short_name}," +
                   $"\n{nameof(sortable_name)}: {sortable_name}," +
                   $"\n{nameof(title)}: {title}," +
                   $"\n{nameof(bio)}: {bio}," +
                   $"\n{nameof(primary_email)}: {primary_email}," +
                   $"\n{nameof(login_id)}: {login_id}," +
                   $"\n{nameof(sis_user_id)}: {sis_user_id}," +
                   $"\n{nameof(lti_user_id)}: {lti_user_id}," +
                   $"\n{nameof(avatar_url)}: {avatar_url}," +
                   $"\n{nameof(calendar)}: {calendar}," +
                   $"\n{nameof(time_zone)}: {time_zone}," +
                   $"\n{nameof(locale)}: {locale}";
        }
    }
}