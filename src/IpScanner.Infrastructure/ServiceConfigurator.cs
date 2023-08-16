using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Infrastructure.ContentCreators;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Factories;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.Infrastructure
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IManufactorRepository, ManufacturerCsvRepository>();
            services.AddSingleton<IMacAddressRepository, MacAddressArpRepository>();
            services.AddSingleton<IHostRepository, HostDnsRepository>();

            services.AddTransient<IDeviceRepositoryFactory, DeviceRepositoryFactory>();

            services.AddTransient<IContentCreatorFactory<ScannedDevice>, DeviceContentCreatorFactory>();
            services.AddTransient<IContentFormatterFactory<DeviceEntity>, ContentFormatterFactory<DeviceEntity>>();

            services.AddTransient<DevicesJsonContentCreator>();
            services.AddTransient<DevicesXmlContentCreator>();
            services.AddTransient<DevicesCsvContentCreator>();
            services.AddTransient<DevicesHtmlContentCreator>();

            services.AddTransient<IFileService, FileService>();

            services.AddTransient<IUriOpenerService, UriOpenerService>();

            services.AddTransient<IFtpService, FtpService>();

            services.AddTransient<IRdpService, RdpService>();

            services.AddScoped<IWakeOnLanService, WakeOnLanService>();

            services.AddTransient<ICmdService, CmdService>();

            services.AddTransient<ITelnetService, TelnetService>();

            return services;
        }
    }
}
