namespace IpScanner.Ui.Services
{
    public interface ISettingsService
    {
        void SaveSettings();
        AppSettings Settings { get; }
    }
}
