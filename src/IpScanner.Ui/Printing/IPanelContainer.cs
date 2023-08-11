using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Printing
{
    public interface IPanelContainer
    {
        Panel Panel { get; }

        void Inialize(Panel panel);
    }
}
