using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Domain.Validators;
using IpScanner.Infrastructure.ContentCreators;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Factories;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using IpScanner.Ui.Printing;
using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels;
using IpScanner.Ui.ViewModels.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.Ui
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IManufactorRepository, ManufacturerCsvRepository>();
            services.AddSingleton<IMacAddressRepository, MacAddressArpRepository>();
            services.AddSingleton<IHostRepository, HostDnsRepository>();

            services.AddTransient<IValidator<IpRange>, IpRangeValidator>();

            services.AddTransient<INetworkScannerFactory, NetworkScannerFactory>();

            services.AddSingleton<MainPageViewModel>();
            services.AddSingleton<ScanPageViewModel>();
            services.AddSingleton<DetailsPageViewModel>();
            services.AddSingleton<ColorThemePageViewModel>();

            services.AddSingleton<FavoritesDevicesModule>();
            services.AddSingleton<ProgressModule>();
            services.AddSingleton<IpRangeModule>();
            services.AddSingleton<ScanningModule>();

            services.AddTransient<INavigationService, NavigationService>();
            services.AddTransient<ILocalizationService, LocalizationService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IApplicationService, ApplicationService>();

            services.AddTransient<IDeviceRepositoryFactory, DeviceRepositoryFactory>();

            services.AddTransient<IContentCreatorFactory<ScannedDevice>, DeviceContentCreatorFactory>();
            services.AddTransient<IContentFormatterFactory<DeviceEntity>, ContentFormatterFactory<DeviceEntity>>();

            services.AddTransient<DevicesJsonContentCreator>();
            services.AddTransient<DevicesXmlContentCreator>();
            services.AddTransient<DevicesCsvContentCreator>();
            services.AddTransient<DevicesHtmlContentCreator>();

            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IPrintServiceFactory, PrintServiceFactory>();

            services.AddSingleton<IMessenger, StrongReferenceMessenger>();

            services.AddSingleton<IColorThemeService, ColorThemeService>();

            return services;
        }
    }
}
