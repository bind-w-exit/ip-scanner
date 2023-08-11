using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Printing
{
    public class PanelContainer : IPanelContainer
    {
        public Panel Panel { get; private set; }

        public void Inialize(Panel panel)
        {
            if(panel == null)
            {
                throw new System.ArgumentNullException(nameof(panel));
            }

            Panel = panel;
        }
    }
}
