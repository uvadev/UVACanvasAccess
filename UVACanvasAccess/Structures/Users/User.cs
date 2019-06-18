using System.Collections.Generic;
using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Structures.Users {
    
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class User {
        // We keep a reference to the API that yielded this User so that getters can query for needed info and setters
        // can update the API. Many API endpoints that return User omit some fields, so some getters will need to check
        // for null and query the API if required. Setters that do not have an implementation that directly updates the
        // API should remain private.
        private readonly Api _api;
        
        public ulong Id { get; private set; }
        public string Name { get; private set; }
        public string SortableName { get; private set; }
        public string ShortName { get; private set; }
        public string SisUserId { get; private set; }
        public ulong? SisImportId { get; private set; }
        public string IntegrationId { get; private set; }
        public string LoginId { get; private set; }
        public string AvatarUrl { get; private set; }
        public List<object> Enrollments { get; private set; }
        public string Email { get; private set; }
        public string Locale { get; private set; }
        public string LastLogin { get; private set; }
        public string TimeZone { get; private set; }
        public string Bio { get; private set; }

        public User(Api api, UserModel model) {
            _api = api;
            Id = model.id;
            Name = model.name;
            SortableName = model.sortable_name;
            ShortName = model.short_name;
            SisUserId = model.sis_user_id;
            SisImportId = model.sis_import_id;
            IntegrationId = model.integration_id;
            LoginId = model.login_id;
            AvatarUrl = model.avatar_url;
            Enrollments = model.enrollments;
            Email = model.email;
            Locale = model.locale;
            LastLogin = model.last_login;
            TimeZone = model.time_zone;
            Bio = model.bio;
        }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}," +
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
                   $"\n{nameof(LastLogin)}: {LastLogin}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Bio)}: {Bio}";
        }
    }
}