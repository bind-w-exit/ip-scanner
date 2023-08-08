using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using IpScanner.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.Domain
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureDomainServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<IpRange>, IpRangeValidator>();
            services.AddTransient<INetworkScannerFactory, NetworkScannerFactory>();

            return services;
        }
    }
}
