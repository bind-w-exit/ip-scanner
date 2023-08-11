using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace IpScanner.Ui.Printing
{
    public class PrintService<T> : IPrintService<T>
    {
        private PrintHelper _printHelper;
        private readonly IPanelContainer _panelContainer;

        public PrintService(IPanelContainer panelContainer)
        {
            _panelContainer = panelContainer;
        }

        public async Task ShowPrintUIAsync(IEnumerable<T> itemsToPrint)
        {
            if(_panelContainer.Panel == null)
            {
                throw new System.ArgumentNullException("PanelContainer.Panel is null");
            }

            _printHelper = new PrintHelper(_panelContainer.Panel);

            var items = itemsToPrint.ToList();
            int pages = (items.Count + 9) / 10;

            for (int i = 0; i < pages; i++)
            {
                var grid = CreatePageGrid(items.Skip(i * 10).Take(10), i + 1);
                _printHelper.AddFrameworkElementToPrint(grid);
            }

            await ShowPrintDialogAsync();
        }

        private Grid CreatePageGrid(IEnumerable<T> items, int pageNumber)
        {
            var grid = new Grid
            {
                RowDefinitions =
            {
                new RowDefinition() { Height = GridLength.Auto },
                new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition() { Height = GridLength.Auto }
            }
            };

            grid.Children.Add(CreateHeader());
            grid.Children.Add(CreateDataGrid(items));
            grid.Children.Add(CreateFooter(pageNumber));

            return grid;
        }

        private TextBlock CreateHeader() => new TextBlock { Text = "Custom Print", Margin = new Thickness(0, 0, 0, 20) };

        private DataGrid CreateDataGrid(IEnumerable<T> items)
        {
            var dataGrid = new DataGrid
            {
                AutoGenerateColumns = true,
                ItemsSource = items
            };

            Grid.SetRow(dataGrid, 1);

            return dataGrid;
        }

        private TextBlock CreateFooter(int pageNumber)
        {
            var footer = new TextBlock { Text = $"page {pageNumber}", Margin = new Thickness(0, 20, 0, 0) };
            Grid.SetRow(footer, 2);

            return footer;
        }

        private async Task ShowPrintDialogAsync()
        {
            var printHelperOptions = new PrintHelperOptions(false)
            {
                Orientation = PrintOrientation.Default
            };

            printHelperOptions.AddDisplayOption(StandardPrintTaskOptions.Orientation);
            await _printHelper.ShowPrintUIAsync("print sample", printHelperOptions);
        }
    }
}
