namespace UVACanvasAccess.Model.Users {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PageViewLinksModel {
        public ulong user { get; set; }
        public ulong context { get; set; }
        public ulong? asset { get; set; }
        public ulong? real_user { get; set; }
        public ulong account { get; set; }

        public override string ToString() {
            return $"{nameof(user)}: {user}," +
                   $"\n{nameof(context)}: {context}," +
                   $"\n{nameof(asset)}: {asset}," +
                   $"\n{nameof(real_user)}: {real_user}," +
                   $"\n{nameof(account)}: {account}";
        }
    }
}