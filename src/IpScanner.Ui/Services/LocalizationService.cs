using System.Threading.Tasks;
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

        private Task WaitForLanguageChangeAsync()
        {
            return Task.Delay(50);
        }
    }
}
