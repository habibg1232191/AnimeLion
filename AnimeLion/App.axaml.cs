using Avalonia;
using Avalonia.Markup.Xaml;
using AnimeLion.ViewModels;
using AnimeLion.Views;
using ReactiveUI;
using Splat;

namespace AnimeLion
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var mainWindowViewModel = new MainWindowViewModel(null);
            Locator.CurrentMutable.RegisterConstant<IScreen>(mainWindowViewModel);
            Locator.CurrentMutable.Register<IViewFor<HomePageViewModel>>(() => new HomePage());

            new MainWindow{ DataContext = mainWindowViewModel }.Show();
            base.OnFrameworkInitializationCompleted();
        }
    }
}