using Windows.UI.Xaml;

namespace IpScanner.Ui.Printing
{
    public class PrintServiceFactory : IPrintServiceFactory
    {
        public IPrintService CreateBasedOneFrameworkElement(FrameworkElement elementToPrint)
        {
            return new PrintService(elementToPrint);
        }
    }
}
