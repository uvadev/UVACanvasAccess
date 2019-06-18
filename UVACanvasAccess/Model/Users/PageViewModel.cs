namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PageViewModel {
        public string id { get; set; }
        public string app_name { get; set; }
        public string url { get; set; }
        public string context_type { get; set; }
        public string asset_type { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public bool contributed { get; set; }
        public double interaction_seconds { get; set; }
        public string created_at { get; set; }
        public bool user_request { get; set; }
        public double render_time { get; set; }
        public string user_agent { get; set; }
        public bool participated { get; set; }
        public string http_method { get; set; }
        public string remote_ip { get; set; }
        public PageViewLinksModel links { get; set; }

        public override string ToString() {
            return $"{nameof(id)}: {id}," +
                   $"\n{nameof(app_name)}: {app_name}," +
                   $"\n{nameof(url)}: {url}," +
                   $"\n{nameof(context_type)}: {context_type}," +
                   $"\n{nameof(asset_type)}: {asset_type}," +
                   $"\n{nameof(controller)}: {controller}," +
                   $"\n{nameof(action)}: {action}," +
                   $"\n{nameof(contributed)}: {contributed}," +
                   $"\n{nameof(interaction_seconds)}: {interaction_seconds}," +
                   $"\n{nameof(created_at)}: {created_at}," +
                   $"\n{nameof(user_request)}: {user_request}," +
                   $"\n{nameof(render_time)}: {render_time}," +
                   $"\n{nameof(user_agent)}: {user_agent}," +
                   $"\n{nameof(participated)}: {participated}," +
                   $"\n{nameof(http_method)}: {http_method}," +
                   $"\n{nameof(remote_ip)}: {remote_ip}," +
                   $"\n{nameof(links)}: {links}";
        }
    }
}