using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IpScanner.Ui
{
    sealed partial class App : Application
    {
        private readonly IServiceCollection _serviceCollection;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            _serviceCollection = new ServiceCollection();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            _serviceCollection.ConfigureServices();

            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            _serviceCollection.AddSingleton(rootFrame);
            ConfigureIoc();

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void ConfigureIoc()
        {
             Ioc.Default.ConfigureServices(_serviceCollection.BuildServiceProvider());
        }
    }
}
