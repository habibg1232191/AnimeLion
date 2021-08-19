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
            Locator.CurrentMutable.RegisterConstant<IScreen>(new MainWindowViewModel());
            Locator.CurrentMutable.Register<IViewFor<HomePageViewModel>>(() => new HomePage());

            new MainWindow{DataContext = new MainWindowViewModel()}.Show();
            base.OnFrameworkInitializationCompleted();
        }
    }
}