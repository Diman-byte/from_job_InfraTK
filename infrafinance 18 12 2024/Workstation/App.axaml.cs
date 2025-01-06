using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Workstation.ViewModels;
using Workstation.Views;

namespace Workstation
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new LaunchWindow
                {
                    DataContext = new LaunchWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}