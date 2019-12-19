using Newtonsoft.Json;


namespace UVACanvasAccess.Model.Users {
    internal class ProfileModel {
        
        [JsonProperty("id")]
        public ulong Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        
        [JsonProperty("sortable_name")]
        public string SortableName { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("bio")]
        public string Bio { get; set; }
        
        [JsonProperty("primary_email")]
        public string PrimaryEmail { get; set; }
        
        [JsonProperty("login_id")]
        public string LoginId { get; set; }
        
        [JsonProperty("sis_user_id")]
        public string SisUserId { get; set; }
        
        [JsonProperty("lti_user_id")]
        public string LtiUserId { get; set; }
        
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonProperty("calendar")]
        public object Calendar { get; set; }
        
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }
        
        [JsonProperty("locale")]
        public string Locale { get; set; }

        
    }
}