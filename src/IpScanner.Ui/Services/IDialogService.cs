using System.Threading.Tasks;

namespace IpScanner.Ui.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string title, string message);
    }
}
