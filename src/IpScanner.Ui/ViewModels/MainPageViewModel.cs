using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Ui.ViewModels.Modules.Menu;

namespace IpScanner.Ui.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private readonly MenuViewModule _viewModule;
        private readonly MenuFileModule _fileModule;
        private readonly MenuSettingsModule _settingsModule;
        
        public MainPageViewModel(MenuViewModule viewModule, MenuFileModule menuFileModule, MenuSettingsModule menuSettingsModule)
        {
            _viewModule = viewModule;
            _fileModule = menuFileModule;
            _settingsModule = menuSettingsModule;
        }

        public MenuViewModule ViewModule => _viewModule;

        public MenuFileModule FileModule => _fileModule;

        public MenuSettingsModule SettingsModule => _settingsModule;
    }
}
