using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Ui.Messages;
using IpScanner.Ui.Printing;
using IpScanner.Ui.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class ScanPage : Page
    {
        public ScanPage()
        {
            this.InitializeComponent();
            this.InitializePanelContainer();
            this.InitializeMessanger();

            DataContext = Ioc.Default.GetService<ScanPageViewModel>();
            DetailsFrame.Navigate(typeof(DetailsPage));
        }

        private void InitializePanelContainer()
        {
            IPanelContainer panelContainer = Ioc.Default.GetService<IPanelContainer>();
            panelContainer.Inialize(CustomPrintContainer);
        }

        private void InitializeMessanger()
        {
            var messenger = Ioc.Default.GetService<IMessenger>();
            messenger.Register<SetFocusToCellMessage>(this, this.SetFocusToCell);
        }

        private void FavoritesDevicesDataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.EditingElement is TextBox textbox)
            {
                textbox.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }

        private void SetFocusToCell(object sender, SetFocusToCellMessage message)
        {
            DataGrid selected = message.Favorite 
                ? FavoritesDevicesDataGrid 
                : ScannedDevicesDataGrid;

            selected.CurrentColumn = selected.Columns[(int)message.Row];
            selected.BeginEdit();
        }
    }
}
