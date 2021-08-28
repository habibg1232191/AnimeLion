using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using AnimeLion.Parser.Models;
using Newtonsoft.Json;
using ReactiveUI;
using Splat;

namespace AnimeLion.ViewModels
{
    public class HomePageViewModel : ReactiveObject, IRoutableViewModel
    {
        private ObservableCollection<TopAnimeCurrentYear> _topAnimeCurrentYear;
        private bool isLoadedTopAnimeCurrentYears = false;
        
        public bool IsLoadedTopAnimeCurrentYears
        {
            get => isLoadedTopAnimeCurrentYears;
            set => this.RaiseAndSetIfChanged(ref isLoadedTopAnimeCurrentYears, value);
        }

        public ObservableCollection<TopAnimeCurrentYear> TopAnimeCurrentYears
        {
            get => _topAnimeCurrentYear;
            set => this.RaiseAndSetIfChanged(ref _topAnimeCurrentYear, value);
        }

        public HomePageViewModel(IScreen? screen = null)
        {
            Console.WriteLine("HomePageViewModel");
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:5000/api/top_anime_current_year");
            
            request.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var r = new StreamReader(stream);

            var dwString = r.ReadToEnd();

            TopAnimeCurrentYears = JsonConvert.DeserializeObject<ObservableCollection<TopAnimeCurrentYear>>(dwString) ?? new ObservableCollection<TopAnimeCurrentYear>();
            IsLoadedTopAnimeCurrentYears = true;
            r.Close();
            stream.Close();
        }
        
        public HomePageViewModel()
        {
        }

        public bool CheckInternet()
        {
            try
            {
                new WebClient().DownloadString("https://google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public string? UrlPathSegment { get; } = "/homepage";
        public IScreen HostScreen { get; }
    }
}