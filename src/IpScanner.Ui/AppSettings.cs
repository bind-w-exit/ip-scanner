namespace IpScanner.Ui
{
    public class AppSettings
    {
        public AppSettings()
        {
            ShowOffline = true;
            ShowOnline = true;
            ShowUnknown = false;
            ShowMiscellaneousToolbar = true;
            ShowActionsToolbar = true;
            ShowDetails = false;
            FavoritesSelected = false;
            IpRange = string.Empty;
        }

        public bool ShowUnknown { get; set; }
        public bool ShowOffline { get; set; }
        public bool ShowOnline { get; set; }
        public bool ShowMiscellaneousToolbar { get; set; }
        public bool ShowActionsToolbar { get; set; }
        public bool ShowDetails { get; set; }
        public bool FavoritesSelected { get; set; }
        public string IpRange { get; set; }
    }
}
