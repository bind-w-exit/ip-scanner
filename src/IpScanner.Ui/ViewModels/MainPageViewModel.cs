using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Ui.ViewModels.Modules.Menu;

namespace IpScanner.Ui.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private readonly MenuViewModule _viewModule;
        private readonly MenuFileModule _fileModule;
        private readonly MenuSettingsModule _settingsModule;
        private readonly MenuHelpModule _helpModule;

        public MainPageViewModel(MenuViewModule viewModule, MenuFileModule menuFileModule,
            MenuSettingsModule menuSettingsModule, MenuHelpModule helpModule)
        {
            _viewModule = viewModule;
            _fileModule = menuFileModule;
            _settingsModule = menuSettingsModule;
            _helpModule = helpModule;
        }

        public MenuViewModule ViewModule => _viewModule;

        public MenuFileModule FileModule => _fileModule;

        public MenuSettingsModule SettingsModule => _settingsModule;

        public MenuHelpModule HelpModule => _helpModule;
    }
}
