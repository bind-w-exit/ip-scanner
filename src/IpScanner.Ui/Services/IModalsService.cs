using System;
using System.Threading.Tasks;

namespace IpScanner.Ui.Services
{
    public interface IModalsService
    {
        Task ShowPageAsync(Type pageType);
    }
}
