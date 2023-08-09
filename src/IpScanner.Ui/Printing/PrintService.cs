using Windows.Graphics.Printing;
using Windows.UI.Xaml.Printing;
using Windows.UI.Xaml;
using System;

namespace IpScanner.Ui.Printing
{
    public class PrintService : IPrintService
    {
        private PrintDocument _printDocument;
        private IPrintDocumentSource _printDocumentSource;
        private FrameworkElement _printContent;

        public PrintService(FrameworkElement printContent)
        {
            this._printContent = printContent;
            _printDocument = new PrintDocument();
            _printDocumentSource = _printDocument.DocumentSource;
            _printDocument.Paginate += OnPaginate;
            _printDocument.GetPreviewPage += OnGetPreviewPage;
            _printDocument.AddPages += OnAddPages;
        }

        public async void ShowPrintUIAsync()
        {
            PrintManager printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested += OnPrintTaskRequested;
            await PrintManager.ShowPrintUIAsync();

            printManager.PrintTaskRequested -= OnPrintTaskRequested;
        }

        private void OnPaginate(object sender, PaginateEventArgs e)
        {
            _printDocument.SetPreviewPageCount(1, PreviewPageCountType.Final);
        }

        private void OnGetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            _printDocument.SetPreviewPage(e.PageNumber, _printContent);
        }

        private void OnAddPages(object sender, AddPagesEventArgs e)
        {
            _printDocument.AddPage(_printContent);
            _printDocument.AddPagesComplete();
        }

        private void OnPrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            PrintTask printTask = args.Request.CreatePrintTask("Print Job", sourceRequested =>
            {
                sourceRequested.SetSource(_printDocumentSource);
            });
        }
    }
}
