using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Models;
using IpScanner.Ui.Printing;
using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels;
using IpScanner.Ui.ViewModels.Modules.Menu;
using IpScanner.Ui.ViewModels.Modules.Scanning;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.Ui
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureUiServices(this IServiceCollection services)
        {
            services.AddSingleton<MainPageViewModel>();
            services.AddSingleton<ScanPageViewModel>();
            services.AddSingleton<DetailsPageViewModel>();
            services.AddSingleton<ColorThemePageViewModel>();

            services.AddSingleton<FavoritesDevicesModule>();
            services.AddSingleton<ProgressModule>();
            services.AddSingleton<IpRangeModule>();
            services.AddSingleton<ScanningModule>();

            services.AddSingleton<MenuViewModule>();
            services.AddSingleton<MenuFileModule>();
            services.AddSingleton<MenuSettingsModule>();
            services.AddSingleton<MenuHelpModule>();

            services.AddTransient<INavigationService, NavigationService>();

            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddTransient<IDialogService, DialogService>();

            services.AddTransient<IApplicationService, ApplicationService>();

            services.AddSingleton<IPanelContainer, PanelContainer>();

            services.AddTransient<IPrintService<ScannedDevice>, PrintService<ScannedDevice>>();

            services.AddSingleton<IMessenger, StrongReferenceMessenger>();

            services.AddSingleton<IColorThemeService, ColorThemeService>();

            services.AddTransient<IModalsService, ModalsService>();

            return services;
        }
    }
}
