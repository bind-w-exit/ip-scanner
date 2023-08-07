using Windows.UI.Xaml;

namespace IpScanner.Ui.Printing
{
    public interface IPrintServiceFactory
    {
        IPrintService CreateBasedOneFrameworkElement(FrameworkElement elementToPrint);
    }
}
