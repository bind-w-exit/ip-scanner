using Windows.UI.Xaml;

namespace IpScanner.Ui.Printing
{
    public interface IPrintElementRepository
    {
        void AddElements(params FrameworkElement[] elements);
        void ClearElements();
        FrameworkElement GetElementToPrint();
    }
}
