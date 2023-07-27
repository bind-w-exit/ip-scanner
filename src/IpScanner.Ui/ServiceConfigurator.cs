using IpScanner.Domain.Factories;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Validators;
using IpScanner.Infrastructure;
using IpScanner.Infrastructure.APIs;
using IpScanner.Ui.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.Ui
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IManufactorReceiver, ManufactorApi>();
            services.AddHttpClient<IManufactorReceiver, ManufactorApi>();

            services.AddSingleton<IMacAddressScanner, ArpMacAddressScanner>();

            services.AddTransient<IValidator<string>, IpRangeValidator>();

            services.AddTransient<IIpScannerFactory, IpScannerFactory>();

            services.AddSingleton<MainPageViewModel>();

            return services;
        }
    }
}
