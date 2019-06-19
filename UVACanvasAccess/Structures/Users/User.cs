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

        private string _name;

        public string Name {
            get => _name;
            set {
                var _ = _api.EditUser(new[] {("name", value)}, Id).Result;
                _name = value;
            }
        }

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
        public string EffectiveLocale { get; private set; }
        public string LastLogin { get; private set; }
        public string TimeZone { get; private set; }

        private string _bio;
        public string Bio {
            get => _bio;
            set {
                var _ = _api.EditUser(new[] {("bio", value)}, Id).Result;
                _bio = value;
            }
        }
        
        public Dictionary<string, bool> Permissions { get; private set; }

        public User(Api api, UserModel model) {
            _api = api;
            Id = model.id;
            _name = model.name;
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
            EffectiveLocale = model.effective_locale;
            LastLogin = model.last_login;
            TimeZone = model.time_zone;
            _bio = model.bio;
            Permissions = model.permissions;
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
                   $"\n{nameof(EffectiveLocale)}: {EffectiveLocale}," +
                   $"\n{nameof(LastLogin)}: {LastLogin}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Bio)}: {Bio}," + 
                   $"\n{nameof(Permissions)}: {string.Join(";", Permissions)}";
        }
    }
}