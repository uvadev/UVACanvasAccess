using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UVACanvasAccess.Model.Users {
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class UserModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("sortable_name")]
        public string SortableName { get; set; }
        
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        
        [JsonProperty("sis_user_id")]
        public string SisUserId { get; set; }
        
        [JsonProperty("sis_import_id")]
        public ulong? SisImportId { get; set; }
        
        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }
        
        [JsonProperty("login_id")]
        public string LoginId { get; set; }
        
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonProperty("enrollments")]
        public List<object> Enrollments { get; set; } // todo Enrollment model
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("locale")]
        public string Locale { get; set; }
        
        [JsonProperty("effective_locale")]
        public string EffectiveLocale { get; set; }
        
        [JsonProperty("last_login")]
        public DateTime? LastLogin { get; set; }
        
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }
        
        [JsonProperty("bio")]
        public string Bio { get; set; }
        
        [JsonProperty("permissions")]
        public Dictionary<string, bool> Permissions { get; set; }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}, " +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(SortableName)}: {SortableName}," +
                   $"\n{nameof(ShortName)}: {ShortName}," +
                   $"\n{nameof(SisUserId)}: {SisUserId}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(LoginId)}: {LoginId}," +
                   $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                   $"\n{nameof(Enrollments)}: {Enrollments}," +
                   $"\n{nameof(Email)}: {Email}," +
                   $"\n{nameof(Locale)}: {Locale}," +
                   $"\n{nameof(EffectiveLocale)}: {EffectiveLocale}," +
                   $"\n{nameof(LastLogin)}: {LastLogin}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Bio)}: {Bio}," + 
                   $"\n{nameof(Permissions)}: {string.Join("; ", Permissions ?? new Dictionary<string, bool>())}";
        }
    }
}