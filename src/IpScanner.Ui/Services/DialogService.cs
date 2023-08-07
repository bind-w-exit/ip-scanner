using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace IpScanner.Ui.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowMessageAsync(string title, string message)
        {
            var dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }
    }
}
