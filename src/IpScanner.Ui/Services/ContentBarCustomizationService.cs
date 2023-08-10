using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Services
{
    public class ContentBarCustomizationService
    {
        private readonly CoreApplicationViewTitleBar coreTitleBar;
        private readonly UIElement AppTitleBar;
        private readonly ColumnDefinition LeftPaddingColumn;
        private readonly ColumnDefinition RightPaddingColumn;

        public ContentBarCustomizationService(CoreApplicationViewTitleBar titleBar, UIElement appTitleBar, 
            ColumnDefinition left, ColumnDefinition right)
        {
            coreTitleBar = titleBar;
            AppTitleBar = appTitleBar;
            LeftPaddingColumn = left;
            RightPaddingColumn = right;
        }

        public void Customize()
        {
            coreTitleBar.ExtendViewIntoTitleBar = true;

            SetCaptionButtonsBackground();
            SetInactiveWindowColors();

            Window.Current.SetTitleBar(AppTitleBar);

            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SetCaptionButtonsBackground()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
        }

        private void SetInactiveWindowColors()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.InactiveForegroundColor = Colors.Black;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Black;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
