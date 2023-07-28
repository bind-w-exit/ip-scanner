using System.Threading.Tasks;
using Windows.Globalization;

namespace IpScanner.Ui.Services
{
    public interface ILocalizationService
    {
        Task SetLanguageAsync(Language language);
    }
}