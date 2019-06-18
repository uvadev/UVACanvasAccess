using System.Collections.Generic;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UserModel {
        public ulong id { get; set; }
        public string name { get; set; }
        public string sortable_name { get; set; }
        public string short_name { get; set; }
        public string sis_user_id { get; set; }
        public ulong? sis_import_id { get; set; }
        public string integration_id { get; set; }
        public string login_id { get; set; }
        public string avatar_url { get; set; }
        public List<object> enrollments { get; set; } // todo Enrollment model
        public string email { get; set; }
        public string locale { get; set; }
        public string last_login { get; set; }
        public string time_zone { get; set; }
        public string bio { get; set; }

        public override string ToString() {
            return $"{nameof(id)}: {id}, " +
                   $"\n{nameof(name)}: {name}," +
                   $"\n{nameof(sortable_name)}: {sortable_name}," +
                   $"\n{nameof(short_name)}: {short_name}," +
                   $"\n{nameof(sis_user_id)}: {sis_user_id}," +
                   $"\n{nameof(sis_import_id)}: {sis_import_id}," +
                   $"\n{nameof(integration_id)}: {integration_id}," +
                   $"\n{nameof(login_id)}: {login_id}," +
                   $"\n{nameof(avatar_url)}: {avatar_url}," +
                   $"\n{nameof(enrollments)}: {enrollments}," +
                   $"\n{nameof(email)}: {email}," +
                   $"\n{nameof(locale)}: {locale}," +
                   $"\n{nameof(last_login)}: {last_login}," +
                   $"\n{nameof(time_zone)}: {time_zone}," +
                   $"\n{nameof(bio)}: {bio}";
        }
    }
}