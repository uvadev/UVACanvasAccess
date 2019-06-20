using UVACanvasAccess.Model.Users;

namespace UVACanvasAccess.Structures.Users {

    // ReSharper disable UnusedAutoPropertyAccessor.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable MemberCanBePrivate.Global
    public class Profile {
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

        private string _title;
        public string Title {
            get => _title;
            set {
                var _ = _api.EditUser(new[] {("title", value)}, Id).Result;
                _title = value;
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
        
        public string PrimaryEmail { get; private set; }
        public string LoginId { get; private set; }
        public string SisUserId { get; private set; }
        public string LtiUserId { get; private set; }
        public string AvatarUrl { get; private set; }
        public object Calendar { get; private set; }
        public string TimeZone { get; private set; }
        public string Locale { get; private set; }

        public Profile(Api api, ProfileModel model) {
            _api = api;
            
            Id = model.id;
            _name = model.name;
            _shortName = model.short_name;
            _sortableName = model.sortable_name;
            _title = model.title;
            _bio = model.bio;
            PrimaryEmail = model.primary_email;
            LoginId = model.login_id;
            SisUserId = model.sis_user_id;
            LtiUserId = model.lti_user_id;
            AvatarUrl = model.avatar_url;
            Calendar = model.calendar;
            TimeZone = model.time_zone;
            Locale = model.locale;
        }

        public override string ToString() {
            return $"{nameof(Id)}: {Id}," +
                   $"\n{nameof(Name)}: {Name}," +
                   $"\n{nameof(ShortName)}: {ShortName}," +
                   $"\n{nameof(SortableName)}: {SortableName}," +
                   $"\n{nameof(Title)}: {Title}," +
                   $"\n{nameof(Bio)}: {Bio}," +
                   $"\n{nameof(PrimaryEmail)}: {PrimaryEmail}," +
                   $"\n{nameof(LoginId)}: {LoginId}," +
                   $"\n{nameof(SisUserId)}: {SisUserId}," +
                   $"\n{nameof(LtiUserId)}: {LtiUserId}," +
                   $"\n{nameof(AvatarUrl)}: {AvatarUrl}," +
                   $"\n{nameof(Calendar)}: {Calendar}," +
                   $"\n{nameof(TimeZone)}: {TimeZone}," +
                   $"\n{nameof(Locale)}: {Locale}";
        }
    }
}