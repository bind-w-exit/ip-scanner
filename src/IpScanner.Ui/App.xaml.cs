using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Domain;
using IpScanner.Infrastructure;
using IpScanner.Ui.Services;
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
        private IServiceProvider _serviceProvider;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            _serviceCollection = new ServiceCollection();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            _serviceCollection.ConfigureUiServices()
                .ConfigureInfrastructureServices()
                .ConfigureDomainServices();

            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            _serviceCollection.AddSingleton(rootFrame);
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            ConfigureIoc(_serviceProvider);

            _serviceProvider.GetRequiredService<ISettingsService>();

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

            if(_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not initialized");
            }

            var settingsService = _serviceProvider.GetRequiredService<ISettingsService>();
            settingsService.SaveSettings();

            deferral.Complete();
        }

        private void ConfigureIoc(IServiceProvider serviceProvider)
        {
             Ioc.Default.ConfigureServices(serviceProvider);
        }
    }
}
