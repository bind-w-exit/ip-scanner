﻿using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Interfaces;
using IpScanner.Domain.Models;
using IpScanner.Domain.Validators;
using IpScanner.Infrastructure.Repositories;
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
            services.AddSingleton<FavoritesDevicesModule>();

            services.AddTransient<INavigationService, NavigationService>();
            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddTransient<IDeviceRepository, DevicesJsonRepository>();

            services.AddSingleton<IMessenger, StrongReferenceMessenger>();

            return services;
        }
    }
}
