using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace IpScanner.Ui.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string title, string message);
    }
}
