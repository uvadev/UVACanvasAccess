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
        
        private string _timeZone;
        public string TimeZone {
            get => _timeZone;
            set {
                var _ = _api.EditUser(new[] {("time_zone", value)}).Result;
                _timeZone = value;
            } 
        }
        
        public string Locale { get; private set; }

        public Profile(Api api, ProfileModel model) {
            _api = api;
            
            Id = model.Id;
            _name = model.Name;
            _shortName = model.ShortName;
            _sortableName = model.SortableName;
            _title = model.Title;
            _bio = model.Bio;
            PrimaryEmail = model.PrimaryEmail;
            LoginId = model.LoginId;
            SisUserId = model.SisUserId;
            LtiUserId = model.LtiUserId;
            AvatarUrl = model.AvatarUrl;
            Calendar = model.Calendar;
            _timeZone = model.TimeZone;
            Locale = model.Locale;
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