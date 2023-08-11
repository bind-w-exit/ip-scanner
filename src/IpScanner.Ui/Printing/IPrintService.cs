using IpScanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace IpScanner.Ui.Printing
{
    public interface IPrintService<T>
    {
        Task ShowPrintUIAsync(IEnumerable<T> devicesToPrint);
    }
}
