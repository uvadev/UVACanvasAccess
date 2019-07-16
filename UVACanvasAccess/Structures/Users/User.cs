using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using StatePrinting;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Model.Users;
using UVACanvasAccess.Structures.Enrollments;
using UVACanvasAccess.Util;

namespace UVACanvasAccess.Structures.Users {
    
    [PublicAPI]
    public class User : IPrettyPrint {
        // We keep a reference to the API that yielded this User so that getters can query for needed info and setters
        // can update the API. Many API endpoints that return User omit some fields, so some getters will need to check
        // for null and query the API if required. Setters that do not have an implementation that directly updates the
        // API should remain private.
        private readonly Api _api;
        
        public ulong Id { get; }

        private string _name;
        public string Name {
            get => _name;
            set {
                var _ = _api.EditUser(new[] {("name", value)}, Id).Result;
                _name = value;
            }
        }

        private string _sortableName;
        public string SortableName {
            get => _sortableName;
            set {
                var _ = _api.EditUser(new[] {("sortable_name", value)}, Id).Result;
                _sortableName = value; 
            }
        }

        private string _shortName;
        public string ShortName {
            get => _shortName;
            set {
                var _ = _api.EditUser(new[] {("short_name", value)}, Id).Result;
                _shortName = value;
            }
        }
        
        public string SisUserId { get; private set; }
        public ulong? SisImportId { get; private set; }
        public string IntegrationId { get; private set; }
        public string LoginId { get; private set; }
        public string AvatarUrl { get; private set; }
        public IEnumerable<Enrollment> Enrollments { get; private set; }
        public string Email { get; private set; }
        public string Locale { get; private set; }
        public string EffectiveLocale { get; private set; }
        public DateTime? LastLogin { get; }

        private string _timeZone;
        public string TimeZone {
            get => _timeZone;
            set {
                var _ = _api.EditUser(new[] {("time_zone", value)}).Result;
                _timeZone = value;
            } 
        }

        private string _bio;
        public string Bio {
            get => _bio;
            set {
                var _ = _api.EditUser(new[] {("bio", value)}, Id).Result;
                _bio = value;
            }
        }
        
        public Dictionary<string, bool> Permissions { get; private set; }

        internal User(Api api, UserModel model) {
            _api = api;
            Id = model.Id;
            _name = model.Name;
            _sortableName = model.SortableName;
            _shortName = model.ShortName;
            SisUserId = model.SisUserId;
            SisImportId = model.SisImportId;
            IntegrationId = model.IntegrationId;
            LoginId = model.LoginId;
            AvatarUrl = model.AvatarUrl;
            Enrollments = model.Enrollments.SelectNotNull(m => new Enrollment(api, m));
            Email = model.Email;
            Locale = model.Locale;
            EffectiveLocale = model.EffectiveLocale;
            LastLogin = model.LastLogin;
            _timeZone = model.TimeZone;
            _bio = model.Bio;
            Permissions = model.Permissions ?? new Dictionary<string, bool>();
        }

        public Task<Profile> GetProfile() {
            return _api.GetUserProfile(Id);
        }

        public Task<IEnumerable<PageView>> GetPageViews(DateTime? startDate = null, DateTime? endDate = null) {
            return _api.GetUserPageViews(Id, startDate, endDate);
        }

        private static readonly Stateprinter Printer = new Stateprinter();
        public override string ToString() {
            return Printer.PrintObject(this);
        }

        public string ToPrettyString() {
            return "User {" + 
                   ($"\n{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(SortableName)}: {SortableName}," +
                   $"\n{nameof(ShortName)}: {ShortName}," +
                   $"\n{nameof(SisUserId)}: {SisUserId}," +
                   $"\n{nameof(SisImportId)}: {SisImportId}," +
                   $"\n{nameof(IntegrationId)}: {IntegrationId}," +
                   $"\n{nameof(LoginId)}: {LoginId}," +
                   $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                   $"\n{nameof(Enrollments)}: {Enrollments.ToPrettyString()}," +
                   $"\n{nameof(Email)}: {Email}," +
                   $"\n{nameof(Locale)}: {Locale}," +
                   $"\n{nameof(EffectiveLocale)}: {EffectiveLocale}," +
                   $"\n{nameof(LastLogin)}: {LastLogin}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Bio)}: {Bio}," +
                   $"\n{nameof(Permissions)}: {Permissions.ToPrettyString()}").Indent(4) +
                   "\n}";
        }
    }
}