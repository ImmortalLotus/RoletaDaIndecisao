using DynamicData.Binding;
using ReactiveUI;
using Roleta.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Roleta.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private string _Spin;
        private string _ReverseAngle;


        private ObservableCollection<Filme> _Filmes;
        public ObservableCollection<Filme> Filmes { get=>_Filmes; set => this.RaiseAndSetIfChanged(ref _Filmes, value); }
        public int PontoInvisivel { get; set; }
        public int FilmeInvisivel { get; set; }
        public string Angle
        {
            get => _Spin;
            set => this.RaiseAndSetIfChanged(ref _Spin, value);
        }

        public string ReverseAngle
        {
            get => _ReverseAngle;
            set => this.RaiseAndSetIfChanged(ref _Spin, value);
        }
        public MainWindowViewModel()
        {
            Angle = "rotate(10deg)";
            this.Filmes = new ObservableCollection<Filme>();
        }
    }
}