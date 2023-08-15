using Windows.Storage;

namespace IpScanner.Ui.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly AppSettings _appSettings;

        public SettingsService()
        {
            _appSettings = LoadSettings();
        }

        public void SaveSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var composite = new ApplicationDataCompositeValue();

            foreach (var property in typeof(AppSettings).GetProperties())
            {
                if (property.CanRead)
                {
                    var value = property.GetValue(_appSettings);
                    composite[property.Name] = value;
                }
            }

            localSettings.Values["AppSettings"] = composite;
        }

        public AppSettings Settings => _appSettings;

        private AppSettings LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var composite = (ApplicationDataCompositeValue)localSettings.Values["AppSettings"];

            var appSettings = new AppSettings();

            if (composite != null)
            {
                foreach (var property in typeof(AppSettings).GetProperties())
                {
                    if (property.CanWrite && composite.ContainsKey(property.Name))
                    {
                        property.SetValue(appSettings, composite[property.Name]);
                    }
                }
            }

            return appSettings;
        }
    }
}
