using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace IpScanner.Ui.Printing
{
    public class PrintElementRepository : IPrintElementRepository
    {
        private readonly List<FrameworkElement> _elements;

        public PrintElementRepository()
        {
            _elements = new List<FrameworkElement>();
        }

        public void AddElements(params FrameworkElement[] elements)
        {
            _elements.AddRange(elements);
        }

        public void ClearElements()
        {
            _elements.Clear();
        }

        public FrameworkElement GetElementToPrint()
        {
            return _elements.First(x => x.Visibility == Visibility.Visible);
        }
    }
}
