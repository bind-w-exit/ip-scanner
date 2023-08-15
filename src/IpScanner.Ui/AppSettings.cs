using System;

namespace IpScanner.Ui
{
    public class AppSettings
    {
        public AppSettings()
        {
            ShowUnknown = false;
            ShowOffline = true;
            ShowOnline = true;
        }

        public bool ShowUnknown { get; set; }
        public bool ShowOffline { get; set; }
        public bool ShowOnline { get; set; }
    }
}
