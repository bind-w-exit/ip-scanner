using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;

namespace IpScanner.Ui.Services
{
    public class LocalizationService : ILocalizationService
    {
        public async Task SetLanguageAsync(Language language)
        {
            ApplicationLanguages.PrimaryLanguageOverride = language.LanguageTag;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();

            await WaitForLanguageChangeAsync();
        }

        public string GetLocalizedString(string key)
        {
            string message = ResourceLoader.GetForCurrentView().GetString(key);
            if(string.IsNullOrEmpty(message))
            {
                throw new System.Exception($"Key '{key}' not found in resources");
            }

            return message;
        }

        private Task WaitForLanguageChangeAsync()
        {
            return Task.Delay(50);
        }
    }
}
