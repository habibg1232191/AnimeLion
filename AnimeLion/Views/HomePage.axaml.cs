using AnimeLion.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace AnimeLion.Views
{
    public class HomePage : ReactiveUserControl<HomePageViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}