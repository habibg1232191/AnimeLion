using System;
using System.Windows.Input;
using ReactiveUI;

namespace AnimeLion.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IScreen
    {
        private readonly object _homePage;
        private RoutingState _router = new ();
        private object _currentPage;
        
        public object CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public ICommand Home
        {
            get;
            set;
        }

        public RoutingState Router
        {
            get => _router;
            set => this.RaiseAndSetIfChanged(ref _router, value);
        }
        
        public MainWindowViewModel()
        {
            
        }

        public MainWindowViewModel(IScreen? screen = null)
        {
            _homePage = new HomePageViewModel(null);
            _currentPage = _homePage;
            Home = ReactiveCommand.Create(() =>
            {
                if(CurrentPage != _homePage)
                    CurrentPage = _homePage;
            });
        }

        public async void Test()
        {
            Console.WriteLine("CTRL + 1");
        }
    }
}