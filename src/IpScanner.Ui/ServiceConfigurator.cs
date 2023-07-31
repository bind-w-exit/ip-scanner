using IpScanner.Domain.Factories;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Domain.Validators;
using IpScanner.Infrastructure;
using IpScanner.Ui.Services;
using IpScanner.Ui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.Ui
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IManufactorReceiver, ManufacturerFileRepository>();

            services.AddSingleton<IMacAddressScanner, ArpMacAddressScanner>();

            services.AddTransient<IValidator<IpRange>, IpRangeValidator>();

            services.AddTransient<IIpScannerFactory, IpScannerFactory>();

            services.AddSingleton<MainPageViewModel>();
            services.AddSingleton<ScanPageViewModel>();

            services.AddTransient<INavigationService, NavigationService>();
            services.AddTransient<ILocalizationService, LocalizationService>();

            return services;
        }
    }
}
