using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Domain.Args;
using IpScanner.Domain.Exceptions;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Models;
using IpScanner.Ui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.System;

namespace IpScanner.Ui.ViewModels
{
    public class MainPageViewModel : ValidationViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ILocalizationService _localizationService;

        public MainPageViewModel(INavigationService navigationService, ILocalizationService localizationService)
        {
            _navigationService = navigationService;
            _localizationService = localizationService;
        }

        
        public AsyncRelayCommand<string> ChangeLanguageCommand { get => new AsyncRelayCommand<string>(ChangeLanguageAsync); }

        private async Task ChangeLanguageAsync(string language)
        {
            await _localizationService.SetLanguageAsync(new Language(language));
            _navigationService.ReloadMainPage();
        }
    }
}
